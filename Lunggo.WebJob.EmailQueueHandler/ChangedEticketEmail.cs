using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using log4net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Core;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.Util;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.WebJob.EmailQueueHandler
{
    public partial class ProcessEmailQueue
    {
        public static void ChangedEticketEmail([QueueTrigger("changedeticketemail")] string rsvNo)
        {
            var sw = new Stopwatch();
            Console.WriteLine("Processing Changed Eticket Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var blobService = BlobStorageService.GetInstance();
            var file = blobService.GetByteArrayByFileInContainer(rsvNo, BlobContainer.Eticket);
            var summaryBytes = blobService.GetByteArrayByFileInContainer(rsvNo, BlobContainer.Reservation);
            var summaryJson = Encoding.UTF8.GetString(summaryBytes);
            var summary = JsonConvert.DeserializeObject<FlightReservation>(summaryJson);
            sw.Stop();
            Console.WriteLine("Done Getting Required Files and Data from Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {summary.ContactData.Email},
                Subject = "[Travorama.com] Jadwal Penerbangan Anda Berubah!!!",
                FromMail = "jangan-reply-ke-sini@travorama.com",
                FromName = "Travorama.com",
                ListFileInfo = new List<FileInfo>
                {
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "Eticket Baru Anda - No. Reservasi " + summary.RsvNo + ".pdf",
                        FileData = file
                    }
                }
            };
            Console.WriteLine("Sending Eticket Email...");
            mailService.SendEmail(summary, mailModel, HtmlTemplateType.FlightEticketEmail);

            Console.WriteLine("Deleting Data in Storage...");
            sw.Start();
            blobService.DeleteBlob(rsvNo, BlobContainer.Eticket);
            blobService.DeleteBlob(rsvNo, BlobContainer.Reservation);
            sw.Stop();
            Console.WriteLine("Done Deleting Data in Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();
            
            Console.WriteLine("Done Processing Changed Eticket Email for RsvNo " + rsvNo);
        }
    }
}
