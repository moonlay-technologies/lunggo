using System;
using System.Diagnostics;
using System.IO;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.FlightProcessor
{
    public partial class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void FlightExpireReservation([QueueTrigger("flightexpirereservation")] string rsvNo)
        {
            var flight = FlightService.GetInstance();
            flight.ExpireReservation(rsvNo);
            Console.WriteLine("Done Expiring Flight Reservation for RsvNo " + rsvNo + "...");
        }
    }
}
