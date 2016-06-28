using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightCrawler
{
    public partial class Program
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessCrawlQueue6([QueueTrigger("flightcrawl6")] string searchId)
        {
            var flight = FlightService.GetInstance();
            Console.WriteLine("Searching for "+searchId+" from supplier 6...");
            var sw = Stopwatch.StartNew();
            flight.CommenceSearchFlight(searchId, 6);
            sw.Stop();
            Console.WriteLine("Done searching " + searchId + " from supplier 6. (" + sw.ElapsedMilliseconds/1000 + " s)");
        }
    }
}
