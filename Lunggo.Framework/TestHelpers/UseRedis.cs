using System;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.Framework.TestHelpers
{
    public static partial class TestHelper
    {
        public static void UseRedis(Action<IDatabase> callback)
        {
            InitRedis(out var connName);
            var redis = RedisService.GetInstance().GetDatabase(connName);
            callback(redis);
        }

        private static void InitRedis(out string connName)
        {
            connName = "master_data_cache";
            var connString =
                "travorama-development.redis.cache.windows.net,allowAdmin=true,syncTimeout=60000,ssl=true,password=16EGFGYzLMtwUP1KiNjgsi2rcgBPYnlSYWRqOK0EX5c=";
            var redis = RedisService.GetInstance();
            redis.Init(new[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = connName,
                    ConnectionString = connString
                }
            });
        }
    }
}