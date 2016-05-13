using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightEticketSlightDelayNotifEmail([QueueTrigger("flighteticketslightdelaynotifemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Eticket Slight Delay Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { reservation.Contact.Email },
                Subject = envPrefix + "[Travorama] CHANGE THIS - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");

            mailService.SendEmail(reservation, mailModel, "FlightEticketSlightDelayNotifEmail");

            Console.WriteLine("Done Processing Flight Eticket Slight Delay Notif Email for RsvNo " + rsvNo);
        }
    }
}
