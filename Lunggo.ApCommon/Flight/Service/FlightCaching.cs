using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string SaveItineraryToCache(string searchId, int itinIndex)
        {
            var plain = new StringBuilder();
            plain.Append(searchId);
            plain.Append(itinIndex.ToString("000"));
            var hash = FlightHashUtil.Hash(plain.ToString());

            var itins = GetSearchResultFromCache(searchId);
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = FlightCacheUtil.ConvertToCacheObject(itins[itinIndex]);
            redisDb.StringSet(hash, cacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "flightItineraryCacheTimeout"))));

            return hash;
        }

        public FlightItineraryFare GetItineraryFromCache(string hash)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = redisDb.StringGet(hash);

            if (!cacheObject.IsNullOrEmpty)
            {
                var itinerary = FlightCacheUtil.DeconvertFromCacheObject<FlightItineraryFare>(cacheObject);
                return itinerary;
            }
            else
            {
                return null;
            }
        }

        private static void SaveSearchResultToCache(string searchId, List<FlightItineraryFare> itineraryList)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = FlightCacheUtil.ConvertToCacheObject(itineraryList);
            redisDb.StringSet(searchId, cacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "flightSearchResultCacheTimeout"))));
        }

        private static List<FlightItineraryFare> GetSearchResultFromCache(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = redisDb.StringGet(searchId);

            if (!cacheObject.IsNullOrEmpty)
            {
                var itineraryList = FlightCacheUtil.DeconvertFromCacheObject<List<FlightItineraryFare>>(cacheObject);
                return itineraryList;
            }
            else
            {
                return null;
            }

        }
    }
}
