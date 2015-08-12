using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.WebJob.EticketQueueHandler
{
    public class ProcessEticketQueue
    {

        public static void ProcessQueue([QueueTrigger("flighteticket")] string rsvNo)
        {
            Console.WriteLine("Processing Eticket for RsvNo " + rsvNo + "...");
            if (rsvNo.First() == 'F')
            {
                var sw = new Stopwatch();
                var swTotal = new Stopwatch();
                var flightService = FlightService.GetInstance();
                var templateService = HtmlTemplateService.GetInstance();
                var converter = new SelectPdf.HtmlToPdf();
                var baseUrl = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");

                Console.WriteLine("Getting Reservation Details for RsvNo " + rsvNo + "...");
                swTotal.Start();
                sw.Start();
                var reservation = flightService.GetDetails(rsvNo);
                sw.Stop();
                Console.WriteLine("Done Getting Reservation Details for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Console.WriteLine("Parsing Eticket Template for RsvNo " + rsvNo + "...");
                sw.Start();
                var eticketTemplate = templateService.GenerateTemplate(reservation, HtmlTemplateType.FlightEticket);
                sw.Stop();
                Console.WriteLine("Done Parsing Eticket Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Console.WriteLine("Generating Eticket File for RsvNo " + rsvNo + "...");
                sw.Start();
                var fileContent = converter.ConvertHtmlString(eticketTemplate, baseUrl).Save();
                sw.Stop();
                Console.WriteLine("Done Generating Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                var blobService = BlobStorageService.GetInstance();

                Console.WriteLine("Saving Eticket File for RsvNo " + rsvNo + "...");
                sw.Start();
                blobService.WriteFileToBlob(new BlobWriteDto
                {
                    FileBlobModel = new FileBlobModel
                    {
                        FileInfo = new FileInfo
                        {
                            FileName = rsvNo,
                            ContentType = "PDF",
                            FileData = fileContent
                        },
                        Container = BlobContainer.Eticket
                    },
                    SaveMethod = SaveMethod.Force
                });
                sw.Stop();
                Console.WriteLine("Done Saving Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Console.WriteLine("Saving Flight Reservation Data for RsvNo " + rsvNo + "...");
                sw.Start();
                var reservationJson = JsonConvert.SerializeObject(reservation);
                var reservationContent = Encoding.UTF8.GetBytes(reservationJson);
                blobService.WriteFileToBlob(new BlobWriteDto
                {
                    FileBlobModel = new FileBlobModel
                    {
                        FileInfo = new FileInfo
                        {
                            FileName = rsvNo,
                            ContentType = "JSON",
                            FileData = reservationContent
                        },
                        Container = BlobContainer.Reservation
                    },
                    SaveMethod = SaveMethod.Force
                });
                sw.Stop();
                Console.WriteLine("Done Saving Flight Reservation Data for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Console.WriteLine("Pushing Eticket Email Queue for RsvNo " + rsvNo + "...");
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference(Queue.FlightEticketEmail);
                queue.AddMessage(new CloudQueueMessage(rsvNo));
                swTotal.Stop();
                Console.WriteLine("Done Processing Eticket for RsvNo " + rsvNo + ". (" + swTotal.Elapsed.TotalSeconds + "s Total)");
            }
        }
    }
}
