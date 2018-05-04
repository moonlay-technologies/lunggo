using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Database;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;

namespace Lunggo.Webjob.CurrencyCrawler
{
    public partial class Program
    {
        public static void Init()
        {
            InitRedisService();
            InitDatabaseService();
            InitQueueService();
            InitFlightService();
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("");
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

        private static void InitDatabaseService()
        {
            var connString = EnvVariables.Get("db", "connectionString");
            var database = DbService.GetInstance();
            database.Init(connString);
        }

        private static void InitQueueService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }
    }
}
