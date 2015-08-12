using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Worker.EticketHandler
{
    public class ProcessEticketQueue
    {

        public static async Task ProcessQueue()
        {
            var queue = QueueService.GetInstance().GetQueueByReference(Queue.FlightEticket);
            Trace.WriteLine("Checking Eticket Queue...");
            var message = await queue.GetMessageAsync();
            if (message == null)
            {
                Trace.WriteLine("No Waiting Eticket Message.");
                return;
            }
            var rsvNo = message.AsString;

            Trace.WriteLine("Processing Eticket for RsvNo " + rsvNo + "...");
            if (rsvNo.First() == 'F')
            {
                var sw = new Stopwatch();
                var flightService = FlightService.GetInstance();
                var templateService = HtmlTemplateService.GetInstance();
                var blobService = BlobStorageService.GetInstance();
                var converter = new NReco.PdfGenerator.HtmlToPdfConverter();
                var reservation = flightService.GetDetails(rsvNo);

                Trace.WriteLine("Parsing Eticket Template for RsvNo " + rsvNo + "...");
                sw.Start();
                var eticketTemplate = templateService.GenerateTemplate(reservation, HtmlTemplateType.FlightEticket);
                sw.Stop();
                Trace.WriteLine("Done Parsing Eticket Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Generating Eticket File for RsvNo " + rsvNo + "...");
                sw.Start();
                var eticketFile = converter.GeneratePdf(eticketTemplate);
                sw.Stop();
                Trace.WriteLine("Done Generating Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Saving Eticket File for RsvNo " + rsvNo + "...");
                sw.Start();
                blobService.WriteFileToBlob(new BlobWriteDto
                {
                    FileBlobModel = new FileBlobModel
                    {
                        FileInfo = new FileInfo
                        {
                            FileName = rsvNo,
                            ContentType = "PDF",
                            FileData = eticketFile
                        },
                        Container = BlobContainer.Eticket
                    },
                    SaveMethod = SaveMethod.Force
                });
                sw.Stop();
                Trace.WriteLine("Done Saving Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Parsing Invoice for RsvNo " + rsvNo + "...");
                sw.Start();
                var invoiceTemplate = templateService.GenerateTemplate(reservation, HtmlTemplateType.FlightInvoice);
                sw.Stop();
                Trace.WriteLine("Done Parsing Invoice Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Generating Invoice File for RsvNo " + rsvNo + "...");
                sw.Start();
                var invoiceFile = converter.GeneratePdf(invoiceTemplate);
                sw.Stop();
                Trace.WriteLine("Done Generating Invoice File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Saving Invoice File for RsvNo " + rsvNo + "...");
                sw.Start();
                blobService.WriteFileToBlob(new BlobWriteDto
                {
                    FileBlobModel = new FileBlobModel
                    {
                        FileInfo = new FileInfo
                        {
                            FileName = rsvNo,
                            ContentType = "PDF",
                            FileData = invoiceFile
                        },
                        Container = BlobContainer.Invoice
                    },
                    SaveMethod = SaveMethod.Force
                });
                sw.Stop();
                Trace.WriteLine("Done Saving Invoice File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Saving Flight Reservation Data for RsvNo " + rsvNo + "...");
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
                Trace.WriteLine("Done Saving Flight Reservation Data for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Pushing Eticket Email Queue for RsvNo " + rsvNo + "...");
                var queueService = QueueService.GetInstance();
                var emailQueue = queueService.GetQueueByReference(Queue.FlightEticketEmail);
                emailQueue.AddMessage(new CloudQueueMessage(rsvNo));
                Trace.WriteLine("Done Processing Eticket for RsvNo " + rsvNo);
            }

            await queue.DeleteMessageAsync(message);
        }
    }
}
