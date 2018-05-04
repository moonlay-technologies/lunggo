using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Environment;
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
            var env = EnvVariables.Get("general", "environment");
            var envPrefix = env != "production" ? "[" + env.ToUpper() + "] " : "";

            var sw = new Stopwatch();
            Console.WriteLine("Processing Flight Eticket Email for RsvNo " + rsvNo + "...");

            Console.WriteLine("Getting Required Files and Data from Storage...");
            sw.Start();
            var blobService = BlobStorageService.GetInstance();
            var eticketFile = blobService.GetByteArrayByFileInContainer(rsvNo + ".pdf", "Eticket");
            var flight = FlightService.GetInstance();
            var summary = flight.GetReservationForDisplay(rsvNo);
            var invoiceFile = blobService.GetByteArrayByFileInContainer(rsvNo + ".pdf", "Invoice");
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
                        FileName = "E-ticket Anda - No. Pemesanan " + summary.RsvNo + ".pdf",
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
            Console.WriteLine("Sending Flight Eticket Email...");
            mailService.SendEmailWithTableTemplate(summary, mailModel, "FlightEticketEmail");
            //FlightService.GetInstance().UpdateIssueProgress(rsvNo, "Eticket Email Sent. Ticket Issuance Complete.");
            
            Console.WriteLine("Done Processing Flight Eticket Email for RsvNo " + rsvNo);
        }
    }
}
