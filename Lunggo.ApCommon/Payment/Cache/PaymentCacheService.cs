using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Payment.Cache
{
    internal partial class PaymentCacheService
    {
        private const int MaxLoop = 3;

        internal string GetRsvNoHavingTransferValue(decimal price)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            for (var i = 0; i < 3; i++)
            {
                var value = redisDb.StringGet(redisKey);
                return value;
            }

            return null;
        }

        internal void SaveTransferValue(decimal price, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferUniquePrice:" + price;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            for (var i = 0; i < 3; i++)
            {
                redisDb.StringSet(redisKey, rsvNo, TimeSpan.FromMinutes(150));
                return;
            }
        }

        internal void SaveUniqueCode(string rsvNo, decimal uniqueCode)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "uniqueCode:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            for (var i = 0; i < 3; i++)
            {
                redisDb.StringSet(redisKey, Convert.ToString(uniqueCode), TimeSpan.FromMinutes(150));
                return;
            }
        }

        internal decimal GetUniqueCode(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "uniqueCode:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
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

        internal void DeleteTransferFee(string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "transferFee:" + rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            for (var i = 0; i < 3; i++)
            {
                redisDb.KeyDelete(redisKey);
                return;
            }
        }
    }
}