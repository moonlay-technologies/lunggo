using System;
using System.Diagnostics;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;
using Lunggo.ApCommon.Flight.Model;
using System.Collections.Generic;

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

            List<FlightIssueData> IssueData = new List<FlightIssueData>();

            for (int i = 1; i < splitMessage.Length; i++)
            {
                var splitSupplier = splitMessage[i].Split(';');
                var localPrice = decimal.Parse(splitSupplier[1]);
                var currentDeposit = decimal.Parse(splitSupplier[2]);
                var singleData = new FlightIssueData { 
                    SupplierName = splitSupplier[0],
                    SupplierPrice = localPrice,
                    CurentDeposit = currentDeposit
                };
                IssueData.Add(singleData);
            }

            var SlightDelayData = new FlightNotifDeveloper
            {
                RsvNo = rsvNo,
                FlightIssueData = IssueData
            };

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "suheri@travelmadezy.com" },
                Subject = envPrefix + env == "production" ? "Issue Failed - No Pemesanan :  " + rsvNo : "[TEST] Ignore This Email",
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(SlightDelayData, mailModel, "FlightIssueFailedNotifEmail");

            Console.WriteLine("Done Processing Flight Issue Failed Notif Email for RsvNo " + rsvNo);
        }
    }
}
