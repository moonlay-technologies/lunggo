using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightIssueDelayApproverNotifEmail([QueueTrigger("flightissuedelayapprovernotifemail")] string message)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            var splitMessage = message.Split(':');
            var rsvNo = splitMessage[0];
            var caseType = splitMessage[1];
            var approverEmail = splitMessage[2];
            Console.WriteLine("Processing Flight Issue Delay Approver Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            FlightSlightDelay EmailData = new FlightSlightDelay
            {
                CaseType = caseType,
                Reservation = reservation
            };

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { approverEmail },
                Subject = envPrefix + "[Travorama] Keterlambatan Pengiriman Etiket - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(EmailData, mailModel, "FlightIssueDelayApproverNotifEmail");

            Console.WriteLine("Done Processing Flight Issue Delay Approver Notif Email for RsvNo " + rsvNo);
        }
    }
}
