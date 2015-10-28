using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BrowserDetection;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;

namespace Lunggo.WebJob.FlightCrawler
{
    partial class Program
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitRedisService();
            InitQueueService();
            InitFlightService();
            InitDictionaryService();
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

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"");
        }

        private static void InitQueueService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

        private static void InitDictionaryService()
        {
            var dict = DictionaryService.GetInstance();
            dict.Init("");
        }
    }
}
