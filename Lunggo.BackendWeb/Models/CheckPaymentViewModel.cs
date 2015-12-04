using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.BackendWeb.Models
{
    public class CheckPaymentViewModel
    {
        public List<TransferConfirmationReport> Reports;
        public List<FlightReservation> ReportedReservations;
        public List<FlightReservation> UnreportedReservations;
    }
}