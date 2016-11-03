using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Hotel.Constant;

using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public HotelDetailForDisplay GetSelectionFromCache(string token)
        {
            try
            {
                var rsv = GetSelectedHotelDetailsFromCache(token);
                return ConvertToHotelDetailForDisplay(rsv);
            }
            catch
            {
                return null;
            }
        }

        public HotelReservationForDisplay GetReservationForDisplay(string rsvNo)
        {
            try
            {
                var rsv = GetReservation(rsvNo);
                return ConvertToReservationForDisplay(rsv);
            }
            catch
            {
                return null;
            }
        }

        public HotelReservation GetReservation(string rsvNo)
        {
            try
            {
                return GetReservationFromDb(rsvNo);
            }
            catch
            {
                return null;
            }
        }
        
        //TODO SQL
        //public List<HotelReservationForDisplay> GetOverviewReservationsByUserIdOrEmail(string userId, string email, string filter, string sort, int? page, int? itemsPerPage)
        //{
        //    var filters = filter != null ? filter.Split(',') : null;
        //    var rsvs = GetOverviewReservationsByUserIdOrEmailFromDb(userId, email, filters, sort, page, itemsPerPage) ?? new List<HotelReservation>();
        //    return rsvs.Select(ConvertToReservationForDisplay).ToList();
        //}

        //TODO SQL
        //public List<HotelReservationForDisplay> GetOverviewReservationsByDeviceId(string deviceId, string filter, string sort, int? page, int? itemsPerPage)
        //{
        //    var filters = filter != null ? filter.Split(',') : null;
        //    var rsvs = GetOverviewReservationsByDeviceIdFromDb(deviceId, filters, sort, page, itemsPerPage) ?? new List<HotelReservation>();
        //    return rsvs.Select(ConvertToReservationForDisplay).ToList();
        //}

        //public List<HotelReservationForDisplay> SearchReservations(HotelReservationSearch search)
        //{
        //    var rsvs = GetSearchReservationsFromDb(search);
        //    return rsvs.Select(ConvertToReservationForDisplay).ToList();
        //}

        public void ExpireReservationWhenTimeout(string rsvNo, DateTime timeLimit)
        {
            var queue = QueueService.GetInstance().GetQueueByReference("HotelExpireReservation");
            var timeOut = timeLimit - DateTime.UtcNow;
            queue.AddMessage(new CloudQueueMessage(rsvNo), initialVisibilityDelay: timeOut);
        }

        public void ExpireReservation(string rsvNo)
        {
            var payment = PaymentDetails.GetFromDb(rsvNo);
            if (payment.Status == PaymentStatus.Pending || payment.Status == PaymentStatus.Verifying ||
                payment.Status == PaymentStatus.Challenged || payment.Status == PaymentStatus.Undefined)
            {
                payment.Status = PaymentStatus.Expired;
                PaymentService.GetInstance().UpdatePayment(rsvNo, payment);
            }
        }
        
        public byte[] GetEticket(string rsvNo)
        {
            var azureConnString = ConfigManager.GetInstance().GetConfigValue("azureStorage", "connectionString");
            var storageName = azureConnString.Split(';')[1].Split('=')[1];
            var url = @"https://" + storageName + @".blob.core.windows.net/eticket/" + rsvNo + ".pdf";
            var client = new WebClient();
            try
            {
                return client.DownloadData(url);
            }
            catch
            {
                return null;
            }
        }
    }
}
