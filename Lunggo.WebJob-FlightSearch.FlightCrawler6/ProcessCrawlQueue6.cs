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
            var searchParam = searchId.Split('|')[0];
            var searchTimeOut = DateTime.Parse(searchId.Split('|')[1]);
            Console.WriteLine("Searching for " + searchParam + " from supplier 6...");
            var sw = Stopwatch.StartNew();
            flight.CommenceSearchFlight(searchParam, 2, searchTimeOut);
            sw.Stop();
            Console.WriteLine("Done searching " + searchParam + " from supplier 6. (" + sw.ElapsedMilliseconds / 1000 + " s)");
        }
    }
}
