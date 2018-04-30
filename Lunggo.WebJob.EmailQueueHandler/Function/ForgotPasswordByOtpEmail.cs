using Lunggo.ApCommon.Activity.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void ForgotPasswordOtpByEmail([QueueTrigger("forgotpasswordbyotpemail")] string contactAndOtp)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";
            var splitContactAndOtp = contactAndOtp.Split(':');
            var contact = splitContactAndOtp[0];
            var Otp = splitContactAndOtp[1];

            var sw = new Stopwatch();
            sw.Start();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { contact },
                BccList = new[] { "maillog.travorama@gmail.com" },
                Subject = envPrefix + " Forgot Password OTP "  ,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
            };
            Console.WriteLine("Sending Flight Eticket Email...");
            mailService.SendEmailWithTableTemplate(Otp, mailModel, "ForgotPasswordByOtpEmail");
            //FlightService.GetInstance().UpdateIssueProgress(rsvNo, "Eticket Email Sent. Ticket Issuance Complete.");

            Console.WriteLine("Done Processing Flight Eticket Email for RsvNo " + contactAndOtp);
        }
    }
}
