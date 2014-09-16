using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.WindowsAzure.Storage.Queue;
using ZendeskApi_v2.Models.Constants;

namespace Lunggo.WebJob.EmailSuccessBooking
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        static void Main()
        {
            new Program().Init();
            var queueService = QueueService.GetInstance();
            var _queue = queueService.GetQueueByReference("apibookingsuccess");
            _queue.CreateIfNotExists();

            foreach (var cloudQueueMessage in _queue.GetMessages(200, TimeSpan.FromMinutes(5)))
            {
                PersonIdentity messageContent = cloudQueueMessage.Deserialize<PersonIdentity>();


                //Email Process


                _queue.DeleteMessage(cloudQueueMessage);
            }

        }
        public void Init()
        {
            InitConfigurationManager();
            InitQueueService();
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
    }
}
