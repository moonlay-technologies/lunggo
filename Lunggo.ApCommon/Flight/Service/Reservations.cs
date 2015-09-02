using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls.WebParts;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public FlightItineraryForDisplay GetItineraryForDisplay(string token)
        {
            var itin = GetItineraryFromCache(token);
            return ConvertToItineraryApi(itin);
        }

        public FlightReservationForDisplay GetReservationForDisplay(string rsvNo)
        {
            var rsv = GetReservation(rsvNo);
            return ConvertToReservationApi(rsv);
        }

        internal FlightReservation GetReservation(string rsvNo)
        {
            return GetFlightDb.Reservation(rsvNo);
        }

        internal FlightReservationForDisplay GetOverviewReservation(string rsvNo)
        {
            var rsv = GetFlightDb.OverviewReservation(rsvNo);
            return ConvertToReservationApi(rsv);
        }

        public List<FlightReservationForDisplay> GetOverviewReservationsByContactEmail(string contactEmail)
        {
            var rsvs = GetFlightDb.OverviewReservationsByContactEmail(contactEmail);
            return rsvs.Select(ConvertToReservationApi).ToList();
        }

        public List<FlightReservation> SearchReservations(FlightReservationSearch search)
        {
            return GetFlightDb.SearchReservations(search).ToList();
        }

        public List<FlightReservation> GetUnpaidReservations()
        {
            return GetFlightDb.UnpaidReservations().ToList();
        }

        public void ExpireReservations()
        {
            UpdateFlightDb.ExpireReservations();
        }

        internal void CancelReservation(string rsvNo, CancellationType cancellationType)
        {
            UpdateFlightDb.CancelReservation(rsvNo, cancellationType);
        }

        internal void ConfirmReservationRefund(string rsvNo, RefundInfo refund)
        {
            UpdateFlightDb.ConfirmRefund(rsvNo, refund);
        }
    }
}
