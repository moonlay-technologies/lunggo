using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;

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
            return GetDb.Reservation(rsvNo);
        }

        public FlightReservationForDisplay GetOverviewReservation(string rsvNo)
        {
            var rsv = GetDb.OverviewReservation(rsvNo);
            return ConvertToReservationForDisplay(rsv);
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

        internal void ConfirmReservationRefund(string rsvNo, RefundInfo refund)
        {
            UpdateDb.ConfirmRefund(rsvNo, refund);
        }
    }
}
