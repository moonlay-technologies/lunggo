using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.SnowMaker;
using Lunggo.Framework.TableStorage;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using ZendeskApi_v2.Models.Constants;
namespace Lunggo.WebJob.EmailQueueHandler
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            new Program().Init();
            var queueService = QueueService.GetInstance();
            var _queue = queueService.GetQueueByReference("emailqueue");
            _queue.CreateIfNotExists();

            JobHostConfiguration configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(30);
            configuration.Queues.MaxDequeueCount = 10;

            JobHost host = new JobHost(configuration);
            host.RunAndBlock();

        }
        public void Init()
        {
            InitConfigurationManager();
            InitQueueService();
            InitMailService();
            InitTableStorageService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            var configDirectoryPath = "Config/";
            try
            {
                configManager.Init(configDirectoryPath);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void InitQueueService()
        {
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            IQueueClient queueClient = new AzureQueueClient();
            try
            {
                queueClient.init(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception("gagal init queueClient");
            }
            var queue = QueueService.GetInstance();

            try
            {
                queue.Init(queueClient);
            }
            catch (Exception ex)
            {
                throw new Exception("gagal init queue");
            }
        }

        private static void InitMailService()
        {
            string defaultMailTable = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailTableName");
            string defaultRowKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailRowName");
            string mandrillTemplate = ConfigManager.GetInstance().GetConfigValue("mandrill", "templateOfMandrill");
            string mailApiKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "apikey");

            IMailTemplateEngine mailTemplate = new RazorMailTemplateEngine();
            mailTemplate.init(defaultMailTable, defaultRowKey);
            IMailClient mailClient = new MandrillMailClient();
            try
            {
                mailClient.init(mailApiKey, mandrillTemplate, mailTemplate);
            }
            catch (Exception ex)
            {
                throw new Exception("gagal init queueClient");
            }
            var mailService = MailService.GetInstance();

            try
            {
                mailService.init(mailClient);
            }
            catch (Exception ex)
            {
                throw new Exception("gagal init queue");
            }

        }

        private static void InitMailTemplate()
        {
        }
        public static void InitTableStorageService()
        {
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            ITableStorageClient tableStorageClient = new AzureTableStorageClient();
            tableStorageClient.init(connectionString);
            TableStorageService.GetInstance().init(tableStorageClient);
        }
    }
}
