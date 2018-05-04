using System;
using System.Diagnostics;
using System.Linq;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Mail;
using Microsoft.Azure.WebJobs;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void CartTransferInstructionEmail([QueueTrigger("carttransferinstructionemail")] string cartRecordId)
        {
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Transfer Instruction Email for CartRecordId " + cartRecordId + "...");

            Console.WriteLine("Getting Required Data...");
            sw.Start();
            var paymentService = new PaymentService();
            var cart = paymentService.GetCart(cartRecordId);
            sw.Stop();
            Console.WriteLine("Done Getting Required Data. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var contact = Contact.GetFromDb(cart.RsvNoList[0]);

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { contact.Email },
                BccList = new[] { "maillog.travorama@gmail.com" },
                Subject =
                    envPrefix + "[Travorama] Harap Selesaikan Pembayaran Anda - No. Pesanan " + cartRecordId,
                FromMail = "booking@travorama.com",
                FromName = "Travorama"
            };

            var payment = paymentService.GetPaymentDetails(cartRecordId);
            var instruction = paymentService.GetInstruction(payment);
            var reservations = cart.RsvNoList.Select(ActivityService.GetInstance().GetReservation).ToList();
            Console.WriteLine("Sending Bank Transfer Instruction Email...");
            mailService.SendEmailWithTableTemplate(new { Payment = payment, contact, Instruction = instruction, reservations}, mailModel, "CartTransferInstructionEmail");

            Console.WriteLine("Done Processing Transfer Instruction Email for CartRecordId " + cartRecordId);

        }
    }
}
