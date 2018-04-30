using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.I18nMessage;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.TableStorage;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Generator
{
    class Tester
    {
        private static object sw;

        static void Main()
        {
            Init();
            var rsvNo = "36540985";
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

            var eVoucherTemplate = templateService.GenerateTemplateFromTable(activityEVoucher, "ActivityEVoucher");

            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
            var eVoucherFile = converter.ConvertHtmlString(eVoucherTemplate).Save();

            blobService.WriteFileToBlob(new BlobWriteDto
            {
                FileBlobModel = new FileBlobModel
                {
                    FileInfo = new Framework.SharedModel.FileInfo
                    {
                        FileName = rsvNo + ".html",
                        ContentType = "text/html",
                        FileData = Encoding.UTF8.GetBytes(eVoucherTemplate)
                    },
                    Container = "Eticket"
                },
                SaveMethod = SaveMethod.Force
            });
        }

        public static void Init()
        {
            InitDatabaseService();
            InitI18NMessageManager();
            InitQueueService();
            InitTableStorageService();
            InitHtmlTemplateService();
            InitRedisService();
            InitMailService();
            InitBlobStorageService();
        }
        
        private static void InitFlightService()
        {
            var flight = FlightService.GetInstance();
            flight.Init("Config/");
        }

        private static void InitHotelService()
        {
            var hotel = HotelService.GetInstance();
            hotel.Init("Config/");
        }

        private static void InitDatabaseService()
        {
            var connString = EnvVariables.Get("db", "connectionString");
            var db = DbService.GetInstance();
            db.Init(connString);
        }

        private static void InitI18NMessageManager()
        {
            var messageManager = MessageManager.GetInstance();
            messageManager.Init("Config/");
        }

        private static void InitQueueService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var queue = QueueService.GetInstance();
            queue.Init(connString);
        }

        private static void InitTableStorageService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var table = TableStorageService.GetInstance();
            table.Init(connString);
        }

        private static void InitMailService()
        {
            var apiKey = EnvVariables.Get("mandrill", "apiKey");
            var mailService = MailService.GetInstance();
            mailService.Init(apiKey);
        }

        public static void InitHtmlTemplateService()
        {
            var htmlTemplateService = HtmlTemplateService.GetInstance();
            htmlTemplateService.Init();
        }

        public static void InitBlobStorageService()
        {
            var connString = EnvVariables.Get("azureStorage", "connectionString");
            var blobStorageService = BlobStorageService.GetInstance();
            blobStorageService.Init(connString);
        }

        private static void InitRedisService()
        {
            var redisService = RedisService.GetInstance();
            redisService.Init(new RedisConnectionProperty[]
            {
                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.SearchResultCacheName,
                    ConnectionString = EnvVariables.Get("redis", "searchResultCacheConnectionString")
                },

                new RedisConnectionProperty
                {
                    ConnectionName = ApConstant.MasterDataCacheName,
                    ConnectionString = EnvVariables.Get("redis", "masterDataCacheConnectionString")
                },

            });
        }
    }
}
