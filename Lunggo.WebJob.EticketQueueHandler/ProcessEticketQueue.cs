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
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.WebJob.EticketQueueHandler
{
    public class ProcessEticketQueue
    {

        public static void ProcessQueue([QueueTrigger("eticketqueue")] string rsvNo)
        {
            Console.WriteLine("Processing Eticket for RsvNo " + rsvNo + "...");
            if (rsvNo.First() == 'F')
            {
                var sw = new Stopwatch();
                var flightService = FlightService.GetInstance();
                var templateService = HtmlTemplateService.GetInstance();
                var converter = new EvoPdf.HtmlToPdfConverter();
                var reservation = flightService.GetDetails(rsvNo);

                Console.WriteLine("Parsing Eticket Template...");
                sw.Start();
                var eticketTemplate = templateService.GenerateTemplate(reservation, HtmlTemplateType.FlightEticket);
                sw.Stop();
                sw.Reset();
                Console.WriteLine("Done Parsing Eticket Template. (" + sw.Elapsed + "s)");

                Console.WriteLine("Generating Eticket File...");
                sw.Start();
                var fileContent = converter.ConvertHtml(eticketTemplate, null);
                sw.Stop();
                sw.Reset();
                Console.WriteLine("Done Generating Eticket File. (" + sw.Elapsed + "s)");

                var blobService = BlobStorageService.GetInstance();

                Console.WriteLine("Saving Eticket File...");
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
                sw.Reset();
                Console.WriteLine("Done Saving Eticket File. (" + sw.Elapsed + "s)");

                Console.WriteLine("Saving Flight Reservation Data...");
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
                sw.Reset();
                Console.WriteLine("Done Saving Flight Reservation Data. (" + sw.Elapsed + "s)");

                Console.WriteLine("Pushing Eticket Email Queue...");
                var queueService = QueueService.GetInstance();
                var queue = queueService.GetQueueByReference(QueueService.Queue.EticketEmail);
                queue.AddMessage(new CloudQueueMessage(rsvNo));
                Console.WriteLine("Done Processing Eticket for RsvNo " + rsvNo);
            }
        }
    }
}
