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

        public void SaveItineraryToCache(FlightItinerary itin, string hash)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + hash;
            var cacheObject = itin.ToCacheObject();
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"))));
        }

        public string SaveItineraryToCache(string searchId, int itinIndex)
        {
            var plain = new StringBuilder();
            plain.Append(searchId);
            plain.Append(itinIndex.ToString("000"));
            var hash = plain.ToString().Hash();

            var itins = GetItinerariesFromCache(searchId);
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + hash;
            var cacheObject = itins[itinIndex].ToCacheObject();
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "ItineraryCacheTimeout"))));

            return hash;
        }

        public FlightItinerary GetItineraryFromCache(string hash)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItinerary:" + hash;
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

        public void SaveItinerariesToCache(string searchId, List<FlightItinerary> itineraryList)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItineraries:" + searchId;
            var cacheObject = itineraryList.ToCacheObject();
            redisDb.StringSet(redisKey, cacheObject, TimeSpan.FromMinutes(
                Int32.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"))));
        }

        public List<FlightItinerary> GetItinerariesFromCache(string searchId)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "flightItineraries:" + searchId;
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
    }
}
