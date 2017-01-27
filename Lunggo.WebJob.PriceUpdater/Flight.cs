using System;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;

namespace Lunggo.WebJob.PriceUpdater
{
    partial class Program
    {
        public static void FlightPriceUpdater(string origin, string destination, DateTime departureDate)
        {
            Console.WriteLine(@"Price updater for " + destination);
            try
            {
                FlightService.GetInstance().SearchFlight(new SearchFlightInput
                {
                    Progress = 0,
                    SearchId = origin + destination + departureDate.ToString("ddMMyy") + "-100y"
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Price Calendar is FAILED!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
