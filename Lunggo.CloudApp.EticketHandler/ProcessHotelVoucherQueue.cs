using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SelectPdf;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.CloudApp.EticketHandler
{
    public class ProcessHotelVoucherQueue
    {

        public static async Task ProcessQueue()
        {
            var queue = QueueService.GetInstance().GetQueueByReference("HotelVoucher");
            Trace.WriteLine("Checking Voucher Queue...");
            var message = await queue.GetMessageAsync();
            if (message == null)
            {
                Trace.WriteLine("No Waiting Voucher Message.");
                return;
            }
            var rsvNo = message.AsString;
            var hotel = HotelService.GetInstance();
            Trace.WriteLine("Processing Voucher for RsvNo " + rsvNo + "...");
            var sw = new Stopwatch();
            var hotelService = HotelService.GetInstance();
            var templateService = HtmlTemplateService.GetInstance();
            var blobService = BlobStorageService.GetInstance();
            var converter = new SelectPdf.HtmlToPdf();
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
            var reservation = hotelService.GetReservationForDisplay(rsvNo);
            var hotelCode = reservation.HotelDetail.HotelCode;
            var hotelDetail = HotelService.GetInstance().GetHotelDetailFromDb(hotelCode);
            reservation.HotelDetail.Latitude = hotelDetail.Latitude;
            reservation.HotelDetail.Longitude = hotelDetail.Longitude;
            reservation.HotelDetail.Facilities = hotelDetail.Facilities == null ? null : new HotelFacilityForDisplay {Other = hotelDetail.Facilities
                    .Where(x => x.MustDisplay == true )
                    .Select(x => (hotel.GetHotelFacilityDescId
                        (Convert.ToInt32(x.FacilityGroupCode) * 1000 + Convert.ToInt32(x.FacilityCode)))).ToList()};
            reservation.HotelDetail.IsWifiAccessAvailable = hotelDetail.Facilities != null &&
                                                            ((hotelDetail.Facilities != null ||
                                                              hotelDetail.Facilities.Count != 0) &&
                                                             hotelDetail.Facilities.Any(
                                                                 f =>
                                                                     (f.FacilityGroupCode == 60 && f.FacilityCode == 261)
                                                                     ||
                                                                     (f.FacilityGroupCode == 70 && f.FacilityCode == 550)));
            reservation.HotelDetail.IsRestaurantAvailable = hotelDetail.Facilities != null &&
                                                            ((hotelDetail.Facilities != null || hotelDetail.Facilities.Count != 0) &&
                                                             hotelDetail.Facilities.Any(
                                                                 f =>
                                                                     (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                                                                     ||
                                                                     (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                                                                     ||
                                                                     (f.FacilityGroupCode == 75 && f.FacilityCode == 845)));
            var images = hotelDetail.ImageUrl;
            var firstOrDefault = images.Where(i => i.Type == "GEN").ToList().FirstOrDefault();
            if (firstOrDefault != null)
                reservation.HotelDetail.MainImage = images == null
                    ? null
                    : "http://photos.hotelbeds.com/giata/bigger/" + firstOrDefault.Path;

            reservation.HotelDetail.MapImage =
                "https://maps.googleapis.com/maps/api/staticmap?center="+ reservation.HotelDetail.Latitude +",+"+ reservation.HotelDetail.Longitude+ "&zoom=16&scale=false&size=640x180&maptype=roadmap&key=AIzaSyCRAmMz6GPXsXi1pZAl5QUsjNTcY0ZfqVA&visual_refresh=true";
            Trace.WriteLine("Parsing Voucher Template for RsvNo " + rsvNo + "...");
            sw.Start();
            var voucherTemplate = templateService.GenerateTemplate(reservation, "HotelVoucher");
            sw.Stop();
            Trace.WriteLine("Done Parsing Voucher Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Generating Voucher File for RsvNo " + rsvNo + "...");
            sw.Start();
            var voucherFile = converter.ConvertHtmlString(voucherTemplate).Save();
            sw.Stop();
            Trace.WriteLine("Done Generating Voucher File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Saving Voucher File for RsvNo " + rsvNo + "...");
            sw.Start();
            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new FileInfo
                    {
                        FileName = rsvNo + ".pdf",
                        ContentType = "application/pdf",
                        FileData = voucherFile
                    },
                    Container = "Voucher"
                },
                SaveMethod = SaveMethod.Force
            });
            sw.Stop();
            Trace.WriteLine("Done Saving Voucher File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();
            
            Trace.WriteLine("Parsing Invoice for RsvNo " + rsvNo + "...");
            sw.Start();
            var invoiceTemplate = templateService.GenerateTemplate(reservation, "HotelInvoice");
            sw.Stop();
            Trace.WriteLine("Done Parsing Invoice Template for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Generating Invoice File for RsvNo " + rsvNo + "...");
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
                        FileName = rsvNo + ".pdf",
                        ContentType = "application/pdf",
                        FileData = invoiceFile
                    },
                    Container = "Invoice"
                },
                SaveMethod = SaveMethod.Force
            });
            sw.Stop();
            Trace.WriteLine("Done Saving Invoice File for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Saving Hotel Reservation Data for RsvNo " + rsvNo + "...");
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
            Trace.WriteLine("Done Saving Hotel Reservation Data for RsvNo " + rsvNo + ". (" + sw.Elapsed.TotalSeconds + "s)");
            sw.Reset();

            Trace.WriteLine("Pushing Voucher Email Queue for RsvNo " + rsvNo + "...");
            var queueService = QueueService.GetInstance();
            var emailQueue = queueService.GetQueueByReference("HotelVoucherEmail");
            emailQueue.AddMessage(new CloudQueueMessage(rsvNo));

            Trace.WriteLine("Pushing Hotel Voucher Notification for RsvNo " + rsvNo + "...");
            //HotelService.PushVoucherIssuedNotif(rsvNo);

            Trace.WriteLine("Done Processing Voucher for RsvNo " + rsvNo);

            await queue.DeleteMessageAsync(message);
        }
    }
}
