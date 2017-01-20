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

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
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
            var tokens = token;
            return RedirectToAction("Checkout", "Flight", new { token = tokens});
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
                    var expiryTime = flight.GetItineraryExpiry(token);
                    //var savedPassengers = flight.GetSavedPassengers(User.Identity.GetEmail());
                    //var savedCreditCards = User.Identity.IsAuthenticated
                    //    ? payment.GetSavedCreditCards(User.Identity.GetEmail())
                    //    : new List<SavedCreditCard>();
                    return View(new FlightCheckoutData
                    {
                        Token = token,
                        Itinerary = itin,
                        ExpiryTime = expiryTime.GetValueOrDefault(),
                        //SavedPassengers = savedPassengers,
                        //SavedCreditCards = savedCreditCards
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
                return RedirectToAction("Index", "Index");
            }
            
        }

        //Buat ngelempar ke halaman payment
        [RequireHttps]
        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            var regId = GenerateId(rsvNo);
            return RedirectToAction("Payment", "Payment", new { rsvNo, regId });
        }
        
        public ActionResult TopDestinations()
        {
            var flightService = FlightService.GetInstance();
            var topDestinations = flightService.GetTopDestination();
            return View(topDestinations);
        }

        #region Helpers

        public string GenerateId(string key)
        {
            string result = "";
            if (key.Length > 7)
            {
                key = key.Substring(key.Length - 7);
            }
            int generatedNumber = (int)double.Parse(key);
            for (int i = 1; i < 4; i++)
            {
                generatedNumber = new Random(generatedNumber).Next();
                result = result + "" + generatedNumber;
            }
            return result;
        }

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