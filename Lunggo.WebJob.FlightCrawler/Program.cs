using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightCrawler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    partial class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Init();

            var configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(4);
            configuration.Queues.MaxDequeueCount = 1;
            configuration.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=lunggodiagdv1;AccountKey=R9Dhgoj8966mL4CE+K2osiDA/hTlkSTDL4eybs1LlkINwWfqPP9gWQRljuj1GyH7mpvlTCR3plJN0AFfywp8/w==";
            configuration.DashboardConnectionString = "DefaultEndpointsProtocol=https;AccountName=lunggodiagdv1;AccountKey=R9Dhgoj8966mL4CE+K2osiDA/hTlkSTDL4eybs1LlkINwWfqPP9gWQRljuj1GyH7mpvlTCR3plJN0AFfywp8/w==";

            var host = new JobHost(configuration);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
