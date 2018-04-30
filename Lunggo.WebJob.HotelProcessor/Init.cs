using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Log;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;

namespace Lunggo.WebJob.HotelProcessor
{
    partial class Program
    {
        public static void Init()
        {
            InitDatabaseService();
            InitQueueService();
            InitRedisService();
            InitBlobStorageService();
            InitHtmlTemplateService();
            InitMailService();
            InitHotelService();
            InitLogService();
        }

        private static void InitHotelService()
        {
            var hotel= HotelService.GetInstance();
            hotel.Init("");
        }

        private static void InitDatabaseService()
        {
            var connString = EnvVariables.Get("db", "connectionString");
            var db = DbService.GetInstance();
            db.Init(connString);
        }

        private static void InitQueueService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitMailService()
        {
            var apiKey = EnvVariables.Get("mandrill", "apiKey");
            var mailService = MailService.GetInstance();
            mailService.Init(apiKey);
        }

        public static void InitHtmlTemplateService()
        {
            var htmlTemplateService = HtmlTemplateService.GetInstance();
            htmlTemplateService.Init();
        }

        public static void InitBlobStorageService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init(connString);
        }
        private static void InitRedisService()
        {
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.SearchResultCacheName,
                    ConnectionString = EnvVariables.Get("redis", "searchResultCacheConnectionString")
                },
                
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "masterDataCacheConnectionString")
                }, 
                 
            });
        }

        public static void InitLogService()
        {
            var webhookUrl = EnvVariables.Get("log", "slack");
            var log = LogService.GetInstance();
            log.Init(webhookUrl);
        }
    }
}
