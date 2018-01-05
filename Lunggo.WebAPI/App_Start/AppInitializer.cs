using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Log;
using Lunggo.Framework.Mail;
using Lunggo.ApCommon.Notifications;
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
            InitRedisService();
            InitFlightService();
            InitQueueService();
            InitMailService();
            InitBlobStorageService();
            InitUniqueIdGenerator();
            InitPaymentService();
            InitTableStorageService();
            InitHtmlTemplateService();
            InitNotificationService();
            InitLogService();
            //InitDocumentsService();
            InitHotelService();
            InitAutocompleteManager();
        }

        private static void InitBlobStorageService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init(connString);
        }
        private static void InitDocumentsService()
        {
            var endpoint = ConfigManager.GetInstance().GetConfigValue("documentDb", "endpoint");
            var authKey = ConfigManager.GetInstance().GetConfigValue("documentDb", "authorizationKey");
            var dbName = ConfigManager.GetInstance().GetConfigValue("documentDb", "databaseName");
            var collectionName = ConfigManager.GetInstance().GetConfigValue("documentDb", "collectionName");
            DocumentService.GetInstance().Init(endpoint, authKey, dbName, collectionName);
        }

        private static void InitNotificationService()
        {
            var notif = NotificationService.GetInstance();
            var connString = ConfigManager.GetInstance().GetConfigValue("notification", "connectionString");
            var hubName = ConfigManager.GetInstance().GetConfigValue("notification", "hubName");
            notif.Init(connString, hubName);
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

        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = ConfigManager.GetInstance().GetConfigValue("general", "seqGeneratorContainerName");
            var optimisticData = new BlobOptimisticDataStore(seqContainerName)
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

        private static void InitAutocompleteManager()
        {
            var autocompleteManager = AutocompleteManager.GetInstance();
            autocompleteManager.Init();
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("Config");
        }

        private static void InitHotelService()
        {
            var flight = HotelService.GetInstance();
            flight.Init("Config");
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

        public static void InitLogService()
        {
            var webhookUrl = ConfigManager.GetInstance().GetConfigValue("log", "slack");
            var log = LogService.GetInstance();
            log.Init(webhookUrl);
        }
    }
}