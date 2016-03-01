using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightPendingPaymentExpiredNotifEmail([QueueTrigger("flightpendingpaymentexpirednotifemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Pending Payment Expired Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            if (reservation.Payment.Status == PaymentStatus.Pending)
            {
                flightService.UpdateFlightPayment(rsvNo, new Payment
                {
                    Status = PaymentStatus.Expired
                });
                PaymentService.GetInstance().UpdateTransferConfirmationReportStatus(rsvNo, TransferConfirmationReportStatus.Invalid);
                var mailService = MailService.GetInstance();
                var mailModel = new MailModel
                {
                    RecipientList = new[] {reservation.Contact.Email},
                    Subject =
                        envPrefix + "[Travorama] Reservasi Anda Kadaluarsa - No. Pemesanan " + reservation.RsvNo,
                    FromMail = "booking@travorama.com",
                    FromName = "Travorama"
                };
                Console.WriteLine("Sending Notification Email...");
                mailService.SendEmail(reservation, mailModel, "FlightPendingPaymentExpiredNotifEmail");
            }
            else
            {
                Console.WriteLine("Flight Reservation Already Paid...");
            }

            Console.WriteLine("Done Processing Flight Pending Payment Notif Email for RsvNo " + rsvNo);
        }
    }
}
