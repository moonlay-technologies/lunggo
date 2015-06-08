using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Core.CustomTraceListener;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.TableStorage;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EticketQueueHandler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Init();
            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("eticketQueue");
            queue.CreateIfNotExists();

            var configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(30);
            configuration.Queues.MaxDequeueCount = 10;

            //JobHost host = new JobHost(configuration);
            //host.RunAndBlock();

        }
        public static void Init()
        {
            InitConfigurationManager();
            InitQueueService();
            InitMailService();
            InitTableStorageService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            const string configDirectoryPath = "Config/";
            configManager.Init(configDirectoryPath);
        }

        private static void InitQueueService()
        {
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            IQueueClient queueClient = new AzureQueueClient();
            queueClient.init(connectionString);
            var queue = QueueService.GetInstance();
            queue.Init(queueClient);
        }

        private static void InitMailService()
        {
            var defaultMailTable = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailTableName");
            var defaultRowKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailRowName");
            var mandrillTemplate = ConfigManager.GetInstance().GetConfigValue("mandrill", "templateOfMandrill");
            var mailApiKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "apikey");

            IMailTemplateEngine mailTemplate = new RazorMailTemplateEngine();
            mailTemplate.init(defaultMailTable, defaultRowKey);
            var mandrillClient = new MandrillMailClient();
            
            mandrillClient.init(mailApiKey, mandrillTemplate, mailTemplate);
            IMailClient mailClient = mandrillClient;
            var mailService = MailService.GetInstance();

            mailService.Init(mailClient);
        }

        public static void InitTableStorageService()
        {
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            ITableStorageClient tableStorageClient = new AzureTableStorageClient();
            tableStorageClient.init(connectionString);
            TableStorageService.GetInstance().Init(tableStorageClient);
        }
    }
}
