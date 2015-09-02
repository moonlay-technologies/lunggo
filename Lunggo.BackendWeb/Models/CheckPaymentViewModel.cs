using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.BackendWeb.Models
{
    public class CheckPaymentViewModel
    {
        public List<TransferConfirmationReport> Reports;
        public List<FlightReservation> Reservation;
    }
}