using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightDepositWarningNotifEmail([QueueTrigger("flightdepositwarningnotifemail")] string message)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var flightService = FlightService.GetInstance();
            var sw = new Stopwatch();
            var splitMessage = message.Split('^');
            decimal localPrice = 0;
            decimal currentDeposit = 0;
            var supplier = splitMessage[0];
            if (string.IsNullOrEmpty(splitMessage[1]))
            {
                localPrice = decimal.Parse(splitMessage[1]);
            }
            if (string.IsNullOrEmpty(splitMessage[2]))
            {
                currentDeposit = decimal.Parse(splitMessage[2]);
            }

            Console.WriteLine("Processing Flight Deposit Warning Notif Email ...");

            sw.Reset();

            var depostiWarningData = new FlightDepositWarning
            {
                Suppplier = splitMessage[0],
                CurrentDeposit = currentDeposit,
                BookingPrice = localPrice
            };

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { "deposit.warning@travelmadezy.com" },
                Subject = envPrefix + env == "production" ? "Deposit Warning :  " + supplier : "[TEST] Deposit Warning" + supplier,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };
            Console.WriteLine("Sending Notification Email...");
            mailService.SendEmail(depostiWarningData, mailModel, "FlightDepositWarningNotifEmail");

            Console.WriteLine("Done Processing Flight Deposit Warning Notif Email");
        }
    }
}
