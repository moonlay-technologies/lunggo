using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void BookingDisabilityNotifEmail([QueueTrigger("BookingDisabilityNotifEmail")] string msg)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Booking Disability Notif Email ");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var msgSplit = msg.Split('&');
            var emailList = msgSplit[0];
            var listEmail = emailList.Split('|').ToArray();
            var status = msgSplit[1];
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = listEmail,
                Subject = envPrefix + "[Travorama] Booking System is Changed",
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama",
                BccList = new[] { "maillog.travorama@gmail.com" }
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(status, mailModel, "BookingDisabilityStatusNotifEmail");

            Console.WriteLine("Done Processing Booking Disability Issue Notif Email");
        }
    }
}
