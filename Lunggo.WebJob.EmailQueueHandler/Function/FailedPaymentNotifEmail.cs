using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FailedPaymentNotifEmail([QueueTrigger("FailedPaymentNotifEmail")] string msg)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Failed Payment Notif Email ");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var splittedMessage = msg.Split('&');
            var rsvNo = splittedMessage[0];
            var approverEmail = splittedMessage[1];
            var userEmail = splittedMessage[2];
            var financeEmails = splittedMessage[3].Split('|').ToArray();
            var listEmail = new[] {approverEmail, userEmail};
            var emails = listEmail.Concat(financeEmails).ToArray();
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = emails,
                Subject = envPrefix + "[Travorama] Payment Process for Reservation Number " + rsvNo + " is Failed",
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(rsvNo, mailModel, "FailedPaymentNotifEmail");

            Console.WriteLine("Done Processing Failed Payment Notif Email");
        }
    }
}
