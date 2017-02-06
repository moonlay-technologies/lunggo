using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Campaign.Model;
using System;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Http;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        private bool IsPanAndEmailEligibleInCache(string promoType, string hashedPan, string email, int limit)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var panRedisKey = "binPromo:pan:" + promoType;
            var isPanExist = redis.SetContains(panRedisKey, hashedPan);
            var emailRedisKey = "binPromo:email:" + promoType;
            var isEmailExist = redis.SetContains(emailRedisKey, email);
            var fullyUsed = redis.SetLength(panRedisKey) >= limit || redis.SetLength(emailRedisKey) >= limit;
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
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

        private bool IsPhoneAndEmailEligibleInCache(string voucherCode, string phone, string email)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var phoneRedisKey = "voucherPromo:phone:" + voucherCode;
            var isPhoneExist = redis.SetContains(phoneRedisKey, phone);
            var emailRedisKey = "voucherPromo:email:" + voucherCode;
            var isEmailExist = redis.SetContains(emailRedisKey, email);
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
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
