using System.Web;
using Lunggo.Framework.Config;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.WebAPI
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitConfigurationManager();
            //InitI18NMessageManager();
            InitUniqueIdGenerator();
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

        private static void InitRedisService()
        {
            //TODO Use Configuration File Do Not Hardcode
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = "search_result_cache",
                    ConnectionString = "lunggosearchdev.redis.cache.windows.net,allowAdmin=true,syncTimeout=5000,ssl=true,password=QqWKr+dVW5sNzxcU5ObYjRIgGmFvRqLUktbWZ7wzTL4="
                },
                new RedisConnectionProperty
                {
                    ConnectionName = "master_data_cache",
                    ConnectionString = "lunggodatadev.redis.cache.windows.net,allowAdmin=true,syncTimeout=5000,ssl=true,password=QqWKr+dVW5sNzxcU5ObYjRIgGmFvRqLUktbWZ7wzTL4="
                }, 
            });
        }
    }
}