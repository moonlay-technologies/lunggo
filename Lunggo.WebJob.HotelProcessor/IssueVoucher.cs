using System;
using System.Diagnostics;
using System.IO;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Microsoft.Azure.WebJobs;
using Lunggo.ApCommon.Hotel.Service;

namespace Lunggo.WebJob.HotelProcessor
{
    public partial class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void HotelIssueVoucher([QueueTrigger("hotelissuevoucher")] string rsvNo)
        {
            //var hotel = HotelService.GetInstance();
            Console.WriteLine("Processing Hotel Issue Voucher for RsvNo " + rsvNo + "...");
            var sw = Stopwatch.StartNew();
            HotelService.GetInstance().CommenceIssueHotel(new IssueHotelTicketInput {RsvNo = rsvNo});
            sw.Stop();
            Console.WriteLine("Done Processing Hotel Issue Voucher for RsvNo " + rsvNo + "... (" + sw.Elapsed.TotalSeconds + "s)");
        }
    }
}
