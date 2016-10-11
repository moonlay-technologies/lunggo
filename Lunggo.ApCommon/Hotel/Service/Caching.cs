using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

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
