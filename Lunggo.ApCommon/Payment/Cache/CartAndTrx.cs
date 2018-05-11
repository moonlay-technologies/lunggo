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

        public string GetUserCartId(string userId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:UserCartId:" + userId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var cartId = redisDb.StringGet(redisKey);
            return cartId;
        }

        public string GetCartUserId(string cartId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:CartUserId:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var userId = redisDb.StringGet(redisKey);
            return userId;
        }

        public void SaveUserCartId(string userId, string cartId)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:UserCartId:" + userId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.StringSet(redisKey, cartId);
            var redisCartKey = "Cart:CartUserId:" + cartId;
            redisDb.StringSet(redisCartKey, userId);
        }

        public void ClearUserCartId(string cartId)
        {
            var userId = GetCartUserId(cartId);
            var redisService = RedisService.GetInstance();
            var redisKey = "Cart:UserCartId:" + userId;
            var redisCartKey = "Cart:CartUserId:" + cartId;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            redisDb.KeyDelete(redisKey);
            redisDb.KeyDelete(redisCartKey);
        }
    }
}