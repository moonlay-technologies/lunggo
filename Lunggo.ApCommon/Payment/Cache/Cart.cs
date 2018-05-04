using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Payment.Cache
{
    internal partial class PaymentCacheService
    {
        internal virtual List<string> GetCartRsvNos(string cartId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:RsvNoList:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            if (!redisDb.KeyExists(redisKey) || redisDb.ListLength(redisKey) == 0)
                return new List<string>();

            var rsvNoList = redisDb.ListRange(redisKey).Select(val => val.ToString()).Distinct().ToList();
            return rsvNoList;
        }

        internal virtual void AddRsvToCart(string cartId, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:RsvNoList:" + cartId;
            var redisValue = rsvNo;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.ListRightPush(redisKey, redisValue);
        }

        internal virtual void RemoveRsvFromCart(string cartId, string rsvNo)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:RsvNoList:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.ListRemove(redisKey, rsvNo);
        }

        internal virtual void DeleteCart(string cartId)
        {
            var redisService = RedisService.GetInstance();
            var redisKeyCartId = "Cart:RsvNoList:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.KeyDelete(redisKeyCartId);
        }
    }
}