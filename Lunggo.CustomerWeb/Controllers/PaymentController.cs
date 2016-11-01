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
using Lunggo.CustomerWeb.Models;

namespace Lunggo.CustomerWeb.Controllers
{
    public class PaymentController : Controller
    {
        [RequireHttps]
        public ActionResult Payment(string rsvNo)
        {
            try
            {
                if (rsvNo[0] == '1')
                {
                    var flight = FlightService.GetInstance();
                    var reservation = flight.GetReservationForDisplay(rsvNo);

                    return View(new FlightPaymentData
                    {
                        RsvNo = rsvNo,
                        FlightReservation = reservation,
                        TimeLimit = reservation.Payment.TimeLimit.GetValueOrDefault(),
                    });
                }
                else
                {
                    var hotel = HotelService.GetInstance();
                    var reservation = hotel.GetReservationForDisplay(rsvNo);

                    return View(new FlightPaymentData
                    {
                        RsvNo = rsvNo,
                        HotelReservation = reservation,
                        TimeLimit = reservation.Payment.TimeLimit.GetValueOrDefault(),
                    });
                }

            }
            catch
            {
                ViewBag.Message = "Failed";
                return View(new FlightPaymentData
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
            var controller = "";
            switch (rsvNo[0])
            {
                case '1':
                    controller = "Flight";
                    break;
                case '2':
                    controller = "Hotel";
                    break;
            }
            var payment = PaymentService.GetInstance().GetPayment(rsvNo);
            if (payment.Method == PaymentMethod.BankTransfer ||
                payment.Method == PaymentMethod.VirtualAccount)
            {
                return RedirectToAction("Confirmation", controller, new { rsvNo });
            }
            else if (!string.IsNullOrEmpty(paymentUrl))
            {
                return Redirect(paymentUrl);
            }
            else
            {
                TempData["AllowThisThankyouPage"] = rsvNo;
                return RedirectToAction("Thankyou", controller, new { rsvNo });
            }
        }
    }
}