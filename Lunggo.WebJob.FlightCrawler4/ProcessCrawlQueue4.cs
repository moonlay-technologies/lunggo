﻿using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightCrawler
{
    public partial class Program
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessCrawlQueue4([QueueTrigger("flightcrawl4")] string searchId)
        {
            var flight = FlightService.GetInstance();
            Console.WriteLine("Searching for "+searchId+" from supplier 4...");
            var sw = Stopwatch.StartNew();
            flight.CommenceSearchFlight(searchId, 4);
            sw.Stop();
            Console.WriteLine("Done searching " + searchId + " from supplier 4. (" + sw.ElapsedMilliseconds/1000 + " s)");
        }
    }
}