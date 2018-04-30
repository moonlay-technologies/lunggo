using System;
using System.Diagnostics;
using log4net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.Core.CustomTraceListener;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.TableStorage;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    partial class Program
    {
        static void Main()
        {
            Init();

            JobHostConfiguration configuration = new JobHostConfiguration();
            configuration.Queues.MaxPollingInterval = TimeSpan.FromSeconds(4);
            configuration.Queues.MaxDequeueCount = 10;
            configuration.StorageConnectionString = EnvVariables.Get("azureStorage", "connectionString");
            configuration.DashboardConnectionString = EnvVariables.Get("azureStorage", "connectionString");

            JobHost host = new JobHost(configuration);
            //Function.ProcessEmailQueue.FlightIssueFailedNotifEmail("137816601979+AirAsia;435671;1231562");
            host.RunAndBlock();

        }
    }
}
