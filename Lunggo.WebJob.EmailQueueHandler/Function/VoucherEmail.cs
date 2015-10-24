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
        public static void VoucherEmail([QueueTrigger("voucheremail")] string messageJson)
        {
            var sw = new Stopwatch();
            var message = JsonConvert.DeserializeObject<VoucherEmailModel>(messageJson);
            var address = message.Email;
            Console.WriteLine("Processing Voucher Email for " + address + "...");

            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {address},
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama",
                Subject = "Selamat! Voucher Diskon Hingga Rp 500.000 untuk Anda"
            };
            mailService.SendEmail(message, mailModel, "VoucherEmail");
            sw.Stop();

            Console.WriteLine("Done Processing Voucher Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
