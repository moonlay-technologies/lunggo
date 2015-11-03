using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.WebJob.FlightCrawlScheduler
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    partial class Scheduler
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage

        static void Main()
        {
            const int noOfProcessesBeforeWait = 1000;
            const int waitPeriodInSeconds = 2;

            Init();
            for (;;)
            {
                var targets = GetFlightCrawlTargets();
                var targetStrings = GenerateTargetStrings(targets);
                var counter = 0;
                foreach (var targetString in targetStrings)
                {
                    counter++;
                    ProcessTargetStrings(targetString);
                    Console.WriteLine("Enqueue : " + targetString.Split('.')[0].Base64Decode());
                    if (counter == noOfProcessesBeforeWait)
                    {
                        Thread.Sleep(waitPeriodInSeconds*1000);
                        counter = 0;
                    }
                }
            }
        }

        private static void ProcessTargetStrings(string targetString)
        {
            var flightService = FlightService.GetInstance();
            var queue = QueueService.GetInstance().GetQueueByReference("FlightCrawl");
            var conditionString = targetString.Split('.')[0];
            var isExpired = flightService.GetSearchedItinerariesExpiry(conditionString, 0) == null;
            if (isExpired)
            {    
                var message = new CloudQueueMessage("", "");
                message.SetMessageContent(targetString);
                queue.AddMessage(message);
            }
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
                    var conditionString = flightService.EncodeConditions(searchCondition);
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
