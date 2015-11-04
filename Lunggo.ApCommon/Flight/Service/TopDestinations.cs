﻿using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.Framework.Config;
using Lunggo.Framework.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public TopDestinations GetTopDestination()
        {
            return GetTopDestinationsFromCache();
        }
        private static TopDestinations GetTopDestinationsFromCache()
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var cacheKey = ConfigManager.GetInstance().GetConfigValue("flight","topdestinationcachekey");
            var rawTopDestinationsListFromCache = redisDb.StringGet(cacheKey);
            var topDestinations = FlightCacheUtil.ConvertTopDestinationRawObjectToTopDestination(rawTopDestinationsListFromCache);
            return topDestinations;
        }
    }
}
