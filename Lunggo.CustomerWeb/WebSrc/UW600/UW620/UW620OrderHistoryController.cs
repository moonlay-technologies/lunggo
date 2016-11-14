using System.Collections.Generic;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Product.Model;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Object;
using Lunggo.Framework.Config;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Product.Constant;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620
{
    public class Uw620OrderHistoryController : Controller
    {
        // GET: UW620OrderHistory
        public ActionResult OrderHistory()
        {
            //var flight = FlightService.GetInstance();
            //var email = User.Identity.GetEmail();
            //var reservations = flight.GetOverviewReservationsByContactEmail(email);
            //return View(reservations ?? new List<FlightReservationForDisplay>());
            if (Request.Cookies["authkey"] != null)
            {
                return View();
            }
            else 
            {
                return RedirectToAction("Index", "UW000TopPage");
            }
            
        }

        [HttpPost]
        public ActionResult OrderHistory(string rsvNo)
        {
            var RsvNo = rsvNo;
            return RedirectToAction("OrderFlightHistoryDetail", "UW620OrderHistory", new { rsvNo = RsvNo });
        }

        public ActionResult OrderFlightHistoryDetail(string rsvNo)
        {
            var flightService = FlightService.GetInstance();
            var hotelService = HotelService.GetInstance();
            ReservationForDisplayBase displayReservation;
            if (rsvNo.Substring(0, 1) == "1")
            {
                displayReservation = flightService.GetReservationForDisplay(rsvNo);
            }
            else
            {
                displayReservation = hotelService.GetReservationForDisplay(rsvNo);
            }
            return View(displayReservation);
        }

        [HttpPost]
        public ActionResult OrderFlightHistoryDetail(string rsvNo, string status)
        {
            ReservationForDisplayBase rsv;
            var flightService = FlightService.GetInstance();
            var hotelService = HotelService.GetInstance();
            ReservationForDisplayBase displayReservation;
            if (rsvNo.Substring(0, 1) == "1")
            {
                displayReservation = flightService.GetReservationForDisplay(rsvNo);
            }
            else
            {
                displayReservation = hotelService.GetReservationForDisplay(rsvNo);
            }
            
            //return View(rsv);
            //var flightService = ApCommon.Flight.Service.FlightService.GetInstance();

            if (displayReservation != null)
            {
                switch (displayReservation.RsvDisplayStatus)
                {
                    case RsvDisplayStatus.Cancelled:
                        return RedirectToAction("Index", "UW000TopPage"); // buat cari baru, blom fix

                    case RsvDisplayStatus.Expired:
                        return RedirectToAction("Index", "UW000TopPage"); // buat cari baru, blom fix

                    case RsvDisplayStatus.FailedPaid:
                        return RedirectToAction("Thankyou", "Payment", new {rsvNo = rsvNo});

                    case RsvDisplayStatus.FailedUnpaid:
                        return RedirectToAction("Thankyou", "Payment", new {rsvNo = rsvNo});

                    case RsvDisplayStatus.Issued:
                        return RedirectToAction("Eticket", "Payment", new {rsvNo = rsvNo});

                    case RsvDisplayStatus.Paid:
                        return RedirectToAction("Thankyou", "Payment", new {rsvNo = rsvNo});

                    case RsvDisplayStatus.PaymentDenied:
                        return RedirectToAction("Thankyou", "Payment", new {rsvNo = rsvNo});

                    case RsvDisplayStatus.PendingPayment:

                        if (displayReservation.Payment.Method == PaymentMethod.BankTransfer ||
                            displayReservation.Payment.Method == PaymentMethod.VirtualAccount)
                        {
                            return RedirectToAction("Confirmation", "Payment", new {rsvNo = rsvNo});
                                // jika bank transfer & VA
                        }
                        else if (displayReservation.Payment.Method == PaymentMethod.CimbClicks)
                        {
                            return Redirect(displayReservation.Payment.RedirectionUrl);
                        }
                        else
                        {
                            return RedirectToAction("Thankyou", "Payment", new {rsvNo = rsvNo});
                        }

                    case RsvDisplayStatus.Reserved:
                        return RedirectToAction("Payment", "Payment", new {rsvNo = rsvNo});

                    case RsvDisplayStatus.VerifyingPayment:
                        if (displayReservation.Payment.Method == PaymentMethod.BankTransfer ||
                            displayReservation.Payment.Method == PaymentMethod.VirtualAccount)
                        {
                            return RedirectToAction("Confirmation", "Payment", new {rsvNo = rsvNo});
                                // jika bank transfer & VA
                        }
                        else if (displayReservation.Payment.Method == PaymentMethod.CimbClicks)
                        {
                            return Redirect(displayReservation.Payment.RedirectionUrl);
                        }
                        else
                        {
                            return RedirectToAction("Thankyou", "Payment", new {rsvNo = rsvNo});
                        }

                    default:
                        return RedirectToAction("OrderFlightHistoryDetail", "Uw620OrderHistory", new {rsvNo = rsvNo});

                }
            
        }

            return View();
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

        //public ActionResult OrderFlightHistoryDetailResendTicket(string rsvNo)
        //{
        //    var flightService = FlightService.GetInstance();
        //    var reservation = flightService.GetReservationForDisplay(rsvNo);
        //    if (reservation.Payment.Status != ApCommon.Payment.Constant.PaymentStatus.Settled)
        //        return Content("ticket unavailable");
        //    if (reservation.Payment.Method != PaymentMethod.BankTransfer)
        //        flightService.SendInstantPaymentConfirmedNotifToCustomer(rsvNo);
        //    else
        //        flightService.SendPendingPaymentConfirmedNotifToCustomer(rsvNo);
        //    return Content("Success");
        //}
    }
}