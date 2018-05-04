using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Payment.Cache
{
    internal partial class PaymentCacheService
    {
        private bool IsPanAndEmailEligibleInCache(string promoType, string hashedPan, string email, int limit)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var panRedisKey = "binPromo:pan:" + promoType;
            var isPanExist = redis.SetContains(panRedisKey, hashedPan);
            var emailRedisKey = "binPromo:email:" + promoType;
            var isEmailExist = redis.SetContains(emailRedisKey, email);
            var fullyUsed = redis.SetLength(panRedisKey) >= limit || redis.SetLength(emailRedisKey) >= limit;
            var env = EnvVariables.Get("general", "environment");
            return env != "production" || (!isPanExist && !isEmailExist && !fullyUsed);
        }

        public void SavePanAndEmailInCache(string promoType, string hashedPan, string email)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var panRedisKey = "binPromo:pan:" + promoType;
            redis.SetAdd(panRedisKey, hashedPan);
            var emailRedisKey = "binPromo:email:" + promoType;
            redis.SetAdd(emailRedisKey, email);
            var timeout = DateTime.UtcNow.AddHours(31).Date - DateTime.UtcNow.AddHours(7);
            redis.KeyExpire(panRedisKey, timeout);
            redis.KeyExpire(emailRedisKey, timeout);
        }

        public void SaveEmailInCache(string promoType, string email)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var emailRedisKey = "methodPromo:email:" + promoType;
            redis.SetAdd(emailRedisKey, email);
            var timeout = DateTime.UtcNow.AddHours(31).Date - DateTime.UtcNow.AddHours(7);
            redis.KeyExpire(emailRedisKey, timeout);
        }

        private bool IsEmailEligibleInCache(string promoType, string email, int limit)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var emailRedisKey = "methodPromo:email:" + promoType;
            var isEmailExist = redis.SetContains(emailRedisKey, email);
            var fullyUsed = redis.SetLength(emailRedisKey) > limit;
            var env = EnvVariables.Get("general", "environment");
            return  (!isEmailExist && !fullyUsed);
            //env != "production" ||
        }
        internal bool IsPhoneAndEmailEligibleInCache(string voucherCode, string phone, string email)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var phoneRedisKey = "voucherPromo:phone:" + voucherCode;
            var isPhoneExist = redis.SetContains(phoneRedisKey, phone);
            var emailRedisKey = "voucherPromo:email:" + voucherCode;
            var isEmailExist = redis.SetContains(emailRedisKey, email);
            var env = EnvVariables.Get("general", "environment");
            env = "production";
            return env != "production" || (!isPhoneExist && !isEmailExist);
        }

        public void SavePhoneAndEmailInCache(string voucherCode, string phone, string email)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var phoneRedisKey = "voucherPromo:phone:" + voucherCode;
            redis.SetAdd(phoneRedisKey, phone);
            var emailRedisKey = "voucherPromo:email:" + voucherCode;
            redis.SetAdd(emailRedisKey, email);
            var timeout = DateTime.UtcNow.AddHours(31).Date - DateTime.UtcNow.AddHours(7);
            redis.KeyExpire(phoneRedisKey, timeout);
            redis.KeyExpire(emailRedisKey, timeout);
        }
    }
}