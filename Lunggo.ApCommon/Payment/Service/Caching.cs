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
        private static string GetRsvNoHavingTransferValue(decimal price)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var value = redisDb.StringGet(redisKey);
            return value;
        }

        private static void SaveTransferValue(decimal price, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, rsvNo, TimeSpan.FromMinutes(150));
        }

        private static void SaveTransferFeeinCache(string rsvNo, decimal transferFee)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.StringSet(redisKey, Convert.ToString(transferFee), TimeSpan.FromMinutes(150));
        }

        private static decimal GetTransferFeeFromCache(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var transferFee = redisDb.StringGet(redisKey);
            if (transferFee.IsNullOrEmpty)
                return 0M;
            return Convert.ToDecimal(transferFee);
        }

        // Penambahan Buat Delete TransferCode jika tidak digunakan
        private static void DeleteTransferFeeFromCache(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            redisDb.KeyDelete(redisKey);
        }
    }
}
