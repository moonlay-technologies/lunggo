using System;
using System.Diagnostics;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Core.CustomTraceListener;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Testinging
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            Init();

            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference(QueueService.Queue.Eticket);
            queue.AddMessage(new CloudQueueMessage("F2ABC25T"));

        }
        public static void Init()
        {
            InitConfigurationManager();
            InitFlightService();
            InitDatabaseService();
            InitDictionaryService();
            InitQueueService();
            InitHtmlTemplateService();
            InitMailService();
            InitBlobStorageService();
            //InitTraceListener();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init("Config/");
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

        private static void InitDatabaseService()
        {
            var db = DbService.GetInstance();
            db.Init();
        }

        private static void InitDictionaryService()
        {
            var dict = DictionaryService.GetInstance();
            dict.Init();
        }

        private static void InitQueueService()
        {
            var queue = QueueService.GetInstance();
            queue.Init();
        }

        private static void InitMailService()
        {   
            var mailService = MailService.GetInstance();
            mailService.Init();
        }

        public static void InitHtmlTemplateService()
        {
            var htmlTemplateService = HtmlTemplateService.GetInstance();
            htmlTemplateService.Init();
        }

        public static void InitBlobStorageService()
        {
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init();
        }
    }
}
