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
        private const int MaxLoop = 3;

        private static string GetRsvNoHavingTransferValue(decimal price)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < 3; i++)
            {
                    var value = redisDb.StringGet(redisKey);
                    return value;
            }

            return null;
        }

        private static void SaveTransferValue(decimal price, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < 3; i++)
            {
                    redisDb.StringSet(redisKey, rsvNo, TimeSpan.FromMinutes(150));
                    return;
            }
        }

        private static void SaveUniqueCodeinCache(string rsvNo, decimal uniqueCode)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "uniqueCode:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < 3; i++)
            {
                    redisDb.StringSet(redisKey, Convert.ToString(uniqueCode), TimeSpan.FromMinutes(150));
                    return;
            }
        }

        private static decimal GetUniqueCodeFromCache(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "uniqueCode:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var uniqueCode = new RedisValue();
            for (var i = 0; i < 3; i++)
            {
                    uniqueCode = redisDb.StringGet(redisKey);
                    break;
            
            }
            
            if (uniqueCode.IsNullOrEmpty)
                return 0M;
            return Convert.ToDecimal(uniqueCode);
        }

        // Penambahan Buat Delete TransferCode jika tidak digunakan
        private static void DeleteTransferFeeFromCache(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < 3; i++)
            {
                    redisDb.KeyDelete(redisKey);
                    return;
            }
        }
    }
}
