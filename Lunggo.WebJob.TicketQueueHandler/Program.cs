using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Lunggo.Framework.TicketSupport;

namespace Lunggo.WebJob.TicketQueueHandler
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        private static void Main()
        {
            new Program().Init();
            var queueService = QueueService.GetInstance();
            CloudQueue _queue = queueService.GetQueueByReference("apibookingfailed");
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
            InitTicketService();
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
        private static void InitTicketService()
        {
            var TicketService = TicketSupportService.GetInstance();
            var apiKey = ConfigManager.GetInstance().GetConfigValue("zendesk", "apikey");
            try
            {
                TicketService.Init(apiKey);
            }
            catch (Exception ex)
            {
                throw new Exception("gagal init TicketService");
            }
        }
    }
}
