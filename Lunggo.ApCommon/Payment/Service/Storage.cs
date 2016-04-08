using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        private static bool IsTransferValueExist(string price)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "transferUniquePrice:" + price;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var value = redisDb.HashGetAll(redisKey).ToList();
                return value.Count != 0;
            }
            catch
            {
                return false;
            }
        }

        private static void SaveTokenTransferCodeinCache(string token, string transferCode)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferCodeToken:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, transferCode, TimeSpan.FromMinutes(150));
        }

        private static decimal GetTransferCodeByTokeninCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferCodeToken:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var price = Decimal.Parse(redisDb.StringGet(redisKey));
            return price;
        }

        //Penambahan Method ini buat menghapus token Transfer Code jika tidak dipakai
        private static void DeleteTokenTransferCodeFromCache(string token)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferCodeToken:" + token;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.KeyDelete(redisKey);
        }

        private static void SaveUniquePriceinCache(string price, Dictionary<string, int> dict)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            foreach (var pair in dict)
            {
                redisDb.HashSet(redisKey, pair.Key, pair.Value);
                redisDb.KeyExpire(redisKey, TimeSpan.FromMinutes(150));
            }

        }

        private static Dictionary<string, int> GetUniquePriceFromCache(string price)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "transferUniquePrice:" + price;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var temp = redisDb.HashGetAll(redisKey).ToList();
                if (temp.Count != 0)
                {
                    return temp.ToDictionary(hashEntry => (string)hashEntry.Name, hashEntry => (int)hashEntry.Value);
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

        // Penambahan Buat Delete TransferCode jika tidak digunakan
        private static void DeleteUniquePriceFromCache(string price)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.KeyDelete(redisKey);
        }

        private static TimeSpan GetUniqueIdExpiry(string key)
        {
            try
            {
                var redisService = RedisService.GetInstance();
                var redisKey = "transferUniquePrice:" + key;
                var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
                var timeToLive = redisDb.KeyTimeToLive(redisKey).GetValueOrDefault();
                return timeToLive;
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
    }
}
