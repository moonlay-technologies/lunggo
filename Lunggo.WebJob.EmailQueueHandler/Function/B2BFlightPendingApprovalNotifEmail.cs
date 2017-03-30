using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Util;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void B2BFlightPendingApprovalNotifEmail([QueueTrigger("b2bflightpendingapprovalnotifemail")] string message)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";
            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            var splitMessage = message.Split(',');
            var rsvNo = splitMessage[0];
            var recipient = new string[splitMessage.Length - 1];
            var counter = 0;
            for (int i = 1; i < splitMessage.Length; i++)
            {
                recipient[counter] = splitMessage[1];
                counter++;
            }
            Console.WriteLine("Processing B2B FlightPendingApprovalNotifEmail for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var reservation = flightService.GetReservationForDisplay(rsvNo);
            var mailData = new FlightBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo(rsvNo),
                Reservation = reservation
            };
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();
            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = recipient,
                Subject = envPrefix + "[Travorama] Persetujuan Anda Dibutuhkan - No. Pemesanan " + reservation.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
                BccList = new[] { "maillog.travorama@gmail.com" }
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(mailData, mailModel, "B2BFlightPendingApprovalNotifEmail");

            Console.WriteLine("Done Processing B2B Flight Pending Approval Notif Email for RsvNo " + rsvNo);
        }
    }
}
