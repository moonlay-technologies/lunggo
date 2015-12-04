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
        public static void FlightInstantPaymentConfirmedNotifEmail([QueueTrigger("flightinstantpaymentconfirmednotifemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Instant Payment Confirmed Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetOverviewReservation(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {reservation.Contact.Email},
                Subject = envPrefix + "[Travorama] Terima Kasih Sudah Memesan di Travorama - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            var mailModel2 = new MailModel
            {
                RecipientList = new[] { "rama.adhitia@travelmadezy.com", "developer@travelmadezy.com" },
                Subject = envPrefix + env == "production" ? "Pelunasan Reservasi No. " + rsvNo : "[TEST] Ignore This Email",
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(reservation, mailModel, "FlightInstantPaymentConfirmedNotifEmail");
            mailService.SendEmail(reservation, mailModel2, "ReservationNotice");

            Console.WriteLine("Done Processing Flight Instant Payment Confirmed Notif Email for RsvNo " + rsvNo);
        }
    }
}
