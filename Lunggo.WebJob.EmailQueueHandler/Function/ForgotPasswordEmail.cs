using System;
using System.Diagnostics;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void ForgotPasswordEmail([QueueTrigger("forgotpasswordemail")] string messageJson)
        {
            var sw = new Stopwatch();
            var message = JsonConvert.DeserializeObject<IdentityMessage>(messageJson);
            var address = message.Destination;
            var link = message.Body;
            Console.WriteLine("Processing Forgot Password Email for " + address + "...");

            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {address},
                FromMail = "jangan-balas-ke-sini@travorama.com",
                FromName = "Travorama.com",
                Subject = "Forgotten Password"
            };
            mailService.SendEmail(message, mailModel, HtmlTemplateType.UserConfirmationEmail);
            sw.Stop();

            Console.WriteLine("Done Processing Forgot Password Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
