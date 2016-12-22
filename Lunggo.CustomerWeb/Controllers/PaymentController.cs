using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.CustomerWeb.Models;
using PaymentData = Lunggo.CustomerWeb.Models.PaymentData;

namespace Lunggo.CustomerWeb.Controllers
{
    public class PaymentController : Controller
    {
        [RequireHttps]
        public ActionResult Payment(string rsvNo)
        {
            try
            {
                ReservationForDisplayBase rsv;
                if (rsvNo[0] == '1')
                    rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
                else
                    rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);

                return View(new PaymentData
                {
                    RsvNo = rsvNo,
                    Reservation = rsv,
                    TimeLimit = rsv.Payment.TimeLimit.GetValueOrDefault(),
                });
            }
            catch
            {
                ViewBag.Message = "Failed";
                return View(new PaymentData
                {
                    RsvNo = rsvNo
                });
            }

        }

        [RequireHttps]
        [HttpPost]
        [ActionName("Payment")]
        public ActionResult PaymentPost(string rsvNo, string paymentUrl)
        {
            var payment = PaymentService.GetInstance().GetPayment(rsvNo);
            if (payment.Method == PaymentMethod.BankTransfer ||
                payment.Method == PaymentMethod.VirtualAccount)
            {
                return RedirectToAction("Confirmation", "Payment", new { rsvNo });
            }
            else if (!string.IsNullOrEmpty(paymentUrl))
            {
                return Redirect(paymentUrl);
            }
            else
            {
                TempData["AllowThisThankyouPage"] = rsvNo;
                return RedirectToAction("Thankyou", "Payment", new { rsvNo });
            }
        }

        public ActionResult Confirmation(string rsvNo)
        {
            ReservationForDisplayBase rsv;
            if (rsvNo[0] == '1')
                rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            else
                rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
            if ((rsv.Payment.Method == PaymentMethod.BankTransfer || rsv.Payment.Method == PaymentMethod.VirtualAccount) 
                && rsv.Payment.Status == PaymentStatus.Pending)
            {
                return View(rsv);
            }
            else
                TempData["AllowThisThankyouPage"] = rsvNo;
            return RedirectToAction("Thankyou", "Payment", new { rsvNo });
        }


        public ActionResult Thankyou(string rsvNo)
        {
            ReservationForDisplayBase rsv;
            if (rsvNo[0] == '1')
                rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            else
                rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
            return View(rsv);
        }

        [HttpPost]
        [ActionName("Thankyou")]
        public ActionResult ThankyouPost(string rsvNo)
        {
            TempData["AllowThisReservationCheck"] = rsvNo;
            return RedirectToAction("OrderFlightHistoryDetail", "Account", new { rsvNo });
        }

        public ActionResult Eticket(string rsvNo)
        {
            ReservationForDisplayBase rsvData;
            if (rsvNo.StartsWith("1"))
                rsvData = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            else
                rsvData = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
            try
            {

                if (rsvData.RsvDisplayStatus == RsvDisplayStatus.Issued)
                {
                    if (rsvData.Type == ProductType.Flight)
                    {
                        var eticketData = FlightService.GetInstance().GetEticket(rsvNo);
                        return File(eticketData, "application/pdf");
                    }
                    else
                    {
                        var eticketData = HotelService.GetInstance().GetEticket(rsvNo);
                        return File(eticketData, "application/pdf");
                    }
                }
                else
                {
                    return View(rsvData);
                }

            }
            catch
            {
                return View(rsvData);
            }

        }
    }
}