﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Logic;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Object;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620
{
    [Authorize]
    public class Uw620OrderHistoryController : Controller
    {
        // GET: UW620OrderHistory
        public ActionResult OrderHistory(Uw620OrderHistoryRespone request)
        {
            var flight = FlightService.GetInstance();
            var email = User.Identity.GetEmail();
            var reservations = flight.GetOverviewReservationsByContactEmail(email);
            return View(reservations ?? new List<FlightReservation>());
        }

        public ActionResult OrderFlightHistoryDetail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservation(rsvNo);
            return View(reservation);
        }
    }
}