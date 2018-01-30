using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
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
        public static void ActivityVoucherAndInvoiceEmail([QueueTrigger("activityevoucherandinvoiceemail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Activity Voucher And Invoice Email for RsvNo " + rsvNo + "...");
            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var blobService = BlobStorageService.GetInstance();
            var eVoucherFile = blobService.GetByteArrayByFileInContainer(rsvNo + ".pdf", "Eticket");
            var activity = ActivityService.GetInstance();
            var summary = activity.GetReservationForDisplay(rsvNo);
            var cartId = PaymentService.GetInstance().GetCartIdByRsvNo(rsvNo);
            var invoiceFile = blobService.GetByteArrayByFileInContainer(cartId + ".pdf", "Invoice");
            //var summaryBytes = blobService.GetByteArrayByFileInContainer(rsvNo, "Reservation");
            //var summaryJson = Encoding.UTF8.GetString(summaryBytes);
            //var summary = JsonConvert.DeserializeObject<FlightReservationForDisplay>(summaryJson);
            sw.Stop();
            Console.WriteLine("Done Getting Required Files and Data from Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] { summary.Contact.Email },
                BccList = new[] { "maillog.travorama@gmail.com" },
                Subject = envPrefix + "[Travorama] E-tiket Anda - No. Pemesanan " + summary.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
                ListFileInfo = new List<FileInfo>
                {
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "E-Voucher Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
                        FileData = eVoucherFile
                    },
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "Invoice Anda - No. Pemesanan " + cartId + ".pdf",
                        FileData = invoiceFile
                    }
                }
            };
            Console.WriteLine("Sending Flight Eticket Email...");
            mailService.SendEmailWithTableTemplate(summary, mailModel, "ActivityEVoucherAndInvoiceEmail");
            //FlightService.GetInstance().UpdateIssueProgress(rsvNo, "Eticket Email Sent. Ticket Issuance Complete.");

            Console.WriteLine("Done Processing Flight Eticket Email for RsvNo " + rsvNo);
        }
    }
}
