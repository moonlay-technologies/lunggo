using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightIssueTimeoutNotifEmail([QueueTrigger("flightissuetimeoutnotifemail")] string rsvNo)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            Console.WriteLine("Processing Flight Issue Timeout Notif Developer for RsvNo " + rsvNo + "...");

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "all@travelmadezy.com" },
                Subject = envPrefix + "[Travorama] Issue Timeout - No. Pemesanan " + rsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmailWithTableTemplate(rsvNo, mailModel, "FlightIssueTimeoutNotifEmail");

            Console.WriteLine("Done Processing Flight Issue Timeout Notif Developer for RsvNo " + rsvNo);
        }
    }
}
