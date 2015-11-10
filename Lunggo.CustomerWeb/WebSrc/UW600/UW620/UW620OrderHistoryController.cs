using System.Collections.Generic;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Object;
using Lunggo.Framework.Config;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.ApCommon.Payment.Constant;

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

        [AllowAnonymous]
        public ActionResult OrderFlightHistoryDetailPrint(string rsvNo)
        {
            //var flightService = FlightService.GetInstance();
            //var templateService = HtmlTemplateService.GetInstance();
            //var converter = new SelectPdf.HtmlToPdf();
            //var reservation = flightService.GetDetails(rsvNo);
            //if (!reservation.IsIssued)
            //    return Content("ticket unavailable");
            //string eticket = templateService.GenerateTemplate(reservation, "FlightEticket");
            ////eticket = eticket.Replace("<body class=\"eticket\">", "<body onload=\"window.print()\">");
            //return Content(eticket);

            return Redirect("https://lunggostorageqa.blob.core.windows.net/eticket/" + rsvNo + ".pdf");
        }

        public ActionResult OrderFlightHistoryDetailResendTicket(string rsvNo)
        {
            var flightService = FlightService.GetInstance();
            var reservation = flightService.GetDetails(rsvNo);
            if (reservation.Payment.Status != ApCommon.Payment.Constant.PaymentStatus.Settled)
                return Content("ticket unavailable");
            if (reservation.Payment.Method != PaymentMethod.BankTransfer)
                flightService.SendInstantPaymentConfirmedNotifToCustomer(rsvNo);
            else
                flightService.SendPendingPaymentConfirmedNotifToCustomer(rsvNo);
            return Content("Success");
        }
    }
}