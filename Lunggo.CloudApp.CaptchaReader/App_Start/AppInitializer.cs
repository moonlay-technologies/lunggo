using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.CloudApp.CaptchaReader.Models;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.CloudApp.CaptchaReader
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitConfigurationManager();
            InitAccountManager();
        }

        public static void InitAccountManager()
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env == "production")
            {
                Account.AccountList.Add("trv.agent.dua");
                Account.AccountList.Add("trv.agent.tiga");
                Account.AccountList.Add("trv.agent.empat");
                Account.AccountList.Add("trv.agent.lima");
                Account.AccountList.Add("trv.agent.enam");
                Account.AccountList.Add("trv.agent.tujuh");
                Account.AccountList.Add("trv.agent.delapan");
                Account.AccountList.Add("trv.agent.sembilan");
                Account.AccountList.Add("trv.agent.sepuluh");
                Account.AccountList.Add("trv.agent.sebelas");
                Account.AccountList.Add("trv.agent.duabelas");
                Account.AccountList.Add("trv.agent.tigabelas");
                Account.AccountList.Add("trv.agent.empatbelas");
                Account.AccountList.Add("trv.agent.limabelas");
                Account.AccountList.Add("trv.agent.enambelas");
                Account.AccountList.Add("trv.agent.tujuhbelas");
                Account.AccountList.Add("trv.agent.delapanbelas");
                Account.AccountList.Add("trv.agent.sembilanbelas");
                Account.AccountList.Add("trv.agent.duapuluh");
                Account.AccountList.Add("trv.agent.duasatu");
                Account.AccountList.Add("trv.agent.duadua");
                Account.AccountList.Add("trv.agent.duatiga");
                Account.AccountList.Add("trv.agent.duaempat");
                Account.AccountList.Add("trv.agent.dualima");
                Account.AccountList.Add("trv.agent.duaenam");
                Account.AccountList.Add("trv.agent.duatujuh");
                Account.AccountList.Add("trv.agent.duadelapan");
                Account.AccountList.Add("trv.agent.duasembilan");
            }
            else
            {
                Account.AccountList.Add("trv.agent.satu");
            }
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

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            var configDirectoryPath = HttpContext.Current.Server.MapPath(@"~/Config/");
            configManager.Init(configDirectoryPath);
        }

        private static void InitI18NMessageManager()
        {
            var messageManager = MessageManager.GetInstance();
            messageManager.Init("Config");
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

        private static void InitDictionaryService()
        {
            var dictionary = DictionaryService.GetInstance();
            dictionary.Init("Config");
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }
    }
}