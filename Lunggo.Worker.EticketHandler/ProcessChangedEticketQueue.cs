using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NReco.PdfGenerator;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Worker.EticketHandler
{
    public class ProcessChangedEticketQueue
    {

        public static async Task ProcessQueue()
        {
            var queue = QueueService.GetInstance().GetQueueByReference("FlightChangedEticket");
            Trace.WriteLine("Checking Changed Eticket Queue...");
            var message = await queue.GetMessageAsync();
            if (message == null)
            {
                Trace.WriteLine("No Waiting Changed Eticket Message.");
                return;
            }
            var rsvNo = message.AsString;

            Trace.WriteLine("Processing Changed Eticket for RsvNo " + rsvNo + "...");
            if (rsvNo.IsFlightRsvNo())
            {
                var sw = new Stopwatch();
                var flightService = FlightService.GetInstance();
                var templateService = HtmlTemplateService.GetInstance();
                var converter = new NReco.PdfGenerator.HtmlToPdfConverter();
                var reservation = flightService.GetDetails(rsvNo);

                Trace.WriteLine("Parsing Eticket Template for RsvNo " + rsvNo + "...");
                sw.Start();
                var eticketTemplate = templateService.GenerateTemplate(reservation, "FlightEticket");
                sw.Stop();
                Trace.WriteLine("Done Parsing Eticket Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Generating Eticket File for RsvNo " + rsvNo + "...");
                sw.Start();
                converter.Margins = new PageMargins
                {
                    Top = 0,
                    Bottom = 0,
                    Left = 0,
                    Right = 0
                };
                var fileContent = converter.GeneratePdf(eticketTemplate);
                sw.Stop();
                Trace.WriteLine("Done Generating Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                var blobService = BlobStorageService.GetInstance();

                Trace.WriteLine("Saving Eticket File for RsvNo " + rsvNo + "...");
                sw.Start();
                blobService.WriteFileToBlob(new BlobWriteDto
                {
                    FileBlobModel = new FileBlobModel
                    {
                        FileInfo = new FileInfo
                        {
                            FileName = rsvNo + ".pdf",
                            ContentType = "application/pdf",
                            FileData = fileContent
                        },
                        Container = "Eticket"
                    },
                    SaveMethod = SaveMethod.Force
                });
                sw.Stop();
                Trace.WriteLine("Done Saving Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
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
                            ContentType = "application/json",
                            FileData = reservationContent
                        },
                        Container = "Reservation"
                    },
                    SaveMethod = SaveMethod.Force
                });
                sw.Stop();
                Trace.WriteLine("Done Saving Flight Reservation Data for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
                sw.Reset();

                Trace.WriteLine("Pushing Changed Eticket Email Queue for RsvNo " + rsvNo + "...");
                var queueService = QueueService.GetInstance();
                var emailQueue = queueService.GetQueueByReference("FlightChangedEticketEmail");
                emailQueue.AddMessage(new CloudQueueMessage(rsvNo));
                Trace.WriteLine("Done Processing Changed Eticket for RsvNo " + rsvNo);
            }

            await queue.DeleteMessageAsync(message);
        }
    }
}
