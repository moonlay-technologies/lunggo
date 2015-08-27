using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightEticketEmail([QueueTrigger("flighteticketemail")] string rsvNo)
        {
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Eticket Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var blobService = BlobStorageService.GetInstance();
            var eticketFile = blobService.GetByteArrayByFileInContainer(rsvNo, BlobContainer.Eticket);
            var invoiceFile = blobService.GetByteArrayByFileInContainer(rsvNo, BlobContainer.Invoice);
            var summaryBytes = blobService.GetByteArrayByFileInContainer(rsvNo, BlobContainer.Reservation);
            var summaryJson = Encoding.UTF8.GetString(summaryBytes);
            var summary = JsonConvert.DeserializeObject<FlightReservationApi>(summaryJson);
            sw.Stop();
            Console.WriteLine("Done Getting Required Files and Data from Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {summary.Contact.Email},
                Subject = "[Travorama] E-tiket Anda - No. Pemesanan " + summary.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
                ListFileInfo = new List<FileInfo>
                {
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "E-tiket Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
                        FileData = eticketFile
                    },
                    /*
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "Invoice Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
                        FileData = invoiceFile
                    }
                    */
                }
            };
            Console.WriteLine("Sending Flight Eticket Email...");
            mailService.SendEmail(summary, mailModel, HtmlTemplateType.FlightEticketEmail);

            Console.WriteLine("Deleting Data in Storage...");
            sw.Start();
            blobService.DeleteBlob(rsvNo, BlobContainer.Eticket);
            blobService.DeleteBlob(rsvNo, BlobContainer.Invoice);
            blobService.DeleteBlob(rsvNo, BlobContainer.Reservation);
            sw.Stop();
            Console.WriteLine("Done Deleting Data in Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();
            
            Console.WriteLine("Done Processing Flight Eticket Email for RsvNo " + rsvNo);
        }
    }
}
