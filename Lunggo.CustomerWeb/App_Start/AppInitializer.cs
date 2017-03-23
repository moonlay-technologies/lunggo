using System.Web;
using System.Web.WebPages;
using log4net;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.BrowserDetection;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Lunggo.Framework.Database;
using Microsoft.WindowsAzure.Storage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.TableStorage;


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
            InitQueueService();
            //InitLogger();
            InitFlightService();
            InitPaymentService();
            InitBrowserDetectionService();
            InitDisplayModes();
            InitMailService();
            InitHtmlTemplateService();
            InitTableStorageService();
            InitBlobStorageService();
            InitHotelService();
        }
        private static void InitBlobStorageService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init(connString);
        }
        private static void InitMailService()
        {
            var apiKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "apiKey");
            var mailService = MailService.GetInstance();
            mailService.Init(apiKey);
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
            var messageManager = MessageManager.GetInstance();
            messageManager.Init("Config");
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
        private static void InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = log4net.LogManager.GetLogger("Log");
            LunggoLogger.GetInstance().init(Log);
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("Config");
        }

        private static void InitHotelService()
        {
            var hotel = HotelService.GetInstance();
            hotel.Init("Config");
        }

        private static void InitPaymentService()
        {
            var payment = PaymentService.GetInstance();
            payment.Init();
        }

        private static void InitBrowserDetectionService()
        {
            var wurflDataFile = HttpContext.Current.Server.MapPath("~/App_Data/wurfl-latest.zip");
            var service = BrowserDetectionService.GetInstance();
            service.Init(wurflDataFile);
        }

        private static void InitDisplayModes()
        {
            var configManager = ConfigManager.GetInstance();
            var mobileUrl = configManager.GetConfigValue("general", "mobileUrl");
            var b2bmobileUrl = configManager.GetConfigValue("general", "b2bMobileUrl");
            DisplayModeProvider.Instance.Modes.Clear();
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("mobile")
            {
                ContextCondition = context =>
                                context.Request.Url.Host == mobileUrl || context.Request.Url.Host == b2bmobileUrl
            });
            DisplayModeProvider.Instance.Modes.Insert(1, new DefaultDisplayMode(""));
        }
        public static void InitHtmlTemplateService()
        {
            var htmlTemplateService = HtmlTemplateService.GetInstance();
            htmlTemplateService.Init();
        }
        public static void InitTableStorageService()
        {
            var connString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var tableStorageService = TableStorageService.GetInstance();
            tableStorageService.Init(connString);
        }
    }
}