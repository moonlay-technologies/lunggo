﻿using System.IO;
using System.Web;
using log4net;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Service;
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
            InitDatabaseService();
            //InitQueueService();
            //InitLogger();
            InitDictionaryService();
            InitFlightService();
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

        private static void InitDictionaryService()
        {
            var dictionary = DictionaryService.GetInstance();
            var airlineFileName = ConfigManager.GetInstance().GetConfigValue("general", "airlineFileName");
            var airlineFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/Config/"), airlineFileName);
            var airportFileName = ConfigManager.GetInstance().GetConfigValue("general", "airportFileName");
            var airportFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/Config/"), airportFileName);
            dictionary.Init(airlineFilePath, airportFilePath);
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }
    }
}