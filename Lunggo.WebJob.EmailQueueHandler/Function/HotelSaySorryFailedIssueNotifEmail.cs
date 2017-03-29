using System;
using System.Diagnostics;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void HotelSaySorryFailedIssueNotifEmail([QueueTrigger("hotelsaysorryfailedissuenotifemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var hotelService = HotelService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Hotel Say Sorry Failed Issue Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = hotelService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { reservation.Contact.Email },
                BccList = new[] { "maillog.travorama@gmail.com" },
                Subject = envPrefix + "[Travorama] Pemesanan Gagal - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(reservation, mailModel, "HotelSaySorryFailedIssueNotifEmail");

            Console.WriteLine("Done Processing Hotel Say Sorry Failed Issue Notif Email for RsvNo " + rsvNo);
        }
    }
}
