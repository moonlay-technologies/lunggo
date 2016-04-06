using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Filter;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.Util;

namespace Lunggo.CustomerWeb.Controllers
{
    public class FlightController : Controller
    {
        [DeviceDetectionFilter]
        public ActionResult Search(FlightSearchData search)
        {
            try
            {
                var parts = search.info.Split('-');
                var tripPart = parts.First();

                var trips = tripPart.Split('.').Select(info => new FlightTrip
                {
                    OriginAirport = info.Substring(0, 3),
                    DestinationAirport = info.Substring(3, 3),
                    DepartureDate = new DateTime(
                        2000 + int.Parse(info.Substring(10, 2)),
                        int.Parse(info.Substring(8, 2)),
                        int.Parse(info.Substring(6, 2)))
                }).ToList();
                var tripType = FlightService.GetInstance().ParseTripType(trips);
                var requestId = Guid.NewGuid().ToString();
                FlightService.GetInstance().SetFlightRequestTripType(requestId, tripType == TripType.RoundTrip);
                ViewBag.RequestId = requestId;
                switch (tripType)
                {
                    case TripType.OneWay:
                        return View("Search-Single", search);
                    case TripType.RoundTrip:
                        return View("Search-Return", search);
                    default:
                        return View("Search-Single", search);
                }
            }
            catch
            {
                return View("Search-Single", search);
            }
        }

        [HttpPost]
        public ActionResult Select(string token)
        {
            //try
            //{
                var tokens = token.Split('.').ToList();
                var flightService = FlightService.GetInstance();
                var newToken = flightService.BundleFlight(tokens);
                return RedirectToAction("Checkout", "Flight", new { token = newToken });
            //}
            //catch
            //{
            //    return RedirectToAction("Checkout", "Flight");
            //}
        }

        [RequireHttps]
        public ActionResult Checkout(string token)
        {
            var itin = FlightService.GetInstance().GetItineraryForDisplay(token);
            if (itin != null)
            {
                if (TempData["FlightCheckoutOrBookingError"] != null)
                {
                    ViewBag.Message = "BookFailed";
                    return View();
                }

                if (token == null)
                {
                    ViewBag.Message = "BookExpired";
                    return View();
                }

                try
                {
                    var flight = FlightService.GetInstance();
                    var payment = PaymentService.GetInstance();
                    var itinerary = flight.GetItineraryForDisplay(token);
                    var expiryTime = flight.GetItineraryExpiry(token);
                    var savedPassengers = flight.GetSavedPassengers(User.Identity.GetEmail());
                    var savedCreditCards = User.Identity.IsAuthenticated
                        ? payment.GetSavedCreditCards(User.Identity.GetEmail())
                        : new List<SavedCreditCard>();
                    return View(new FlightCheckoutData
                    {
                        Token = token,
                        Itinerary = itinerary,
                        ExpiryTime = expiryTime.GetValueOrDefault(),
                        SavedPassengers = savedPassengers,
                        SavedCreditCards = savedCreditCards
                    });
                }
                catch
                {
                    ViewBag.Message = "BookExpired";
                    return View(new FlightCheckoutData
                    {
                        Token = token
                    });
                }
            }
            else 
            {
                return RedirectToAction("Index", "UW000TopPage");
            }
            
        }

        [RequireHttps]
        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var paymentUrl = flight.GetBookingRedirectionUrl(rsvNo);
            if (paymentUrl != null)
                return Redirect(paymentUrl);
            else
            {
                TempData["AllowThisThankyouPage"] = rsvNo;
                return RedirectToAction("Thankyou", "Flight", new { rsvNo });
            }

        }

        public ActionResult Thankyou(string rsvNo)
        {
            if (TempData["AllowThisThankyouPage"] as string == rsvNo)
            {
                var service = FlightService.GetInstance();
                var summary = service.GetReservationForDisplay(rsvNo);
                return View(summary);
            }
            else
            {
                return RedirectToAction("Index", "UW000TopPage");
            }
        }

        [HttpPost]
        [ActionName("Thankyou")]
        public ActionResult ThankyouPost(string rsvNo)
        {
            TempData["AllowThisReservationCheck"] = rsvNo;
            return RedirectToAction("OrderFlightHistoryDetail", "Uw620OrderHistory", new { rsvNo });
        }

        public ActionResult Confirmation(string rsvNo)
        {
            var reservation = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            if (reservation.Payment.Method == PaymentMethod.BankTransfer &&
                (reservation.Payment.Status == PaymentStatus.Pending || reservation.Payment.Status == PaymentStatus.Challenged))
            {
                return View(new FlightPaymentConfirmationData
                {
                    RsvNo = rsvNo,
                    FinalPrice = reservation.Payment.FinalPrice,
                    TimeLimit = reservation.Payment.TimeLimit.GetValueOrDefault()
                });
            }
            else
                return RedirectToAction("Index", "UW000TopPage");
        }

        [HttpPost]
        public ActionResult Confirmation(TransferConfirmationReport report, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Confirmation", "Flight", new { report.RsvNo });
            var fileInfo = file != null && file.ContentLength > 0
                ? new FileInfo
                {
                    FileData = file.InputStream.StreamToByteArray(),
                    ContentType = file.ContentType,
                    FileName = file.FileName
                }
                : null;
            var paymentService = PaymentService.GetInstance();
            paymentService.SubmitTransferConfirmationReport(report, fileInfo);
            return RedirectToAction("Thankyou", "Flight", new { rsvNo = report.RsvNo });
        }

        public ActionResult TopDestinations()
        {
            var flightService = FlightService.GetInstance();
            var topDestinations = flightService.GetTopDestination();
            return View(topDestinations);
        }

        public ActionResult Eticket(string rsvNo)
        {
            try
            {
                var eticketData = FlightService.GetInstance().GetEticket(rsvNo);
                return File(eticketData, "application/pdf");
            }
            catch
            {
                return View();
            }

        }

        #region Helpers

        public static string RsvNoHash(string rsvNo)
        {
            var rsa = RSA.Create();
            var encryptedRsvNo = rsa.EncryptValue(Encoding.UTF8.GetBytes(rsvNo));
            return Encoding.UTF8.GetString(encryptedRsvNo);
        }

        private static string RsvNoUnhash(string encryptedRsvNo)
        {
            var rsa = RSA.Create();
            var rsvNo = rsa.DecryptValue(Encoding.UTF8.GetBytes(encryptedRsvNo));
            return Encoding.UTF8.GetString(rsvNo);
        }

        #endregion
    }
}