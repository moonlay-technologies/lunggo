using System.IO;
using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Service;
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
            InitRedisService();
            InitAutocompleteManager();
            InitDictionaryService();
            InitFlightService();
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
        private static void InitAutocompleteManager()
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            var airportFileName = ConfigManager.GetInstance().GetConfigValue("general", "airportFileName");
            var airportFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/Config/"), airportFileName);
            var hotelLocationFileName = ConfigManager.GetInstance().GetConfigValue("general", "hotelLocationFileName");
            var hotelLocationFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~/Config/"), hotelLocationFileName);
            autocompleteManager.Init(airportFilePath, hotelLocationFilePath);
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