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
                try
                {
                    var value = redisDb.StringGet(redisKey);
                    return value;
                }
                catch
                {

                }
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
                try
                {
                    redisDb.StringSet(redisKey, rsvNo, TimeSpan.FromMinutes(150));
                    return;
                }
                catch
                {

                }
            }
        }

        private static void SaveTransferFeeinCache(string rsvNo, decimal transferFee)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    redisDb.StringSet(redisKey, Convert.ToString(transferFee), TimeSpan.FromMinutes(150));
                    return;
                }
                catch
                {

                }
            }
        }

        private static decimal GetTransferFeeFromCache(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var transferFee = new RedisValue();
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    transferFee = redisDb.StringGet(redisKey);
                    break;
                }
                catch
                {

                }
            
            }
            
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
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    redisDb.KeyDelete(redisKey);
                    return;
                }
                catch
                {

                }

            }
        }
    }
}
