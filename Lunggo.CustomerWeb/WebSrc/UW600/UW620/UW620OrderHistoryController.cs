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

        public FileStreamResult OrderFlightHistoryDetailPrint(string rsvNo)
        {
            Stream fileStream = GeneratePDF(rsvNo);

            HttpContext.Response.AddHeader("content-disposition",
            "attachment; filename=" + rsvNo + "pdf");

            return new FileStreamResult(fileStream, "application/pdf");
        }
        private Stream GeneratePDF(string rsvNo)
        {
            var flightService = FlightService.GetInstance();
            var templateService = HtmlTemplateService.GetInstance();
            var converter = new SelectPdf.HtmlToPdf();
            var reservation = flightService.GetDetails(rsvNo);
            var eticketTemplate = templateService.GenerateTemplate(reservation, "FlightEticket");

            var eticketFile = converter.ConvertHtmlString(eticketTemplate).Save();

            MemoryStream ms = new MemoryStream();

            byte[] byteInfo = eticketFile;
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            return ms;
        }
    }
}