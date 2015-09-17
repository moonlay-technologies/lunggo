using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;
using ThreadState = System.Diagnostics.ThreadState;

namespace Lunggo.WebJob.FlightCrawler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    partial class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage

        static void RunScheduler()
        {
            const int noOfProcessesBeforeWait = 5;
            const int waitPeriodInSeconds = 10;
            const int maxThreads = 3;

            for (;;)
            {
                var targets = GetFlightCrawlTargets();
                var targetStrings = GenerateTargetStrings(targets);
                var counter = 0;
                foreach (var targetString in targetStrings)
                {
                    var activeThreads = 0;
                    while (activeThreads >= maxThreads)
                    {
                        activeThreads = Process.GetCurrentProcess().Threads.Count;
                    }
                    Console.WriteLine(activeThreads);
                    counter += ProcessTargetStrings(targetString);
                    if (counter == noOfProcessesBeforeWait)
                    {
                        Thread.Sleep(waitPeriodInSeconds*1000);
                        counter = 0;
                    }
                }
            }
        }

        private static int ProcessTargetStrings(string targetString)
        {
            var flightService = FlightService.GetInstance();
            var conditionString = targetString.Split('.')[0];
            var isExpired = flightService.GetSearchedItinerariesExpiry(conditionString) == null;
            if (isExpired)
            {
                Console.WriteLine("Enqueue : " + targetString.Split('.')[0].Base64Decode());
                new Thread(Crawl).Start(targetString);
                return 1;
            }
            return 0;
        }

        private static IEnumerable<string> GenerateTargetStrings(IEnumerable<FlightCrawlTargetTableRecord> targets)
        {
            var flightService = FlightService.GetInstance();
            foreach (var target in targets)
            {
                var searchCondition = new SearchFlightConditions
                {
                    AdultCount = target.AdultCount.GetValueOrDefault(),
                    ChildCount = target.ChildCount.GetValueOrDefault(),
                    InfantCount = target.InfantCount.GetValueOrDefault(),
                    CabinClass = FlightService.ParseCabinClass(target.RequestedCabinClassCd),
                    Trips = new List<FlightTrip>
                    {
                        new FlightTrip
                        {
                            OriginAirport = target.OriginAirport,
                            DestinationAirport = target.DestinationAirport
                        }
                    }
                };
                for (var i = target.DaysAdvanceDepartureDateStart; i <= target.DaysAdvanceDepartureDateEnd; i++)
                {
                    searchCondition.Trips[0].DepartureDate = DateTime.UtcNow.AddDays(i.GetValueOrDefault());
                    var conditionString = flightService.HashEncodeConditions(searchCondition);
                    var timeout = target.Timeout;
                    var conditionStringWithTimeout = conditionString + '.' + timeout;
                    yield return conditionStringWithTimeout;
                }
            }
        }

        private static List<FlightCrawlTargetTableRecord> GetFlightCrawlTargets()
        {
            List<FlightCrawlTargetTableRecord> targets;
            using (var conn = DbService.GetInstance().GetOpenConnection())
                targets = FlightCrawlTargetTableRepo.GetInstance().FindAll(conn).ToList();
            return targets;
        }
    }
}
