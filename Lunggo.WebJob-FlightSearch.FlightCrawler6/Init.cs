using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Log;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;

namespace Lunggo.WebJob.FlightCrawler
{
    partial class Program
    {
        public static void Init()
        {
            InitDatabaseService();
            InitRedisService();
            InitQueueService();
            InitFlightService();
            InitLogService();
        }

        private static void InitRedisService()
        {
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "searchResultCacheConnectionString")
                },
                
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "masterDataCacheConnectionString")
                }, 
                 
            });
        }

        private static void InitQueueService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("");
        }

        private static void InitDatabaseService()
        {
            var connString = EnvVariables.Get("db", "connectionString");
            var database = DbService.GetInstance();
            database.Init(connString);
        }
        public static void InitLogService()
        {
            var webhookUrl = EnvVariables.Get("log", "slack");
            var log = LogService.GetInstance();
            log.Init(webhookUrl);
        }
    }
}
