using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SnowMaker;
using Lunggo.Framework.TicketSupport;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Lunggo.WebJob.APIBookingFailed
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    internal class Program
    {

        private static void Main()
        {
            new Program().Init();
            CloudQueue _queue;
            var queueService = QueueService.GetInstance();
            _queue = queueService.GetQueueByReference("APIBookingFailed");

            var _queueInsert = queueService.GetQueueByReference("SendFromFailed");
            _queue.CreateIfNotExists();
            _queueInsert.CreateIfNotExists();
            foreach (var cloudQueueMessage in _queue.GetMessages(10,TimeSpan.FromMinutes(1)))
            {
                var messageContent = cloudQueueMessage.AsString;
                Trace.TraceInformation("Processing request: {0}", messageContent);

                _queueInsert.AddMessage(new CloudQueueMessage(DateTime.Now.ToString()));
                _queue.DeleteMessage(cloudQueueMessage);
            }
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
            string serverMapPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + "\\Config\\";
            var configDirectoryPath = serverMapPath;
            configManager.Init(configDirectoryPath);
        }

        private static void InitQueueService()
        {
            var connectionString = ConfigManager.GetInstance().GetConfigValue("AzureWebJobsStorage", "connectionString");
            IQueueClient queueClient = new AzureQueueClient();
            queueClient.init(connectionString);
            var queue = QueueService.GetInstance();
            queue.Init(queueClient);
        }
        private static void InitTicketService()
        {
            var TicketService = TicketSupportService.GetInstance();
            var apiKey = ConfigManager.GetInstance().GetConfigValue("zendesk", "apikey");
            TicketService.Init(apiKey);
        }
    }
    public static class CloudQueueMessageExtensions
    {
        public static CloudQueueMessage Serialize(Object o)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(o.GetType().FullName);
            stringBuilder.Append(':');
            stringBuilder.Append(JsonConvert.SerializeObject(o));
            return new CloudQueueMessage(stringBuilder.ToString());
        }

        public static T Deserialize<T>(this CloudQueueMessage m)
        {
            int indexOf = m.AsString.IndexOf(':');

            if (indexOf <= 0)
                throw new Exception(string.Format("Cannot deserialize into object of type {0}",
                    typeof(T).FullName));

            string typeName = m.AsString.Substring(0, indexOf);
            string json = m.AsString.Substring(indexOf + 1);

            if (typeName != typeof(T).FullName)
            {
                throw new Exception(string.Format("Cannot deserialize object of type {0} into one of type {1}",
                    typeName,
                    typeof(T).FullName));
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
