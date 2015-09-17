using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.WebJob.FlightCrawlScheduler
{
    partial class Scheduler
    {
        private static void Init()
        {
            InitConfigurationManager();
            InitDatabaseService();
            InitUniqueIdGenerator();
            InitRedisService();
            InitFlightService();
            InitQueueService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"Config\");
        }

        private static void InitDatabaseService()
        {
            var db = DbService.GetInstance();
            db.Init();
        }

        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = ConfigManager.GetInstance().GetConfigValue("general", "seqGeneratorContainerName");
            var storageConnectionString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var optimisticData = new BlobOptimisticDataStore(CloudStorageAccount.Parse(storageConnectionString), seqContainerName)
            {
                SeedValueInitializer = (sequenceName) => generator.GetIdInitialValue(sequenceName)
            };
            generator.Init(optimisticData);
            generator.BatchSize = 100;
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

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

        private static void InitQueueService()
        {
            var queue = QueueService.GetInstance();
            queue.Init();
        }
    }
}
