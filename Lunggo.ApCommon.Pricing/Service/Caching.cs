using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Pricing.Service
{
    public partial class PricingService
    {
        private static void SetCurrencyRateInCache(string currency, decimal rate)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var redisKey = currency + "Rate";
            redisDb.StringSet(redisKey, (RedisValue)rate);
        }

        private static decimal GetCurrencyRateInCache(string currency)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            var redisKey = currency + "Rate";
            var rate = (decimal?) redisDb.StringGet(redisKey) ?? 0M;
            return rate;
        }
    }
}
