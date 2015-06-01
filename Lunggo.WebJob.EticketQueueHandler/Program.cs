using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
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

            var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();

            var queueService = QueueService.GetInstance();
            var queue = queueService.GetQueueByReference("Eticket");
            var message
        }

        private static void Init()
        {
            InitConfigurationManager();
            InitFlightService();
        }

        private static void InitConfigurationManager()
        {
            var configManager = ConfigManager.GetInstance();
            configManager.Init(@"Config\");
        }

        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init();
        }

    }
}
