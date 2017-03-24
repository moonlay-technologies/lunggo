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
        public static void UserSuspendNotifEmail([QueueTrigger("UserSuspendNotifEmail")] string msg)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing UserSuspend Notif Email ");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var msgSplit = msg.Split('&');
            var emailList = msgSplit[0];
            var status = msgSplit[1];
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new []{emailList},
                Subject = envPrefix + (status == "true" ? " [Travorama] Your Account Has Been Deactivated" : " [Travorama] Your Account Has Been Reactivated"),
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama",
                BccList = new[] { "maillog.travorama@gmail.com" }
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(status, mailModel, "UserSuspendNotifEmail");

            Console.WriteLine("Done Processing User Suspend Issue Notif Email");
        }
    }
}
