using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void HotelSaySorryFailedIssueApproverNotifEmail([QueueTrigger("hotelsaysorryfailedissueapprovernotifemail")] string message)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";
            var splitMessage = message.Split(':');
            var rsvNo = splitMessage[0];
            var approverEmail = splitMessage[1];
            var hotelService = HotelService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Hotel Say Sorry Failed Issue Approver Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = hotelService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { approverEmail },
                Subject = envPrefix + "[Travorama] Pemesanan Gagal - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
                BccList = new[] { "maillog.travorama@gmail.com" }
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(reservation, mailModel, "HotelSaySorryFailedIssueApproverNotifEmail");

            Console.WriteLine("Done Processing Hotel Say Sorry Failed Issue Approver Notif Email for RsvNo " + rsvNo);
        }
    }
}
