using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public FlightItineraryForDisplay GetItineraryForDisplay(string token)
        {
            var itin = GetItineraryFromCache(token);
            return ConvertToItineraryForDisplay(itin);
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

        internal FlightReservation GetReservation(string rsvNo)
        {
            try
            {
                return GetDb.Reservation(rsvNo);
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
                var rsv = GetDb.OverviewReservation(rsvNo);
                return ConvertToReservationForDisplay(rsv);
            }
            catch
            {
                return null;
            }
        }

        public List<FlightReservationForDisplay> GetOverviewReservationsByContactEmail(string contactEmail)
        {
            var rsvs = GetDb.OverviewReservationsByContactEmail(contactEmail) ?? new List<FlightReservation>();
            return rsvs.Select(ConvertToReservationForDisplay).ToList();
        }

        public List<FlightReservationForDisplay> SearchReservations(FlightReservationSearch search)
        {
            var rsvs = GetDb.SearchReservations(search);
            return rsvs.Select(ConvertToReservationForDisplay).ToList();
        }

        public List<FlightReservation> GetUnpaidReservations()
        {
            return GetDb.UnpaidReservations().ToList();
        }

        public void ExpireReservations()
        {
            UpdateDb.ExpireReservations();
        }

        internal void CancelReservation(string rsvNo, CancellationType cancellationType)
        {
            UpdateDb.CancelReservation(rsvNo, cancellationType);
        }

        internal void ConfirmReservationRefund(string rsvNo, Refund refund)
        {
            UpdateDb.ConfirmRefund(rsvNo, refund);
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

        public void GetAndUpdateBookingStatus(out List<string> ticketedRsvNos, out List<string> scheduleChangedRsvNos)
        {
            var statusData = GetBookingStatusInternal();
            if (statusData.Any())
                UpdateDb.BookingStatus(statusData);
            var ticketedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.Ticketed).Select(data => data.BookingId).ToList();
            var scheduleChangedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.ScheduleChanged).Select(data => data.BookingId).ToList();
            var rsvNosWithTicketedBooking = ticketedBookingIds.Any()
                ? GetDb.RsvNoByBookingId(ticketedBookingIds).Distinct()
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
                ? GetDb.RsvNoByBookingId(scheduleChangedBookingIds).Distinct().ToList()
                : new List<string>();
        }

        public void UpdateIssueProgress(string rsvNo, string progressMessage)
        {
            UpdateDb.IssueProgress(rsvNo, progressMessage);
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
