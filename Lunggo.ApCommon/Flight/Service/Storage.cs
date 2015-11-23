using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Caching;
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

        private static bool GetSearchingStatusInCache(string searchId, int supplierIndex)
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

        private void InvalidateSearchingStatusInCache(string searchId, int supplierIndex)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "searchFlightStatus:" + searchId + ":" + supplierIndex;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var expiry = GetSearchedItinerariesExpiry(searchId, supplierIndex);
            redisDb.StringSet(redisKey, false, expiry - DateTime.UtcNow);
        }

        private int GetSearchingCompletenessInCache(string searchId)
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

        private void SetSearchingCompletenessInCache(string searchId, int completeness)
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

        private static void SaveSearchedItinerariesToCache(List<FlightItinerary> itineraryList, string searchId, int timeout,
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

        private static FlightItinerary GetItineraryFromSearchCache(string searchId, int registerNumber)
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

        private Dictionary<int, List<FlightItinerary>> GetSearchedSupplierItineraries(string searchId, List<int> requestedSupplierIds)
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

        private string SaveItineraryFromSearchToCache(string searchId, int registerNumber, string requestId)
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

        private string SaveItineraryToCache(FlightItinerary itin)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = CacheIdentifier.Flight + SingleItinKeyPrefix + plainItinCacheId;
            SaveItineraryToCache(itin, itinCacheId);
            return itinCacheId;
        }

        private void SaveItineraryToCache(FlightItinerary itin, string itinCacheId)
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

        private FlightItinerary GetItineraryFromCache(string itinCacheId)
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

        private string SaveItinerarySetAndBundleToCache(List<FlightItinerary> itinSet, FlightItinerary itinBundle)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = CacheIdentifier.Flight + ItinBundleKeyPrefix + plainItinCacheId;
            SaveItinerarySetAndBundleToCache(itinSet, itinBundle, itinCacheId);
            return itinCacheId;
        }

        private void SaveItinerarySetAndBundleToCache(List<FlightItinerary> itinSet, FlightItinerary itinBundle, string itinCacheId)
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

        private List<FlightItinerary> GetItinerarySetFromCache(string itinCacheId)
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

        private void SavePaymentRedirectionUrlInCache(string rsvNo, string paymentUrl, DateTime? timeLimit)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "paymentRedirectionUrl:" + rsvNo;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                redisDb.StringSet(redisKey, paymentUrl, timeLimit - DateTime.UtcNow);
            }
            catch { }
        }

        private string GetPaymentRedirectionUrlInCache(string rsvNo)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "paymentRedirectionUrl:" + rsvNo;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var redirectionUrl = redisDb.StringGet(redisKey);
                return redirectionUrl;
            }
            catch
            {
                return null;
            }
        }

        private void SaveActiveMarginRulesToCache(List<MarginRule> rules)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "activeFlightMarginRules";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = rules.ToCacheObject();
                redisDb.StringSet(redisKey, cacheObject);
            }
            catch { }
        } 

        private List<MarginRule> GetAllActiveMarginRulesFromCache()
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "activeFlightMarginRules";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);
                var marginRules = cacheObject.DeconvertTo<List<MarginRule>>();
                return marginRules;
            }
            catch
            {
                return new List<MarginRule>();
            }
        }

        private void SaveActiveMarginRulesInBufferCache(List<MarginRule> rules)
        {
            var redisService = RedisService.GetInstance();
            var rulesRedisKey = "activeFlightMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var rulesCacheObject = rules.ToCacheObject();
            redisDb.StringSet(rulesRedisKey, rulesCacheObject);
        }

        private void SaveDeletedMarginRulesInBufferCache(List<MarginRule> deletedRules)
        {
            var redisService = RedisService.GetInstance();
            var deletedRulesRedisKey = "deletedFlightMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var deletedRulesCacheObject = deletedRules.ToCacheObject();
            redisDb.StringSet(deletedRulesRedisKey, deletedRulesCacheObject);
        }

        private List<MarginRule> GetActiveMarginRulesFromBufferCache()
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "activeFlightMarginRulesBuffer";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);
                var marginRules = cacheObject.DeconvertTo<List<MarginRule>>();
                return marginRules;
            }
            catch
            {
                return new List<MarginRule>();
            }
        }

        private List<MarginRule> GetDeletedMarginRulesFromBufferCache()
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "deletedFlightMarginRulesBuffer";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);
                var marginRules = cacheObject.DeconvertTo<List<MarginRule>>();
                return marginRules;
            }
            catch
            {
                return new List<MarginRule>();
            }
        }

        public void SetFlightRequestTripType(string requestId, bool asReturn)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestAsReturn:" + requestId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeout = Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            redisDb.StringSet(redisKey, asReturn, new TimeSpan(0, 2*timeout, 0));
        }

        public bool? GetFlightRequestTripType(string requestId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestAsReturn:" + requestId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            return (bool?) redisDb.StringGet(redisKey);
        }

        public void SaveFlightRequestPrices(string requestId, string searchId, List<FlightItinerary> itins)
        {
            var cacheContent = GetFlightRequestPrices(requestId, searchId).ToDictionary(e => e.Key, e => e.Value);
            itins.ForEach(itin =>
            {
                try
                {
                    cacheContent.Add(itin.RegisterNumber, itin.LocalPrice);
                }
                catch { }
            });

            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestPrices:" + searchId + ":" + requestId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeout = Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            redisDb.StringSet(redisKey, cacheContent.ToCacheObject(), new TimeSpan(0, 2 * timeout, 0));
        }

        public Dictionary<int, decimal> GetFlightRequestPrices(string requestId, string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestPrices:" + searchId + ":" + requestId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cache = redisDb.StringGet(redisKey);
            return cache.IsNull ? new Dictionary<int, decimal>() : cache.DeconvertTo<Dictionary<int, decimal>>();
        }

        private bool IsItinBundleCacheId(string cacheId)
        {
            return cacheId.Substring(1, 4) == ItinBundleKeyPrefix;
        }
    }
}
