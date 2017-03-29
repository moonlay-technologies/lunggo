using System;
using System.Diagnostics;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void GetCalendarEmail([QueueTrigger("getcalendar")] string address)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Get Calendar Email for " + address + "...");

            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { address },
                BccList = new[] { "maillog.travorama@gmail.com" },
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama",
                Subject = envPrefix + "[Travorama] Selamat, Anda mendapatkan sebuah Calendar"
            };
            mailService.SendEmail(address, mailModel, "GetCalendarEmail");
            sw.Stop();

            Console.WriteLine("Done Processing Get Calendar Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
