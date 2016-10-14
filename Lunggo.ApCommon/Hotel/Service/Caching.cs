using System;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveSelectedHotelDetailsToCache(string token, HotelDetailsBase hotel)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "token:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var timeNow = DateTime.UtcNow;
            var expiry = timeNow.AddHours(1);
            var redisValue = "hoteldetails:" + hotel.Serialize(); 
            redisDb.StringSet(redisKey, redisValue, expiry - timeNow);
        }


        public void SaveSearchResultintoDatabaseToCache(string token, SearchHotelResult searchResult)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "token:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisValue = "searchresult:" + searchResult.Serialize();
            redisDb.StringSet(redisKey, redisValue, TimeSpan.FromMinutes(60));
        }

        public SearchHotelResult GetSearchHotelResultFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "token:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = (string)redisDb.StringGet(redisKey);
            var searchResult = cacheObject.Substring(13).Deserialize<SearchHotelResult>();
            return searchResult;
        }

        public HotelDetailsBase GetSelectedHotelDetailsFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "token:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var cacheObject = (string) redisDb.StringGet(redisKey);
            var hotelDetails = cacheObject.Substring(13).Deserialize<HotelDetailsBase>();
            return hotelDetails;
        }

        public DateTime? GetSearchedHotelDetailsExpiry(string searchId)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "searchedFlightItineraries:0:" + searchId + ":";
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeToLive = redisDb.KeyTimeToLive(redisKey);
                var expiryTime = DateTime.UtcNow + timeToLive;
                return expiryTime;
            }
            catch
            {
                return null;
            }
        }
    }
}
