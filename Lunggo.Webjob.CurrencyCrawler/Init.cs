using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;

namespace Lunggo.Webjob.CurrencyCrawler
{
    public partial class Program
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitRedisService();
            InitDatabaseService();
            InitQueueService();
            InitPayment();
            InitFlightService();
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("");
        }
        private static void InitPayment()
        {
            PaymentService.GetInstance().Init();
        }
        private static void InitRedisService()
        {
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.SearchResultCacheName,
                    ConnectionString = ConfigManager.GetInstance().GetConfigValue("redis", "searchResultCacheConnectionString")
                },
                
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = ConfigManager.GetInstance().GetConfigValue("redis", "masterDataCacheConnectionString")
                }, 
                 
            });
        }

        private static void InitDatabaseService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("db", "connectionString");
            var database = DbService.GetInstance();
            database.Init(connString);
        }

        private static void InitQueueService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"");
        }
    }
}
