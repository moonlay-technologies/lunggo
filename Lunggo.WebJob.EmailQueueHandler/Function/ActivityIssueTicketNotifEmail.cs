using System;
using System.Diagnostics;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void ActivityIssueTicketNotifEmail([QueueTrigger("activityissueticketnotifemail")] string rsvNo)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var activityService = ActivityService.GetInstance();
            var sw = new Stopwatch();
            Console.WriteLine("Processing Activity Issue Ticket Notif Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = activityService.GetReservationForDisplay(rsvNo);
            sw.Stop();
            var x = !string.IsNullOrEmpty(reservation.DateTime.Session) ? reservation.DateTime.Session : "N/A" ;
            //Console.WriteLine("session :" + reservation.DateTime.Session);
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "marjan.maulataufik@travelmadezy.com" },
                Subject = envPrefix + "[Travorama] Activity Issue Ticket - RsvNo : " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmailWithTableTemplate(reservation, mailModel, "ActivityIssueTicketNotifEmail");

            Console.WriteLine("Done Processing Activity Issue Ticket Notif Email for RsvNo " + rsvNo);
        }
    }
}
