using System;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;
using Lunggo.ApCommon.Query;
using Lunggo.Framework.Environment;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void ContactUs([QueueTrigger("contactusemail")] string msg)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var splittedMsg = msg.Split('+');
            var name = splittedMsg[0];
            var email = splittedMsg[1];
            var message = splittedMsg[2];

            var emailData = new ContactUs
            {
                Name = name,
                Email = email,
                Message = message
            };
         // { "cs@travorama.com" },
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "cs@travorama.com" },
                Subject = envPrefix + "[Travorama] 1 Person Contacted Us" ,
                FromMail = email,
                FromName = name
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmailWithTableTemplate(emailData, mailModel, "contactusemail");
            Console.WriteLine("Done Sending Notification Email...");
        }
    }
}
