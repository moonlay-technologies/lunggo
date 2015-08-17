using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightInstantPaymentNotifEmail([QueueTrigger("flightinstantpaymentnotifemail")] string rsvNo)
        {
            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Instant Payment Notif Email for RsvNo " + rsvNo + "...");

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
                Subject = "[Travorama] Terima Kasih Sudah Memesan di Travorama - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(reservation, mailModel, HtmlTemplateType.FlightInstantPaymentNotifEmail);

            Console.WriteLine("Done Processing Flight Instant Payment Notif Email for RsvNo " + rsvNo);
        }
    }
}
