using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private const int SupplierIndexCap = 333;

        public List<FlightPassenger> GetSavedPassengers(string contactEmail)
        {
            return GetSavedPassengersFromDb(contactEmail);
        }

        public void SaveTransacInquiryInCache(string mandiriCacheId, List<KeyValuePair<string, string>> transaction, TimeSpan timeout)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "mandiriTransactionPrice:" + mandiriCacheId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            foreach (var pair in transaction)
            {
                redisDb.HashSet(redisKey, pair.Key, pair.Value);
                redisDb.KeyExpire(redisKey, timeout);
            }

        }

        public List<KeyValuePair<string, string>> GetTransacInquiryFromCache(string mandiriCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "mandiriTransactionPrice:" + mandiriCacheId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var temp = redisDb.HashGetAll(redisKey).ToList();
                return temp.Count != 0
                    ? temp.Select(hashEntry => new KeyValuePair<string, string>(hashEntry.Name, hashEntry.Value)).ToList()
                    : null;
            }
            catch
            {
                return null;
            }

        }

        public TimeSpan GetRedisExpiry(string key)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "mandiriTransactionPrice:" + key;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
                return timeToLive;
            }
            catch
            {
                return TimeSpan.Zero;
            }
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

        private void SaveSearchedItinerariesToCache(List<List<FlightItinerary>> itinLists, string searchId, int timeout, int supplierIndex)
        {
            if (timeout == 0)
                timeout =
                    Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisTransaction = redisDb.CreateTransaction();
            for (var i = 0; i < itinLists.Count; i++)
            {
                var itinList = itinLists[i];
                var redisKey = "searchedFlightItineraries:" + i + ":" + searchId + ":" + supplierIndex;
                var cacheObject = itinList.ToCacheObject();
                redisTransaction.StringSetAsync(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
            }
            redisTransaction.Execute();
        }

        private static void SaveSearchedPartialItinerariesToBufferCache(List<FlightItinerary> itineraryList, string searchId, int timeout, int supplierIndex, int partNumber)
        {
            if (timeout == 0)
                timeout =
                    Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            var redisService = RedisService.GetInstance();
            var redisKey = "searchedPartialFlightItineraries:" + searchId + ":" + supplierIndex + ":" + partNumber;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = itineraryList.ToCacheObject();
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
        }

        private static void SaveSearchedSupplierIndexToCache(string searchId, int supplierIndex, int timeout)
        {
            if (timeout == 0)
                timeout =
                    Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            var redisService = RedisService.GetInstance();
            var redisKey = "searchedSupplierIndices:" + searchId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.ListRightPush(redisKey, supplierIndex);
            redisDb.KeyExpire(redisKey, TimeSpan.FromMinutes(timeout));
        }

        private static List<int> GetSearchedSupplierIndicesFromCache(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "searchedSupplierIndices:" + searchId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var length = redisDb.ListLength(redisKey);
            return redisDb.ListRange(redisKey, 0, length - 1).Select(val => (int)val).Distinct().ToList();
        }

        private List<List<FlightItinerary>> GetSearchedPartialItinerariesFromBufferCache(string searchId, int supplierIndex)
        {
            var partialItinsCount = DecodeSearchConditions(searchId).Trips.Count;
            var redisService = RedisService.GetInstance();
            var itineraryLists = new List<List<FlightItinerary>>();
            for (var i = 0; i <= partialItinsCount; i++)
            {
                var redisKey = "searchedPartialFlightItineraries:" + searchId + ":" + supplierIndex + ":" + i;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);
                var itins = cacheObject.DeconvertTo<List<FlightItinerary>>();
                itineraryLists.Add(itins);
                if (partialItinsCount == 1)
                    break;
            }
            return itineraryLists;
        }

        private static FlightItinerary GetItineraryFromSearchCache(string searchId, int registerNumber, int partNumber = 1)
        {
            var redisService = RedisService.GetInstance();
            var supplierIndex = registerNumber / SupplierIndexCap;
            var redisKey = "searchedFlightItineraries:" + partNumber + ":" + searchId + ":" + supplierIndex;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = redisDb.StringGet(redisKey);
            var itins = cacheObject.DeconvertTo<List<FlightItinerary>>();
            if (itins == null)
                return null;
            return itins.SingleOrDefault(itin => itin.RegisterNumber == registerNumber);
        }

        public DateTime? GetSearchedItinerariesExpiry(string searchId, int supplierIndex)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchedFlightItineraries:0:" + searchId + ":" + supplierIndex;
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

        private Dictionary<int, List<List<FlightItinerary>>> GetSearchedSupplierItineraries(string searchId, List<int> requestedSupplierIds)
        {
            var conditions = DecodeSearchConditions(searchId);
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var searchedSupplierItins = new Dictionary<int, List<List<FlightItinerary>>>();
            foreach (var supplierId in requestedSupplierIds)
            {
                var isSearched = true;
                var cacheObjects = new List<RedisValue>();
                for (var i = 0; i <= conditions.Trips.Count; i++)
                {
                    var redisKey = "searchedFlightItineraries:" + i + ":" + searchId + ":" + supplierId;
                    var cacheObject = redisDb.StringGet(redisKey);
                    if (cacheObject.IsNull)
                    {
                        isSearched = false;
                        break;
                    }
                    cacheObjects.Add(cacheObject);
                    if (conditions.Trips.Count == 1)
                        break;
                }
                if (isSearched)
                {
                    var itinsList = cacheObjects.Select(obj => obj.DeconvertTo<List<FlightItinerary>>()).ToList();
                    searchedSupplierItins.Add(supplierId, itinsList);
                }
            }
            return searchedSupplierItins;
        }

        private string SaveItineraryFromSearchToCache(string searchId, int registerNumber, int partNumber)
        {
            var itinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itin = GetItineraryFromSearchCache(searchId, registerNumber, partNumber);

            if (itin == null)
                return null;

            var redisService = RedisService.GetInstance();
            var redisKey = "flightItinerary:" + itinCacheId;
            var cacheObject = itin.ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
            return itinCacheId;
        }

        private void SaveItineraryToCache(FlightItinerary itin, string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightItinerary:" + itinCacheId;
            var cacheObject = itin.ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
        }

        private void SaveCombosToCache(List<Combo> combos, string searchId, int supplierIndex)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightCombos:" + searchId + ":" + supplierIndex;
            var cacheObject = combos.ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
        }

        private List<Combo> GetCombosFromCache(string searchId, int supplierIndex)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightCombos:" + searchId + ":" + supplierIndex;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = redisDb.StringGet(redisKey);
            return cacheObject.DeconvertTo<List<Combo>>() ?? new List<Combo>();
        }

        private FlightItinerary GetItineraryFromCache(string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItinerary:" + itinCacheId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);

                if (cacheObject.IsNullOrEmpty)
                    return null;

                var itinerary = cacheObject.DeconvertTo<FlightItinerary>();
                return itinerary;
            }
            catch
            {
                return null;
            }
        }

        private void DeleteItineraryFromCache(string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightItinerary:" + itinCacheId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            //var cacheObject = redisDb.StringGet(redisKey);
            redisDb.KeyDelete(redisKey);
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

        private string SaveItinerariesToCache(List<FlightItinerary> itins)
        {
            var itinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            SaveItinerariesToCache(itins, itinCacheId);
            return itinCacheId;
        }

        private void SaveItinerariesToCache(List<FlightItinerary> itins, string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightItineraries:" + itinCacheId;
            var setCacheObject = itins.ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, setCacheObject, TimeSpan.FromMinutes(timeout));
        }

        private List<FlightItinerary> GetItinerariesFromCache(string itinCacheId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "flightItineraries:" + itinCacheId;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var cacheObject = redisDb.StringGet(redisKey);

                if (cacheObject.IsNullOrEmpty)
                    return null;

                var itinerary = cacheObject.DeconvertTo<List<FlightItinerary>>();
                return itinerary;
            }
            catch
            {
                return null;
            }
        }

        private void DeleteItinerariesFromCache(string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightItineraries:" + itinCacheId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.KeyDelete(redisKey);
        }

        private void SavePaymentRedirectionUrlInCache(string rsvNo, string paymentUrl, DateTime? timeLimit)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "paymentRedirectionUrl:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, paymentUrl, timeLimit - DateTime.UtcNow);
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

        private void SaveActiveMarginRulesToCache(List<FlightMarginRule> marginRules)
        {
            var redisService = RedisService.GetInstance();
            var redisMarginsKey = "activeFlightMargins";
            var redisRulesKey = "activeFlightMarginRules";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var marginsCacheObject = marginRules.Select(mr => mr.Margin).ToCacheObject();
            var rulesCacheObject = marginRules.Select(mr => mr.Rule).ToCacheObject();
            redisDb.StringSet(redisMarginsKey, marginsCacheObject);
            redisDb.StringSet(redisRulesKey, rulesCacheObject);
        }

        private List<FlightMarginRule> GetAllActiveMarginRulesFromCache()
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisMarginKey = "activeFlightMargins";
                var redisRuleKey = "activeFlightMarginRules";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var marginCacheObject = redisDb.StringGet(redisMarginKey);
                var margins = marginCacheObject.DeconvertTo<List<Margin>>();
                var ruleCacheObject = redisDb.StringGet(redisRuleKey);
                var rules = ruleCacheObject.DeconvertTo<List<FlightItineraryRule>>();
                var marginRules = margins.Zip(rules, (margin, rule) => new FlightMarginRule(margin, rule)).ToList();
                return marginRules;
            }
            catch
            {
                return new List<FlightMarginRule>();
            }
        }

        private void SaveActiveMarginRulesInBufferCache(List<FlightMarginRule> marginRules)
        {
            var redisService = RedisService.GetInstance();
            var marginsRedisKey = "activeFlightMarginsBuffer";
            var rulesRedisKey = "activeFlightMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var marginsCacheObject = marginRules.Select(mr => mr.Margin).ToCacheObject();
            var rulesCacheObject = marginRules.Select(mr => mr.Rule).ToCacheObject();
            redisDb.StringSet(marginsRedisKey, marginsCacheObject);
            redisDb.StringSet(rulesRedisKey, rulesCacheObject);
        }

        private void SaveDeletedMarginRulesInBufferCache(List<FlightMarginRule> deletedMarginRules)
        {
            var redisService = RedisService.GetInstance();
            var deletedMarginsRedisKey = "deletedFlightMarginsBuffer";
            var deletedRulesRedisKey = "deletedFlightMarginRulesBuffer";
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var deletedMarginsCacheObject = deletedMarginRules.Select(mr => mr.Margin).ToCacheObject();
            var deletedRulesCacheObject = deletedMarginRules.Select(mr => mr.Rule).ToCacheObject();
            redisDb.StringSet(deletedMarginsRedisKey, deletedMarginsCacheObject);
            redisDb.StringSet(deletedRulesRedisKey, deletedRulesCacheObject);
        }

        private List<FlightMarginRule> GetActiveMarginRulesFromBufferCache()
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisMarginsKey = "activeFlightMarginsBuffer";
                var redisRulesKey = "activeFlightMarginRulesBuffer";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var marginsCacheObject = redisDb.StringGet(redisMarginsKey);
                var rulesCacheObject = redisDb.StringGet(redisRulesKey);
                var margins = marginsCacheObject.DeconvertTo<List<Margin>>();
                var rules = rulesCacheObject.DeconvertTo<List<FlightItineraryRule>>();
                var marginRules = margins.Zip(rules, (margin, rule) => new FlightMarginRule(margin, rule)).ToList();
                return marginRules;
            }
            catch
            {
                return new List<FlightMarginRule>();
            }
        }

        private List<FlightMarginRule> GetDeletedMarginRulesFromBufferCache()
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisMarginsKey = "deletedFlightMarginsBuffer";
                var redisRulesKey = "deletedFlightMarginRulesBuffer";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var marginCacheObject = redisDb.StringGet(redisMarginsKey);
                var rulesCacheObject = redisDb.StringGet(redisRulesKey);
                var margins = marginCacheObject.DeconvertTo<List<Margin>>();
                var rules = rulesCacheObject.DeconvertTo<List<FlightItineraryRule>>();
                var marginRules = margins.Zip(rules, (margin, rule) => new FlightMarginRule(margin, rule)).ToList();
                return marginRules;
            }
            catch
            {
                return new List<FlightMarginRule>();
            }
        }

        public void SetFlightRequestTripType(string requestId, bool asReturn)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestAsReturn:" + requestId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeout = Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            redisDb.StringSet(redisKey, asReturn, new TimeSpan(0, 2 * timeout, 0));
        }

        public bool? GetFlightRequestTripType(string requestId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestAsReturn:" + requestId;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            return (bool?)redisDb.StringGet(redisKey);
        }

        public void SaveFlightRequestPrices(string searchId, List<FlightItinerary> itins, int itinSetNo = 0)
        {
            var cacheContent = GetFlightRequestPrices(searchId).ToDictionary(e => e.Key, e => e.Value);
            itins.ForEach(itin => cacheContent.Add(itin.RegisterNumber, itin.Price.Local));

            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestPrices:" + searchId + ":" + itinSetNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeout = Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            redisDb.StringSet(redisKey, cacheContent.ToCacheObject(), new TimeSpan(0, 2 * timeout, 0));
        }

        public Dictionary<int, decimal> GetFlightRequestPrices(string searchId, int itinSetNo = 0)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "flightRequestPrices:" + searchId + ":" + itinSetNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cache = redisDb.StringGet(redisKey);
            return cache.IsNull ? new Dictionary<int, decimal>() : cache.DeconvertTo<Dictionary<int, decimal>>();
        }
    }
}
