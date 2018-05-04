using System.Web;
using System.Web.WebPages;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.BrowserDetection;
using Lunggo.Framework.Core;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;
using Lunggo.Framework.Environment;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SnowMaker;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage;

namespace Lunggo.BackendWeb
{
    public class AppInitializer
    {
        public static void Init()
        {
            InitI18NMessageManager();
            InitRedisService();
            InitDatabaseService();
            InitQueueService();
            InitBlobStorageService();
            InitUniqueIdGenerator();
            InitFlightService();
            //InitBrowserDetectionService();
            InitDisplayModes();
            InitMailService();
            InitHtmlTemplateService();
            InitTableStorageService();
            InitHotelService();
        }

        private static void InitBlobStorageService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init(connString);
        }

        private static void InitDocumentsService()
        {
            var endpoint = EnvVariables.Get("documentDb", "endpoint");
            var authKey = EnvVariables.Get("documentDb", "authorizationKey");
            var dbName = EnvVariables.Get("documentDb", "databaseName");
            var collectionName = EnvVariables.Get("documentDb", "collectionName");
            DocumentService.GetInstance().Init(endpoint, authKey, dbName, collectionName);
        }

        private static void InitMailService()
        {
            var apiKey = EnvVariables.Get("mandrill", "apiKey");
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
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "searchResultCacheConnectionString")
                },
                
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "masterDataCacheConnectionString")
                }, 
                 
            });
        }

        private static void InitI18NMessageManager()
        {
            var messageManager = MessageManager.GetInstance();
            messageManager.Init("Config");
        }

        private static void InitUniqueIdGenerator()
        {
            var generator = UniqueIdGenerator.GetInstance();
            var seqContainerName = EnvVariables.Get("general", "seqGeneratorContainerName");
            var optimisticData = new BlobOptimisticDataStore(seqContainerName)
            {
                SeedValueInitializer = (sequenceName) => generator.GetIdInitialValue(sequenceName)
            };
            generator.Init(optimisticData);
            generator.BatchSize = 100;
        }

        private static void InitDatabaseService()
        {
            var connString = EnvVariables.Get("db", "connectionString");
            var database = DbService.GetInstance();
            database.Init(connString);
        }
        private static void InitQueueService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("Config");
        }

        private static void InitHotelService()
        {
            var hotel = HotelService.GetInstance();
            hotel.Init("config");
        }

        private static void InitBrowserDetectionService()
        {
            var wurflDataFile = HttpContext.Current.Server.MapPath("~/App_Data/wurfl-latest.zip");
            var service = BrowserDetectionService.GetInstance();
            service.Init(wurflDataFile);
        }

        private static void InitDisplayModes()
        {
            var mobileUrl = EnvVariables.Get("general", "mobileUrl");
            DisplayModeProvider.Instance.Modes.Clear();
            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("mobile")
            {
                ContextCondition = context =>
                                context.Request.Url.Host == mobileUrl
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
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var tableStorageService = TableStorageService.GetInstance();
            tableStorageService.Init(connString);
        }
    }
}