﻿using System;
using System.Diagnostics;
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
                FromMail = "no-reply@travorama.com",
                FromName = "Travorama",
                Subject = "[Travorama] Verifikasikan E-mail Anda"
            };
            mailService.SendEmail(message, mailModel, "UserConfirmationEmail");
            sw.Stop();

            Console.WriteLine("Done Processing User Confirmation Email for " + address + " (" + sw.Elapsed.TotalSeconds + "s).");
        }
    }
}
