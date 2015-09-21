using System;
using System.Diagnostics;
using Lunggo.ApCommon.Subscriber;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Mandrill.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void InitialSubscriberEmail([QueueTrigger("initialsubscriberemail")] string messageJson)
        {
            var sw = new Stopwatch();
            var message = JsonConvert.DeserializeObject<SubscriberEmailModel>(messageJson);
            var address = message.Email;
            Console.WriteLine("Processing Initial Subscriber Email for " + address + "...");

            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {address},
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama.com",
                Subject = "[Travorama] Terima Kasih Telah Berlangganan Newsletter Travorama"
            };
            mailService.SendEmail(message, mailModel, HtmlTemplateType.InitialSubscriberEmail);
            sw.Stop();

            Console.WriteLine("Done Processing Initial Subscriber Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
