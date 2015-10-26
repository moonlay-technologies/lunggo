using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightCrawler
{
    public partial class Program
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("flightcrawl")] string searchId)
        {
            var flight = FlightService.GetInstance();
            Console.WriteLine("Searching for "+searchId+"...");
            var sw = Stopwatch.StartNew();
            //flight.CommenceSearchFlight(searchId);
            Thread.Sleep(1000);
            sw.Stop();
            Console.WriteLine("Done searching " + searchId + ". (" + sw.ElapsedMilliseconds/1000 + " s)");
        }
    }
}
