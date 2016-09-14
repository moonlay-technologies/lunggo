using System;
using System.Diagnostics;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        //public static void InitialSubscriberEmail([QueueTrigger("initialsubscriberemail")] string messageJson)
        //{
        //    var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
        //    var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

        //    var sw = new Stopwatch();
        //    var message = JsonConvert.DeserializeObject<SubscriberEmailModel>(messageJson);
        //    var address = message.Email;
        //    Console.WriteLine("Processing Initial Subscriber Email for " + address + "...");

        //    sw.Start();
        //    var mailService = MailService.GetInstance();
        //    var mailModel = new MailModel
        //    {
        //        RecipientList = new[] {address},
        //        FromMail = "no-reply@travorama.com",
        //        FromName = "Travorama.com",
        //        Subject = envPrefix + "[Travorama] Terima Kasih Telah Berlangganan Newsletter Travorama"
        //    };
        //    mailService.SendEmail(message, mailModel, "InitialSubscriberEmail");
        //    sw.Stop();

        //    Console.WriteLine("Done Processing Initial Subscriber Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        //}
    }
}
