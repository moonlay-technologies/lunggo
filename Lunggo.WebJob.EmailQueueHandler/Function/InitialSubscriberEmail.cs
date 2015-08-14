using System;
using System.Diagnostics;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void InitialSubscriberEmail([QueueTrigger("initialsubscriberemail")] string address)
        {
            var sw = new Stopwatch();
            Console.WriteLine("Processing Initial Subscriber Email for " + address + "...");

            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {address},
                FromMail = "jangan-balas-ke-sini@travorama.com",
                FromName = "Travorama.com",
                Subject = "f dgbdvuen sdghuhfsu gfrngk Anda"
            };
            mailService.SendEmail(address, mailModel, HtmlTemplateType.InitialSubscriberEmail);
            sw.Stop();

            Console.WriteLine("Done Processing Initial Subscriber Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
