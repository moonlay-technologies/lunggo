using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightIssueSlightDelayNotifEmail([QueueTrigger("flightissueslightdelaynotifemail")] string message)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            var splitMessage = message.Split('+');
            var rsvNo = splitMessage[0];
            var caseType = splitMessage[1];
            Console.WriteLine("Processing Flight Issue Slight Delay Notif Email for RsvNo " + rsvNo + "...");

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
                RecipientList = new[] { reservation.Contact.Email },
                BccList = new[] { "maillog.travorama@gmail.com" },
                Subject = envPrefix + "[Travorama] Keterlambatan Pengiriman Etiket - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmailWithTableTemplate(EmailData, mailModel, "FlightIssueSlightDelayNotifEmail");

            Console.WriteLine("Done Processing Flight Issue Slight Delay Notif Email for RsvNo " + rsvNo);
        }
    }
}
