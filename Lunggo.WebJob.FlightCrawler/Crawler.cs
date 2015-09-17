using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Encoder;
using Microsoft.Azure.WebJobs;
using Microsoft.Owin.Logging;

namespace Lunggo.WebJob.FlightCrawler
{
    partial class Program
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.

        public static void Crawl(object targetString)
        {
            var flightService = FlightService.GetInstance();
            var splittedMessage = targetString.ToString().Split('.');
            var conditionString = splittedMessage[0];
            var timeout = int.Parse(splittedMessage[1]);
            var decodedConditionString = conditionString.Base64Decode();
            Console.WriteLine("Invocation: Process Id: {0}, Thread Id: {1}.",
                System.Diagnostics.Process.GetCurrentProcess().Id, Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Processing : " + decodedConditionString);
            //flightService.SearchFlightAndFillInSearchCache(conditionString, timeout);
            Thread.Sleep(60000);
            Console.WriteLine("Done : " + decodedConditionString);
        }
    }
}
