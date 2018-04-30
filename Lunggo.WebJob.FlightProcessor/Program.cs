using System;
using Lunggo.Framework.Config;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightProcessor
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    partial class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            Init();

            JobHostConfiguration configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(4);
            configuration.Queues.MaxDequeueCount = 10;
            configuration.StorageConnectionString = EnvVariables.Get("azureStorage", "connectionString");
            configuration.DashboardConnectionString = EnvVariables.Get("azureStorage", "connectionString");

            JobHost host = new JobHost(configuration);
            host.RunAndBlock();
        }
    }
}
