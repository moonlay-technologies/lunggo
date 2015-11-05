using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.WebAPI
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitConfigurationManager();
            //InitI18NMessageManager();
            InitDatabaseService();
            InitUniqueIdGenerator();
            InitRedisService();
            InitDictionaryService();
            InitAutocompleteManager();
            InitFlightService();
            InitQueueService();
            InitMailService();
            InitPaymentService();
            InitTableStorageService();
            InitHtmlTemplateService();
            InitPaymentService();
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

        private static void InitDatabaseService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("db", "connectionString");
            var database = DbService.GetInstance();
            database.Init(connString);
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
                }
            });
        }

        private static void InitDictionaryService()
        {
            var dict = DictionaryService.GetInstance();
            dict.Init("Config");
        }

        private static void InitAutocompleteManager()
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            autocompleteManager.Init();
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

        private static void InitPaymentService()
        {
            var payment = PaymentService.GetInstance();
            payment.Init();
        }

        private static void InitMailService()
        {
            var apiKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "apiKey");
            var mailService = MailService.GetInstance();
            mailService.Init(apiKey);
        }
        private static void InitQueueService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        public static void InitHtmlTemplateService()
        {
            var htmlTemplateService = HtmlTemplateService.GetInstance();
            htmlTemplateService.Init();
        }

        public static void InitTableStorageService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var table = TableStorageService.GetInstance();
            table.Init(connString);
        }

        public static void InitPaymentService()
        {
            var payment = PaymentService.GetInstance();
            payment.Init();
        }
    }
}