using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public FlightItineraryForDisplay GetItineraryForDisplay(string token)
        {
            var itins = GetItinerariesFromCache(token);
            return ConvertToItineraryForDisplay(itins);
        }

        public FlightReservationForDisplay GetReservationForDisplay(string rsvNo)
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

        public FlightReservation GetReservation(string rsvNo)
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

        public FlightReservationForDisplay GetOverviewReservation(string rsvNo)
        {
            try
            {
                var rsv = GetOverviewReservationFromDb(rsvNo);
                return ConvertToReservationForDisplay(rsv);
            }
            catch
            {
                return null;
            }
        }

        public List<FlightReservationForDisplay> GetOverviewReservationsByUserId(string userId, string filter, string sort, int? page, int? itemsPerPage)
        {
            var filters = filter != null ? filter.Split(',') : null;
            var rsvs = GetOverviewReservationsByUserIdFromDb(userId, filters, sort, page, itemsPerPage) ?? new List<FlightReservation>();
            return rsvs.Select(ConvertToReservationForDisplay).ToList();
        }

        public List<FlightReservationForDisplay> GetOverviewReservationsByDeviceId(string deviceId, string filter, string sort, int? page, int? itemsPerPage)
        {
            var filters = filter != null ? filter.Split(',') : null;
            var rsvs = GetOverviewReservationsByDeviceIdFromDb(deviceId, filters, sort, page, itemsPerPage) ?? new List<FlightReservation>();
            return rsvs.Select(ConvertToReservationForDisplay).ToList();
        }

        public List<FlightReservationForDisplay> SearchReservations(FlightReservationSearch search)
        {
            var rsvs = GetSearchReservationsFromDb(search);
            return rsvs.Select(ConvertToReservationForDisplay).ToList();
        }

        public void ExpireReservationWhenTimeout(string rsvNo, DateTime timeLimit)
        {
            var queue = QueueService.GetInstance().GetQueueByReference("FlightExpireReservation");
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

        public void ExpireReservations()
        {
            UpdateExpireReservationsToDb();
        }

        internal void CancelReservation(string rsvNo, CancellationType cancellationType)
        {
            UpdateCancelReservationToDb(rsvNo, cancellationType);
        }

        //internal void ConfirmReservationRefund(string rsvNo, Refund refund)
        //{
        //    UpdateConfirmRefundToDb(rsvNo, refund);
        //}

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

        //public void UpdateIssueProgress(string rsvNo, string progressMessage)
        //{
        //    UpdateIssueProgressToDb(rsvNo, progressMessage);
        //}

        public void GetAndUpdateBookingStatus(out List<string> ticketedRsvNos, out List<string> scheduleChangedRsvNos)
        {
            var statusData = MystiflyWrapper.GetBookingStatus();
            if (statusData.Any())
                UpdateBookingStatusToDb(statusData);
            var ticketedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.Ticketed).Select(data => data.BookingId).ToList();
            var scheduleChangedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.ScheduleChanged).Select(data => data.BookingId).ToList();
            var rsvNosWithTicketedBooking = ticketedBookingIds.Any()
                ? GetRsvNoByBookingIdFromDb(ticketedBookingIds).Distinct()
                : new List<string>();
            var rsvsWithTicketedBooking = rsvNosWithTicketedBooking.Select(GetReservation);
            ticketedRsvNos =
                rsvsWithTicketedBooking.Where(
                    rsv =>
                        rsv.Itineraries.TrueForAll(
                            itin =>
                                itin.BookingStatus == BookingStatus.Ticketed ||
                                itin.BookingStatus == BookingStatus.ScheduleChanged))
                    .Select(rsv => rsv.RsvNo).ToList();
            scheduleChangedRsvNos = scheduleChangedBookingIds.Any()
                ? GetRsvNoByBookingIdFromDb(scheduleChangedBookingIds).Distinct().ToList()
                : new List<string>();
        }

        public List<Tuple<FlightTripForDisplay, BookingStatus>> GetAllBookingStatus(string rsvNo)
        {
            var rsv = GetReservation(rsvNo);
            if (rsv == null)
                return null;
            return
                rsv.Itineraries.SelectMany(
                            itin =>
                        itin.Trips.Select(
                            trip =>
                                new Tuple<FlightTripForDisplay, BookingStatus>(ConvertToTripForDisplay(trip),
                                    itin.BookingStatus))).ToList();
        }
    }
}
