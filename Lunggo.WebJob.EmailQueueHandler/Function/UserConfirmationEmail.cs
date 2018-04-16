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
        public static void UserConfirmationEmail([QueueTrigger("userconfirmationemail")] string messageJson)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            var message = JsonConvert.DeserializeObject<IdentityMessage>(messageJson);
            var address = message.Destination;
            var link = message.Body;
            Console.WriteLine("Processing User Confirmation Email for " + address + "...");

            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {address},
                BccList = new[] { "maillog.travorama@gmail.com" },
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama",
                Subject = envPrefix + "[Travorama] Verifikasikan E-mail Anda"
            };
            mailService.SendEmailWithTableTemplate(message, mailModel, "UserConfirmationEmail");
            sw.Stop();

            Console.WriteLine("Done Processing User Confirmation Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
