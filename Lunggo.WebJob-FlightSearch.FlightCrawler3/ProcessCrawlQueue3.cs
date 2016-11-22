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
        public static void ProcessCrawlQueue3([QueueTrigger("flightcrawl3")] string searchId)
        {
            var flight = FlightService.GetInstance();
            var searchParam = searchId.Split('|')[0];
            var searchTimeOut = DateTime.Parse(searchId.Split('|')[1]);
            Console.WriteLine("Searching for " + searchParam + " from supplier 3...");
            var sw = Stopwatch.StartNew();
            flight.CommenceSearchFlight(searchParam, 3, searchTimeOut);
            sw.Stop();
            Console.WriteLine("Done searching " + searchParam + " from supplier 3. (" + sw.ElapsedMilliseconds / 1000 + " s)");
            
        }
    }
}
