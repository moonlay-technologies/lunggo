using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace Lunggo.WebJob.EmailQueueHandler.Function
{
    public partial class ProcessEmailQueue
    {
        public static void HotelVoucherEmail([QueueTrigger("hotelvoucheremail")] string rsvNo)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Hotel Voucher Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var blobService = BlobStorageService.GetInstance();
            var eticketFile = blobService.GetByteArrayByFileInContainer(rsvNo + ".pdf", "Voucher");
            var invoiceFile = blobService.GetByteArrayByFileInContainer(rsvNo + ".pdf", "Invoice");
            var summaryBytes = blobService.GetByteArrayByFileInContainer(rsvNo, "Reservation");
            var summaryJson = Encoding.UTF8.GetString(summaryBytes);
            var summary = JsonConvert.DeserializeObject<HotelReservationForDisplay>(summaryJson);
            var hotelCd = summary.HotelDetail.HotelCode;
            var images = HotelService.GetInstance().GetHotelDetailFromDb(hotelCd).ImageUrl;
            var firstOrDefault = images.Where(i => i.Type == "GEN").ToList().FirstOrDefault();
            if (firstOrDefault != null)
                summary.HotelDetail.MainImage = images == null
                    ? null
                    : "http://photos.hotelbeds.com/giata/" + firstOrDefault.Path;
            sw.Stop();
            Console.WriteLine("Done Getting Required Files and Data from Storage. (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            var mailService = MailService.GetInstance();
            var mailModel = new MailModel
            {
                RecipientList = new[] {summary.Contact.Email},
                Subject = envPrefix + "[Travorama] Voucher Anda - No. Pemesanan " + summary.RsvNo,
                FromMail = "booking@travorama.com",
                FromName = "Travorama",
                ListFileInfo = new List<FileInfo>
                {
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "Voucher Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
                        FileData = eticketFile
                    },
                    new FileInfo
                    {
                        ContentType = "PDF",
                        FileName = "Invoice Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
                        FileData = invoiceFile
                    }
                }
            };
            Console.WriteLine("Sending Hotel Voucher Email...");
            mailService.SendEmail(summary, mailModel, "HotelVoucherEmail");
            //HotelService.GetInstance().UpdateIssueProgress(rsvNo, "Eticket Email Sent. Ticket Issuance Complete.");
            
            Console.WriteLine("Done Processing Hotel Voucher Email for RsvNo " + rsvNo);
        }
    }
}
