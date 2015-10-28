using System;
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
using System.IO;
using Lunggo.Framework.HtmlTemplate;

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
            return View(reservations ?? new List<FlightReservationForDisplay>());
        }

        public ActionResult OrderFlightHistoryDetail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservationForDisplay(rsvNo);
            return View(reservation);
        }

        public ActionResult OrderFlightHistoryDetailPrint(string rsvNo)
        {
            var flightService = FlightService.GetInstance();
            var templateService = HtmlTemplateService.GetInstance();
            var converter = new SelectPdf.HtmlToPdf();
            var reservation = flightService.GetDetails(rsvNo);
            string eticket = templateService.GenerateTemplate(reservation, "FlightEticket");
            eticket = eticket.Replace("<body class=\"eticket\">", "<body onload=\"window.print()\">");
            return Content(eticket);
        }
    }
}