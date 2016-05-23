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
        public static void FlightIssueFailedNotifEmail([QueueTrigger("flightissuefailednotifemail")] string message)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            var splitMessage = message.Split('+');
            var rsvNo = splitMessage[0];
            
            

            Console.WriteLine("Processing Flight Issue Failed Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            /*dynamic emailData = reservation;
            if (splitMessage.Length > 2)
            {
                int index = 0;
                for (int i = 1; i < splitMessage.Length; i++)
                {
                    var splitSupplier = splitMessage[i].Split(';');
                    emailData.SupplierName[index] = splitSupplier[0];
                    emailData.LocalPrice[index] = splitSupplier[1];
                    emailData.CurrentDeposit[index] = splitSupplier[2];
                    index++;
                }
                emailData.ItinCount = index;
            }
            else 
            {
                var splitSupplier = splitMessage[1].Split(';');
                emailData.SupplierName = splitSupplier[0];
                emailData.SupplierName = splitSupplier[1];
                emailData.SupplierName = splitSupplier[2];
            }*/

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "developer@travelmadezy.com" },
                Subject = envPrefix + env == "production" ? "Issue Failed - No Pemesanan :  " + rsvNo : "[TEST] Ignore This Email",
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(reservation, mailModel, "FlightIssueFailedNotifEmail");

            Console.WriteLine("Done Processing Flight Issue Failed Notif Email for RsvNo " + rsvNo);
        }
    }
}
