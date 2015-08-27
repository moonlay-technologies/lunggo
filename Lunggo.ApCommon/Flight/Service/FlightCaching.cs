using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private const string SingleItinKeyPrefix = "9284";
        private const string ItinBundleKeyPrefix = "3462";

        internal string SaveSearchedItinerariesToCache(List<FlightItinerary> itineraryList)
        {
            var searchId = FlightSearchIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "searchedFlightItineraries:" + searchId;
            var cacheObject = itineraryList.ToCacheObject();
            var timeout = Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
            return searchId;
        }

        internal List<FlightItinerary> GetSearchedItinerariesFromCache(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "searchedFlightItineraries:" + searchId;
            var cacheObject = redisDb.StringGet(redisKey);

            if (!cacheObject.IsNullOrEmpty)
            {
                var itineraryList = cacheObject.DeconvertTo<List<FlightItinerary>>();
                return itineraryList;
            }
            else
            {
                return null;
            }
        }

        public DateTime GetSearchedItinerariesExpiry(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "SearchedFlightItineraries:" + searchId;
            var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
            var expiryTime = DateTime.UtcNow + timeToLive;
            return expiryTime;
        }

        internal string SaveItineraryFromSearchToCache(string searchId, int itinIndex)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = SingleItinKeyPrefix + plainItinCacheId;
            var itins = GetSearchedItinerariesFromCache(searchId);
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + itinCacheId;
            var cacheObject = itins[itinIndex].ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
            return itinCacheId;
        }
        internal FlightItinerary GetItineraryFromSearchCache(string searchId, int itinIndex)
        {
            var itins = GetSearchedItinerariesFromCache(searchId);
            return itins[itinIndex];
        }

        internal string SaveItineraryToCache(FlightItinerary itin)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = SingleItinKeyPrefix + plainItinCacheId;
            SaveItineraryToCache(itin, itinCacheId);
            return itinCacheId;
        }

        internal void SaveItineraryToCache(FlightItinerary itin, string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + itinCacheId;
            var cacheObject = itin.ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(timeout));
        }

        internal FlightItinerary GetItineraryFromCache(string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + itinCacheId;
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

        public DateTime GetItineraryExpiry(string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + itinCacheId;
            var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
            var expiryTime = DateTime.UtcNow + timeToLive;
            return expiryTime;
        }

        internal string SaveItinerarySetAndBundleToCache(List<FlightItinerary> itinSet, FlightItinerary itinBundle)
        {
            var plainItinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itinCacheId = ItinBundleKeyPrefix + plainItinCacheId;
            SaveItinerarySetAndBundleToCache(itinSet, itinBundle, itinCacheId);
            return itinCacheId;
        }

        internal void SaveItinerarySetAndBundleToCache(List<FlightItinerary> itinSet, FlightItinerary itinBundle, string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisSetKey = "flightItinerarySet:" + itinCacheId;
            var setCacheObject = itinSet.ToCacheObject();
            var redisBundleKey = "flightItinerary:" + itinCacheId;
            var bundleCacheObject = itinBundle.ToCacheObject();
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"));
            redisDb.StringSet(redisSetKey, setCacheObject, TimeSpan.FromMinutes(timeout));
            redisDb.StringSet(redisBundleKey, bundleCacheObject, TimeSpan.FromMinutes(timeout));
        }

        internal List<FlightItinerary> GetItinerarySetFromCache(string itinCacheId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerarySet:" + itinCacheId;
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
    }
}
