using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void B2BHotelRejectionEmail([QueueTrigger("b2bhotelrejectionemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";
            var hotelService = HotelService.GetInstance();
            var sw = new Stopwatch();
            var counter = 0;
            Console.WriteLine("Processing B2B Hotel Rejection Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = hotelService.GetBookerReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { reservation.Contact.Email },
                Subject = envPrefix + "[Travorama] Pesanan Hotel Anda Ditolak Perusahaan - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
                BccList = new[] { "maillog.travorama@gmail.com" }
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(reservation, mailModel, "B2BHotelRejectionEmail");

            Console.WriteLine("Done Processing B2B Hotel Rejection Email for RsvNo " + rsvNo);
        }
    }
}
