using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Environment;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void ActivityAcceptanceEmail([QueueTrigger("activityacceptanceemail")] string rsvNo)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Activity Voucher And Invoice Email for RsvNo " + rsvNo + "...");
            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var activity = ActivityService.GetInstance();
            var summary = activity.GetReservationForDisplay(rsvNo);
            sw.Stop();
            Console.WriteLine("Done Getting Required Files and Data from Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { summary.Contact.Email },
                BccList = new[] { "maillog.travorama@gmail.com" },
                Subject = envPrefix + "[Travorama] Aktivitas Diterima: No. Pesanan " + summary.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"           
            };
            mailService.SendEmailWithTableTemplate(summary, mailModel, "ActivityAcceptanceEmail");
            //FlightService.GetInstance().UpdateIssueProgress(rsvNo, "Eticket Email Sent. Ticket Issuance Complete.");

            Console.WriteLine("Done Processing Flight Eticket Email for RsvNo " + rsvNo);
        }
    }
}
