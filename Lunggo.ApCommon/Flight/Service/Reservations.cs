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
        public FlightReservationApi GetReservation(string rsvNo)
        {
            return GetFlightDb.Reservation(rsvNo);
        }

        public FlightReservationApi GetOverviewReservation(string rsvNo)
        {
            return GetFlightDb.OverviewReservation(rsvNo);
        }

        public List<FlightReservationApi> GetOverviewReservationsByContactEmail(string contactEmail)
        {
            return GetFlightDb.OverviewReservationsByContactEmail(contactEmail);
        }

        public List<FlightReservationApi> SearchReservations(FlightReservationSearch search)
        {
            return GetFlightDb.SearchReservations(search).ToList();
        }

        public void ExpireReservations()
        {
            UpdateFlightDb.ExpireReservations();
        }

        public void CancelReservation(string rsvNo, CancellationType cancellationType)
        {
            UpdateFlightDb.CancelReservation(rsvNo, cancellationType);
        }

        public void ConfirmReservationRefund(string rsvNo, RefundInfo refund)
        {
            UpdateFlightDb.ConfirmRefund(rsvNo, refund);
        }
    }
}
