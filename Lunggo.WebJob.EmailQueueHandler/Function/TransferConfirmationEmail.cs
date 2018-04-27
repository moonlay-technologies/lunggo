using System;
using System.Diagnostics;
using System.Linq;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
    //    public static void TransferConfirmationEmail([QueueTrigger("transferconfirmationemail")] string rsvNo)
    //    {
    //        var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
    //        var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

    //        var sw = new Stopwatch();
    //        Console.WriteLine("Processing Transfer Confirmation Email for RsvNo " + rsvNo + "...");

    //        Console.WriteLine("Getting Required Data...");
    //        sw.Start();
    //        var report =
    //            new PaymentService().GetUncheckedTransferConfirmationReports().SingleOrDefault(r => r.RsvNo == rsvNo);
    //        sw.Stop();
    //        Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
    //        sw.Reset();

    //        if (report == null)
    //        {
    //            Console.WriteLine("Reservation already expired; report already purged...");
    //            Console.WriteLine("Done Processing Transfer Confirmation Email for RsvNo " + rsvNo);
    //            return;
    //        }


    //        var mailService = MailService.GetInstance();
    //        var mailModel = new MailModel
    //        {
    //            RecipientList = new[] { "rama.adhitia@travelmadezy.com" },
    //            Subject = envPrefix + env == "production" ? "Konfirmasi Transfer No. " + rsvNo : "[TEST] Ignore This Email",
    //            FromMail = "booking@travorama.com",
    //            FromName = "TRAVORAMA"
    //        };
    //        Console.WriteLine("Sending Notification Email...");
    //        mailService.SendEmail(report, mailModel, "TransferConfirmationEmail");

    //        Console.WriteLine("Done Processing Transfer Confirmation Email for RsvNo " + rsvNo);
    //    }
    }
}
