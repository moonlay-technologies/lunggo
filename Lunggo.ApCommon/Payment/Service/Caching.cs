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

        private static void SaveTransferFeeinCache(decimal finalPrice, string rsvNo, decimal transferFee)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + finalPrice;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.HashSet(redisKey, rsvNo, (RedisValue)transferFee);
            redisDb.KeyExpire(redisKey, TimeSpan.FromMinutes(150));

        }

        private static decimal GetTransferFeeFromCache(decimal finalPrice, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + finalPrice;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var transferFee = redisDb.HashGet(redisKey, rsvNo);
            return (decimal) transferFee;
        }

        // Penambahan Buat Delete TransferCode jika tidak digunakan
        private static void DeleteTransferFeeFromCache(decimal finalPrice)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + finalPrice;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.KeyDelete(redisKey);
        }
    }
}
