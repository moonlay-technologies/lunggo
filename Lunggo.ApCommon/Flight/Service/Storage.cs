using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;

using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private const string SingleItinKeyPrefix = "9284";
        private const string ItinBundleKeyPrefix = "3462";

        public List<FlightPassenger> GetSavedPassengers(string contactEmail)
        {
            return GetDb.SavedPassengers(contactEmail);
        }

        public bool GetSearchingStatusInCache(string searchId, int supplierIndex)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchFlightStatus:" + searchId + ":" + supplierIndex;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var redisTransaction = redisDb.CreateTransaction();
                redisTransaction.AddCondition(Condition.KeyNotExists(redisKey));
                redisTransaction.StringSetAsync(redisKey, true, TimeSpan.FromMinutes(5));
                var currentStatusTask = redisTransaction.StringGetAsync(redisKey);
                redisTransaction.Execute();
                return currentStatusTask.Status == TaskStatus.Canceled;
            }
            catch
            {
                return true;
            }
        }

        public void InvalidateSearchingStatusInCache(string searchId, int supplierIndex)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "searchFlightStatus:" + searchId + ":" + supplierIndex;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var expiry = GetSearchedItinerariesExpiry(searchId, supplierIndex);
            redisDb.StringSet(redisKey, false, expiry - DateTime.UtcNow);
        }

        public int GetSearchingCompletenessInCache(string searchId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchFlightCompleteness:" + searchId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var completeness = redisDb.StringGet(redisKey);
                return (int)completeness;
            }
            catch
            {
                return 0;
            }
        }

        public void SetSearchingCompletenessInCache(string searchId, int completeness)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchFlightCompleteness:" + searchId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeout =
                    Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
                redisDb.StringSet(redisKey, completeness, TimeSpan.FromMinutes(timeout));
            }
            catch { }
        }

        public void SaveSearchedItinerariesToCache(List<FlightItinerary> itineraryList, string searchId, int timeout,
            int supplierIndex)
        {
            var sequenceNo = 0;
            itineraryList.ForEach(itin => itin.RegisterNumber = (supplierIndex * 333) + sequenceNo++);

            if (timeout == 0)
                timeout =
                    Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            var redisService = RedisService.GetInstance();
            var redisKey = "searchedFlightItineraries:" + searchId + ":" + supplierIndex;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = itineraryList.ToCacheObject();
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
        }

        private FlightItinerary GetItineraryFromSearchCache(string searchId, int registerNumber)
        {
            var redisService = RedisService.GetInstance();
            var supplierIndex = registerNumber / 333;
            var redisKey = "searchedFlightItineraries:" + searchId + ":" + supplierIndex;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = redisDb.StringGet(redisKey);
            var itins = cacheObject.DeconvertTo<List<FlightItinerary>>();
            return itins.Single(itin => itin.RegisterNumber == registerNumber);
        }

        public DateTime? GetSearchedItinerariesExpiry(string searchId, int supplierIndex)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchedFlightItineraries:" + searchId + ":" + supplierIndex;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeToLive = redisDb.KeyTimeToLive(redisKey);
                var expiryTime = DateTime.UtcNow + timeToLive;
                return expiryTime;
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        public Dictionary<int, List<FlightItinerary>> GetSearchedSupplierItineraries(string searchId, List<int> requestedSupplierIds)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var searchedSupplierItins = new Dictionary<int, List<FlightItinerary>>();
            foreach (var supplierId in requestedSupplierIds)
            {
                var redisKey = "searchedFlightItineraries:" + searchId + ":" + supplierId;
                var cacheObject = redisDb.StringGet(redisKey);
                var isSearched = !cacheObject.IsNull;
                if (isSearched)
                    searchedSupplierItins.Add(supplierId, cacheObject.DeconvertTo<List<FlightItinerary>>());
            }
            return searchedSupplierItins;
        }

        internal string SaveItineraryFromSearchToCache(string searchId, int registerNumber)
        {
            try
            {
                var plainItinCacheId =
                    FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
                var itinCacheId = CacheIdentifier.Flight + SingleItinKeyPrefix + plainItinCacheId;
                var itin = GetItineraryFromSearchCache(searchId, registerNumber);
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItinerary:" + itinCacheId;
                var cacheObject = itin.ToCacheObject();
                var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
                return itinCacheId;
            }
            catch
            {
                return null;
            }
        }

        internal string SaveItineraryToCache(FlightItinerary itin)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = CacheIdentifier.Flight + SingleItinKeyPrefix + plainItinCacheId;
            SaveItineraryToCache(itin, itinCacheId);
            return itinCacheId;
        }

        internal void SaveItineraryToCache(FlightItinerary itin, string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItinerary:" + itinCacheId;
                var cacheObject = itin.ToCacheObject();
                var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
            }
            catch { }
        }

        internal FlightItinerary GetItineraryFromCache(string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItinerary:" + itinCacheId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);

                if (!cacheObject.IsNullOrEmpty)
                {
                    var itinerary = cacheObject.DeconvertTo<FlightItinerary>();
                    return itinerary;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public DateTime? GetItineraryExpiry(string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItinerary:" + itinCacheId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
                var expiryTime = DateTime.UtcNow + timeToLive;
                return expiryTime;
            }
            catch
            {
                return DateTime.UtcNow;
            }
        }

        internal string SaveItinerarySetAndBundleToCache(List<FlightItinerary> itinSet, FlightItinerary itinBundle)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = CacheIdentifier.Flight + ItinBundleKeyPrefix + plainItinCacheId;
            SaveItinerarySetAndBundleToCache(itinSet, itinBundle, itinCacheId);
            return itinCacheId;
        }

        internal void SaveItinerarySetAndBundleToCache(List<FlightItinerary> itinSet, FlightItinerary itinBundle, string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisSetKey = "flightItinerarySet:" + itinCacheId;
                var setCacheObject = itinSet.ToCacheObject();
                var redisBundleKey = "flightItinerary:" + itinCacheId;
                var bundleCacheObject = itinBundle.ToCacheObject();
                var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                redisDb.StringSet(redisSetKey, setCacheObject, TimeSpan.FromMinutes(timeout));
                redisDb.StringSet(redisBundleKey, bundleCacheObject, TimeSpan.FromMinutes(timeout));
            }
            catch { }
        }

        internal List<FlightItinerary> GetItinerarySetFromCache(string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItinerarySet:" + itinCacheId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);

                if (!cacheObject.IsNullOrEmpty)
                {
                    var itinerary = cacheObject.DeconvertTo<List<FlightItinerary>>();
                    return itinerary;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        internal void SaveRedirectionUrlInCache(string rsvNo, string paymentUrl, DateTime? timeLimit)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "rsvNoRedirectionUrl:" + rsvNo;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                redisDb.StringSet(redisKey, paymentUrl, timeLimit - DateTime.Now);
            }
            catch { }
        }

        internal string GetRedirectionUrlInCache(string rsvNo)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "rsvNoRedirectionUrl:" + rsvNo;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var redirectionUrl = redisDb.StringGet(redisKey);
                return redirectionUrl;
            }
            catch
            {
                return null;
            }
        }

        internal void DeleteRedirectionUrlInCache(string rsvNo)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "rsvNoRedirectionUrl:" + rsvNo;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                redisDb.KeyDelete(redisKey);
            }
            catch { }
        }

        private bool IsItinBundleCacheId(string cacheId)
        {
            return cacheId.Substring(1, 4) == ItinBundleKeyPrefix;
        }
    }
}
