using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
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
        public static void FlightPendingPaymentExpiredNotifEmail([QueueTrigger("flightpendingpaymentexpirednotifemail")] string rsvNo)
        {
            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Pending Payment ExpiredNotif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetOverviewReservation(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            if (reservation.Payment.Status == PaymentStatus.Pending)
            {
                flightService.UpdateFlightPayment(rsvNo, new PaymentInfo
                {
                    Status = PaymentStatus.Expired
                });
                var mailService = MailService.GetInstance();
                var mailModel = new MailModel
                {
                    RecipientList = new[] {reservation.Contact.Email},
                    Subject =
                        "[Travorama] Reservasi Anda Kadaluarsa - No. Pemesanan " + reservation.RsvNo,
                    FromMail = "booking@travorama.com",
                    FromName = "Travorama"
                };
                Console.WriteLine("Sending Notification Email...");
                mailService.SendEmail(reservation, mailModel, "FlightPendingPaymentNotifEmail");
            }
            else
            {
                Console.WriteLine("Flight Reservation Already Paid...");
            }

            Console.WriteLine("Done Processing Flight Pending Payment Notif Email for RsvNo " + rsvNo);
        }
    }
}
