using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.BackendWeb.Models;
using Microsoft.Ajax.Utilities;

namespace Lunggo.BackendWeb.Controllers
{
    [Authorize]
    public class FlightController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(FlightReservationSearch search)
        {
            var flight = FlightService.GetInstance();
            var reservations = flight.SearchReservations(search);
            return View(reservations);
        }

        public ActionResult Detail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservationForDisplay(rsvNo);
            return View(reservation);
        }

        public ActionResult CheckPayment()
        {
            var reports = PaymentService.GetInstance().GetAllTransferConfirmationReports();
            var reservations = FlightService.GetInstance().GetUnpaidReservations();
            var Temporary = new CheckPaymentViewModel
            {
                Reports = reports,
                Reservation = reservations
            };
            return View(Temporary);
        }
    }
}