using System;
using System.Diagnostics;
using log4net;
using Lunggo.ApCommon.Dictionary;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Core.CustomTraceListener;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.TableStorage;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.WebJob.EticketQueueHandler
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            Init();

            Functions.ProcessQueueMessage("F2ABC25T");

            JobHostConfiguration configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(30);
            configuration.Queues.MaxDequeueCount = 10;

            JobHost host = new JobHost(configuration);
            //host.RunAndBlock();

        }
        public static void Init()
        {
            InitConfigurationManager();
            InitDatabaseService();
            InitDictionaryService();
            InitQueueService();
            InitHtmlTemplateService();
            InitMailService();
            
            //InitTraceListener();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init("Config/");
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

        private static void InitTraceListener()
        {
            Trace.Listeners.Clear();
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            string traceName = typeof(TableTraceListener).Name;
            var listener =
                new TableTraceListener(connectionString, "webjobTrace")
                {
                    Name = traceName
                };
            Trace.Listeners.Add(listener);
            log4net.Config.XmlConfigurator.Configure();
            ILog Log = log4net.LogManager.GetLogger("Log");
            LunggoLogger.GetInstance().init(Log);
        }
    }
}
