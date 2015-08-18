using System;
using System.Diagnostics;
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
        public static void Newsletter([QueueTrigger("newsletter")] string message)
        {
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {"travorama.newsletter@gmail.com"},
                FromMail = "newsletter@travorama.com",
                FromName = "Newsletter Travorama",
                Subject = "message"
            };
            mailService.SendEmail(message, mailModel, HtmlTemplateType.Newsletter);
        }
    }
}
