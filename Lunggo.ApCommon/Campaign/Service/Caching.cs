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
using Lunggo.Framework.Http;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        private bool CheckPanInCache(string hashedPan)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var redisKey = "binPromo:btn:" + hashedPan;
            var isExist = !redis.StringGet(redisKey).IsNullOrEmpty;
            return isExist;
        }

        public void SavePanInCache(string hashedPan)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.MasterDataCacheName);
            var redisKey = "binPromo:btn:" + hashedPan;
            var timeout = DateTime.UtcNow.AddHours(31).Date - DateTime.UtcNow.AddHours(7);
            redis.StringSet(redisKey, "true", timeout);
        }
    }
}
