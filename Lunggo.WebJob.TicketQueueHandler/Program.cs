using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Core.CustomTraceListener;
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
            CloudQueue _queue = queueService.GetQueueByReference(Queue.FlightEticket); // TODO Flight placeholder queue, change it
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
            InitTraceListener();
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
            var queue = QueueService.GetInstance();
            queue.Init();
        }
        private static void InitTicketService()
        {
            var apiKey = ConfigManager.GetInstance().GetConfigValue("zendesk", "apikey");
            ITicketSupportClient ticket = new ZendeskTicketClient();
            ticket.init(apiKey);
            var TicketService = TicketSupportService.GetInstance();
            try
            {
                TicketService.Init(ticket);
            }
            catch (Exception ex)
            {
                throw new Exception("gagal init TicketService");
            }
        }

        private static void InitTraceListener()
        {
            Trace.Listeners.Clear();
            var connectionString = ConfigManager.GetInstance().GetConfigValue("azurestorage", "connectionString");
            string traceName = typeof (TableTraceListener).Name;
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
