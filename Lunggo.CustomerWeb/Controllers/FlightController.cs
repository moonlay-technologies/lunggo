using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment;
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
        public ActionResult SearchResultList(FlightSearchData search)
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
                var tripType = FlightService.ParseTripType(trips);
                switch (tripType)
                {
                    case TripType.OneWay:
                        return View("SearchResultList-Single", search);
                    case TripType.Return:
                        return View("SearchResultList-Return", search);
                    default:
                        return View("SearchResultList-Single", search);
                }
            }
            catch
            {
                return View("SearchResultList-Single", search);
            }
        }

        [HttpPost]
        public ActionResult Select(string token)
        {
            try
            {
                var tokens = token.Split('.').ToList();
                var flightService = FlightService.GetInstance();
                var newToken = flightService.BundleFlight(tokens);
                return RedirectToAction("Checkout", "Flight", new { token = newToken });
            }
            catch
            {
                return RedirectToAction("Checkout", "Flight");
            }
        }

        public ActionResult Checkout(string token)
        {
            try
            {
                var service = FlightService.GetInstance();
                var itinerary = service.GetItineraryForDisplay(token);
                var expiryTime = service.GetItineraryExpiry(token);
                var savedPassengers = service.GetSavedPassengers(User.Identity.GetEmail());
                return View(new FlightCheckoutData
                {
                    Token = token,
                    Itinerary = itinerary,
                    ExpiryTime = expiryTime.GetValueOrDefault(),
                    SavedPassengers = savedPassengers,
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

        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var paymentUrl = flight.GetBookingRedirectionUrl(rsvNo);
            if (paymentUrl != null)
                return Redirect(paymentUrl);
            else
                return RedirectToAction("Confirmation", "Flight", new { rsvNo });
        }

        public ActionResult Thankyou(string rsvNo)
        {
            var service = FlightService.GetInstance();
            var summary = service.GetReservationForDisplay(rsvNo);
            return View(summary);
        }

        public ActionResult Confirmation(string rsvNo)
        {
            var reservation = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            return View(new FlightPaymentConfirmationData
                {
                    RsvNo = rsvNo,
                    FinalPrice = reservation.Payment.FinalPrice,
                    TimeLimit = reservation.Payment.TimeLimit.GetValueOrDefault()
                });
        }

        [HttpPost]
        public ActionResult Confirmation(TransferConfirmationReport report, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Confirmation", "Flight", new {report.RsvNo});
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
    }
}