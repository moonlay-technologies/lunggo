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
using SelectPdf;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Model;

namespace Lunggo.Worker.EticketHandler
{
    public class ProcessActivityVoucherQueue
    {
        public static async Task ProcessQueue()
        {
            var queue = QueueService.GetInstance().GetQueueByReference("ActivityEVoucherAndInvoice");
            Trace.WriteLine("Checking Voucher Queue...");
            var message = await queue.GetMessageAsync();
            if (message == null)
            {
                Trace.WriteLine("No Waiting Voucher Message.");
                return;
            }
            var rsvNo = message.AsString;
            Trace.WriteLine("Processing Voucher for RsvNo " + rsvNo + "...");

            var sw = new Stopwatch();
            var activityService = ActivityService.GetInstance();
            var templateService = HtmlTemplateService.GetInstance();
            var blobService = BlobStorageService.GetInstance();
            var converter = new SelectPdf.HtmlToPdf();
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
            var reservation = activityService.GetReservationForDisplay(rsvNo);
            var bookingDetail = activityService.GetMyBookingDetailFromDb(new GetMyBookingDetailInput { RsvNo = rsvNo });
            var activityEVoucher = new ActivityEVoucher();
            activityEVoucher.BookingDetail = bookingDetail.BookingDetail;
            activityEVoucher.ActivityReservation = reservation;

            Trace.WriteLine("Parsing EVoucher Template for RsvNo " + rsvNo + "...");
            sw.Start();
            var eVoucherTemplate = templateService.GenerateTemplateFromTable(activityEVoucher, "ActivityEVoucher");
            sw.Stop();
            Trace.WriteLine("Done Parsing EVoucher Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Generating EVoucher File for RsvNo " + rsvNo + "...");
            sw.Start();
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            var eVoucherFile = converter.ConvertHtmlString(eVoucherTemplate).Save();
            sw.Stop();
            Trace.WriteLine("Done Generating Voucher File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Saving Eticket File for RsvNo " + rsvNo + "...");
            sw.Start();
            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new Framework.SharedModel.FileInfo
                    {
                        FileName = rsvNo + ".pdf",
                        ContentType = "application/pdf",
                        FileData = eVoucherFile
                    },
                    Container = "Eticket"
                },
                SaveMethod = SaveMethod.Force
            });
            sw.Stop();
            Trace.WriteLine("Done Saving Eticket File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            ActivityService.GetInstance().UpdateRsvNoPdfFlag(rsvNo);
            var cartId = new PaymentService().GetCartIdByRsvNo(rsvNo);
            var activityReservation = activityService.GetActivityInvoice(cartId);
            Trace.WriteLine("Parsing Invoice for CartId " + cartId + "...");
            sw.Start();
            var invoiceTemplate = templateService.GenerateTemplateFromTable(activityReservation, "ActivityInvoice");
            sw.Stop();
            Trace.WriteLine("Done Parsing Invoice Template for RsvNo " + cartId + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Generating Invoice File for RsvNo " + cartId + "...");
            sw.Start();/*
                converter.Margins = new PageMargins
                {
                    Top = 0,
                    Bottom = 0,
                    Left = 0,
                    Right = 0
                };*/
            var invoiceFile = converter.ConvertHtmlString(invoiceTemplate).Save();
            sw.Stop();
            Trace.WriteLine("Done Generating Invoice File for RsvNo " + cartId + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Saving Invoice File for RsvNo " + cartId + "...");
            sw.Start();
            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new FileInfo
                    {
                        FileName = cartId + ".pdf",
                        ContentType = "application/pdf",
                        FileData = invoiceFile
                    },
                    Container = "Invoice"
                },
                SaveMethod = SaveMethod.Force
            });
            sw.Stop();
            Trace.WriteLine("Done Saving Invoice File for RsvNo " + cartId + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Saving Flight Reservation Data for RsvNo " + cartId + "...");
            sw.Start();
            var activityReservationJson = JsonConvert.SerializeObject(activityReservation);
            var activityReservationContent = Encoding.UTF8.GetBytes(activityReservationJson);
            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new FileInfo
                    {
                        FileName = cartId,
                        ContentType = "application/json",
                        FileData = activityReservationContent
                    },
                    Container = "AcivityCart"
                },
                SaveMethod = SaveMethod.Force
            });
            sw.Stop();
            Trace.WriteLine("Done Saving Activity Data for CartId " + cartId + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Pushing Eticket Email Queue for CartId " + cartId + "...");
            
            var queueService = QueueService.GetInstance();
            var emailQueue = queueService.GetQueueByReference("ActivityEVoucherAndInvoiceEmail");
            emailQueue.AddMessage(new CloudQueueMessage(rsvNo));

            Trace.WriteLine("Pushing Eticket Eticket Notification for CartId " + rsvNo + "...");
            //flightService.PushEticketIssuedNotif(rsvNo);

            Trace.WriteLine("Done Processing Eticket for CartId " + rsvNo);

            await queue.DeleteMessageAsync(message);
        }
    }
}
