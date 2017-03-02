using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void FlightChangedEticketEmail([QueueTrigger("flightchangedeticketemail")] string rsvNo)
        {
            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Changed Eticket Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var blobService = BlobStorageService.GetInstance();
            var file = blobService.GetByteArrayByFileInContainer(rsvNo + ".pdf", "Eticket");
            var summaryBytes = blobService.GetByteArrayByFileInContainer(rsvNo, "Reservation");
            var summaryJson = Encoding.UTF8.GetString(summaryBytes);
            var summary = JsonConvert.DeserializeObject<FlightReservationForDisplay>(summaryJson);
            sw.Stop();
            Console.WriteLine("Done Getting Required Files and Data from Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {summary.Contact.Email},
                BccList = new[] {"maillog.travorama@gmail.com"},
                Subject = "[Travorama.com] Jadwal Penerbangan Anda Berubah!!!",
                FromMail = "jangan-reply-ke-sini@travorama.com",
                FromName = "Travorama.com",
                ListFileInfo = new List<FileInfo>
                {
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "E-ticket Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
                        FileData = file
                    }
                }
            };
            Console.WriteLine("Sending Eticket Email...");
            mailService.SendEmail(summary, mailModel, "FlightChangedEticketEmail");

            Console.WriteLine("Deleting Data in Storage...");
            sw.Start();
            blobService.DeleteBlob(rsvNo, "Eticket");
            blobService.DeleteBlob(rsvNo, "Reservation");
            sw.Stop();
            Console.WriteLine("Done Deleting Data in Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();
            
            Console.WriteLine("Done Processing Flight Changed Eticket Email for RsvNo " + rsvNo);
        }
    }
}
