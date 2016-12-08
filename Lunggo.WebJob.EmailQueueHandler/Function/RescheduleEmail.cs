using System;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;
using Lunggo.ApCommon.Query;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void Reschedule([QueueTrigger("rescheduleemail")] string msg)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var splittedMsg = msg.Split('|');
            var rsvNo = splittedMsg[0];
            var name = splittedMsg[1];
            var email = splittedMsg[2];
            var message = splittedMsg[3];

            var emailData = new Reschedule
            {
                Name = name,
                Email = email,
                RsvNo = rsvNo,
                Message = message
            };
     
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "intan.yutami@travelmadezy.com" },
                Subject = envPrefix + "[Travorama] Reschedule Request (RsvNo = " + rsvNo + ")"  ,
                FromMail = email,
                FromName = name
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(emailData, mailModel, "rescheduleemail");
            Console.WriteLine("Done Sending Notification Email...");
        }
    }
}
