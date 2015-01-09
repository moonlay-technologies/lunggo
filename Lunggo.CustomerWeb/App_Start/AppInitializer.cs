﻿using System.Web;
using log4net;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Queue;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;
using Lunggo.Framework.Database;


namespace Lunggo.CustomerWeb
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitI18NMessageManager();
            InitUniqueIdGenerator();
            InitRedisService();
            //InitDatabaseService();
            //InitQueueService();
            //InitLogger();
        }

        private static void InitRedisService()
        {
            //TODO Use Configuration File Do Not Hardcode
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = "search_result_cache",
                    ConnectionString = "lunggosearchdev.redis.cache.windows.net,allowAdmin=true,syncTimeout=5000,ssl=true,password=Wl4iQpbjuvs+Yr5OnNzOYo3AhY/1+1K5Gunpu7IvoR4="
                },
                new RedisConnectionProperty
                {
                    ConnectionName = "master_data_cache",
                    ConnectionString = "lunggodatadev.redis.cache.windows.net,allowAdmin=true,syncTimeout=5000,ssl=true,password=QqWKr+dVW5sNzxcU5ObYjRIgGmFvRqLUktbWZ7wzTL4="
                }, 
            });
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            var configDirectoryPath = HttpContext.Current.Server.MapPath(@"~/Config/");
            configManager.Init(configDirectoryPath);
        }

        private static void InitI18NMessageManager()
        {
            var configDirectoryPath = HttpContext.Current.Server.MapPath(@"~/Config/");
            var messageManager = MessageManager.GetInstance();
            messageManager.Init(configDirectoryPath);
        }

        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = ConfigManager.GetInstance().GetConfigValue("general", "seqGeneratorContainerName");
            var storageConnectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            var optimisticData = new BlobOptimisticDataStore(CloudStorageAccount.Parse(storageConnectionString), seqContainerName)
            {
                SeedValueInitializer = (sequenceName) => generator.GetIdInitialValue(sequenceName)
            };
            generator.Init(optimisticData);
            generator.BatchSize = 100;
        }

        private static void InitDatabaseService()
        {
            var database = DbService.GetInstance();
            var connectionString = ConfigManager.GetInstance().GetConfigValue("db", "connectionString");
            database.Init(connectionString);
        }
        private static void InitQueueService()
        {
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            IQueueClient queueClient = new AzureQueueClient();
            queueClient.init(connectionString);
            var queue = QueueService.GetInstance();
            queue.Init(queueClient);
        }
        private static void InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = log4net.LogManager.GetLogger("Log");
            LunggoLogger.GetInstance().init(Log);
        }

    }
}