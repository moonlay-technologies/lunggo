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
        public static void RegisterSubscribeEmail([QueueTrigger("registersubscribeemail")] string address)
         {
             var env = EnvVariables.Get("general", "environment");
             var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

             var sw = new Stopwatch();
             Console.WriteLine("Processing Subscribe Email for " + address + "...");

             sw.Start();
             var mailService = MailService.GetInstance();
             var mailModel = new MailModel
             {
                 RecipientList = new[] { address },
                 BccList = new[] { "maillog.travorama@gmail.com" },
                 FromMail = "no-reply@travorama.com",
                 FromName = "Travorama",
                 Subject = envPrefix + "[Travorama] Terimakasih, Anda telah berlangganan di Travorama"
             };
             mailService.SendEmailWithTableTemplate(address, mailModel, "RegisterSubscribeEmail");
             sw.Stop();

             Console.WriteLine("Done Processing Subscribe Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
         }
    }
}
