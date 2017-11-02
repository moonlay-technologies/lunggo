using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void TransferInstructionEmail([QueueTrigger("transferinstructionemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            if (rsvNo.StartsWith("1"))
            {
                var flightService = FlightService.GetInstance();
                var sw = new Stopwatch();
                Console.WriteLine("Processing Transfer Instruction Email for RsvNo " + rsvNo + "...");

                Console.WriteLine("Getting Required Data...");
                sw.Start();
                var reservation = flightService.GetReservationForDisplay(rsvNo);
                sw.Stop();
                Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                var mailService = MailService.GetInstance();
                var mailModel = new MailModel
                {
                    RecipientList = new[] {reservation.Contact.Email},
                    BccList = new[] { "maillog.travorama@gmail.com" },
                    Subject =
                        envPrefix + "[Travorama] Harap Selesaikan Pembayaran Anda - No. Pemesanan " + reservation.RsvNo,
                    FromMail = "booking@travorama.com",
                    FromName = "Travorama"
                };
                if (reservation.Payment.Method == PaymentMethod.BankTransfer)
                {
                    Console.WriteLine("Sending Bank Transfer Instruction Email...");
                    mailService.SendEmail(reservation, mailModel, "BankTransferInstructionEmail");
                }
                else if (reservation.Payment.Method == PaymentMethod.VirtualAccount)
                {
                    Console.WriteLine("Sending Virtual Account Instruction Email...");
                    mailService.SendEmail(reservation, mailModel, "VirtualAccountInstructionEmail");
                }

                Console.WriteLine("Done Processing Transfer Instruction Email for RsvNo " + rsvNo);
            }
            else if (rsvNo.StartsWith("2"))
            {
                var hotelService = ApCommon.Hotel.Service.HotelService.GetInstance();
                var sw = new Stopwatch();
                Console.WriteLine("Processing Transfer Instruction Email for RsvNo " + rsvNo + "...");

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
                    Subject = envPrefix + "[Travorama] Harap Selesaikan Pembayaran Anda - No. Pemesanan " + reservation.RsvNo,
                    FromMail = "booking@travorama.com",
                    FromName = "Travorama"
                };
                if (reservation.Payment.Method == PaymentMethod.BankTransfer)
                {
                    Console.WriteLine("Sending Bank Transfer Instruction Email...");
                    mailService.SendEmail(reservation, mailModel, "BankTransferInstructionEmailHotel");
                }
                else if (reservation.Payment.Method == PaymentMethod.VirtualAccount)
                {
                    Console.WriteLine("Sending Virtual Account Instruction Email...");
                    mailService.SendEmail(reservation, mailModel, "VirtualAccountInstructionEmailHotel");
                }

                Console.WriteLine("Done Processing Transfer Instruction Email for RsvNo " + rsvNo);
            }
            else
            {
                var activityService = ApCommon.Activity.Service.ActivityService.GetInstance();
                var sw = new Stopwatch();
                Console.WriteLine("Processing Transfer Instruction Email for RsvNo " + rsvNo + "...");

                Console.WriteLine("Getting Required Data...");
                sw.Start();
                var reservation = activityService.GetReservationForDisplay(rsvNo);
                sw.Stop();
                Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                var mailService = MailService.GetInstance();
                var mailModel = new MailModel
                {
                    RecipientList = new[] { reservation.Contact.Email },
                    BccList = null,
                    Subject = envPrefix + "[Travorama] Harap Selesaikan Pembayaran Anda - No. Pemesanan " + reservation.RsvNo,
                    FromMail = "booking@travorama.com",
                    FromName = "Travorama"
                };
                //if (reservation.Payment.Method == PaymentMethod.BankTransfer)
                //{
                //    Console.WriteLine("Sending Bank Transfer Instruction Email...");
                //    mailService.SendEmail(reservation, mailModel, "BankTransferInstructionEmailHotel");
                //}
                //else if (reservation.Payment.Method == PaymentMethod.VirtualAccount)
                //{
                //    Console.WriteLine("Sending Virtual Account Instruction Email...");
                //    mailService.SendEmail(reservation, mailModel, "VirtualAccountInstructionEmailHotel");
                //}

                Console.WriteLine("Done Processing Transfer Instruction Email for RsvNo " + rsvNo);
            }
            
        }
    }
}
