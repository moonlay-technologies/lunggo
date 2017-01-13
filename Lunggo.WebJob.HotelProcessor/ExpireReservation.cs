using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.HotelProcessor
{
    public partial class Functions
    {
        public static void FlightExpireReservation([QueueTrigger("hotelexpirereservation")] string rsvNo)
        {
            var hotel = HotelService.GetInstance();
            hotel.ExpireReservation(rsvNo);
            Console.WriteLine("Done Expiring Hotel Reservation for RsvNo " + rsvNo + "...");
        }
    }
}
