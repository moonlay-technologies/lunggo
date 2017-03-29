using System;
using Lunggo.Framework.Config;
//using Microsoft.Azure.WebJobs;

namespace Lunggo.Webjob.PriceCalendarUpdater
{
    partial class Program
    {
        static void Main()
        {
            Init();
            HotelPriceUpdater();
            //JobHostConfiguration configuration = new JobHostConfiguration();
            //configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(4);
            //configuration.Queues.MaxDequeueCount = 10;
            //configuration.StorageConnectionString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            //configuration.DashboardConnectionString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");

            //var host = new JobHost(configuration);
            //// The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();
        }

    }
}

