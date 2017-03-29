using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Util;
using Lunggo.CustomerWeb.Models;
using Lunggo.CustomerWeb.Utils;
using Lunggo.Framework.Config;
using Lunggo.Framework.Filter;
using Lunggo.Framework.Http;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.CustomerWeb.Controllers
{
    public class FlightController : Controller
    {
        [DeviceDetectionFilter]
        [Route("id/tiket-pesawat/cari/{searchParam}/{searchId}")]
        public ActionResult Search(string searchId, string searchParam)
        {
            if (!B2BUtil.IsB2BAuthorized(Request))
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = B2BUtil.IsB2BDomain(Request) ? "B2B" : "B2C";
            var search = new FlightSearchData
            {
                info = searchId
            };
            try
            {
                var parts = searchId.Split('-');
                var tripPart = parts.First();

                var trips = tripPart.Split('~').Select(info => new FlightTrip
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

        [DeviceDetectionFilter]
        [Route("id/tiket-pesawat/cari/{searchParam}")]
        public ActionResult Search(string searchParam)
        {
            if (!B2BUtil.IsB2BAuthorized(Request))
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = B2BUtil.IsB2BDomain(Request) ? "B2B" : "B2C";
            var parts = searchParam.Split('-').ToList();
            var originAirport = parts[parts.Count -2];
            var destinationAirport = parts[parts.Count - 1];
            var nextMonthDate = DateTime.Today.AddMonths(1);
            var data = originAirport + destinationAirport + nextMonthDate.Day.ToString("d2") + nextMonthDate.Month.ToString("d2") +
                             nextMonthDate.Year.ToString().Substring(2, 2) + "-100y";

            var search = new FlightSearchData
            {
                info = data
            };
            try
            {
                var trips = new List<FlightTrip>
                {
                    new FlightTrip{
                        OriginAirport = originAirport,
                        DestinationAirport = destinationAirport,
                        DepartureDate = nextMonthDate
                    }
                };

                var tripType = FlightService.GetInstance().ParseTripType(trips);
                var requestId = Guid.NewGuid().ToString();
                FlightService.GetInstance().SetFlightRequestTripType(requestId, tripType == TripType.RoundTrip);
                ViewBag.RequestId = requestId;
                return View("Search-Single", search);
            }
            catch
            {
                return View("Search-Single", search);
            }
        }
        [HttpPost]
        public ActionResult Select(string token)
        {
            if (!B2BUtil.IsB2BAuthorized(Request))
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = B2BUtil.IsB2BDomain(Request) ? "B2B" : "B2C";
            var tokens = token;
            return RedirectToAction("Checkout", "Flight", new { token = tokens});
        }

        public ActionResult Checkout(string token)
        {
            if (!B2BUtil.IsB2BAuthorized(Request))
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = B2BUtil.IsB2BDomain(Request) ? "B2B" : "B2C";
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
        [HttpPost]
        [ActionName("Checkout")]
        public ActionResult CheckoutPost(string rsvNo)
        {
            if (!B2BUtil.IsB2BAuthorized(Request))
                return RedirectToAction("Index", "Index");
            ViewBag.Domain = B2BUtil.IsB2BDomain(Request) ? "B2B" : "B2C";
            var regId = GenerateId(rsvNo);
            if(ViewBag.Domain.Equals("B2B"))
                return RedirectToAction("B2BThankyou", "Payment", new { rsvNo, regId });

            return RedirectToAction("Payment", "Payment", new { rsvNo, regId });
        }
        
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult UpdateReservation(string token, string rsvNo, string status)
        {
            if (rsvNo == null || status == null || token == null)
            {
                return RedirectToAction("Index", "Index");
            }
            
            var regId = GenerateTokenUtil.GenerateTokenByRsvNo(rsvNo);
            if (!regId.Equals(token))
                return RedirectToAction("Index", "Index");

            if (status.Equals("rejected"))
            {
               return  RedirectToAction("BookingRejection", "Flight", new {token, rsvNo, status});
            }
            else
            {
                var isUpdated = FlightService.GetInstance().UpdateReservation(rsvNo, status, null);
                if (isUpdated)
                {
                    //Gak tau masih dia pergi kemana
                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    //Gak tau masih dia pergi kemana
                    return RedirectToAction("Index", "Index");
                }
            }
            return RedirectToAction("Index", "Index");
        }

        public ActionResult BookingRejection(string token, string rsvNo, string status)
        {
            var regId = GenerateTokenUtil.GenerateTokenByRsvNo(rsvNo);
            if (! regId.Equals(token))
                return RedirectToAction("Index", "Index");
            var rejectionData = new RejectionModel
            {
                Token = token,
                Status = status,
                RsvNo = rsvNo
            };
            return View(rejectionData);
        }

        [HttpPost]
        public ActionResult BookingRejection(RejectionModel rejectionData)
        {
            if (rejectionData == null)
                return null;
            var isUpdated = FlightService.GetInstance().UpdateReservation(rejectionData.RsvNo, rejectionData.Status,rejectionData.Message);
            if (isUpdated)
            {
                //Gak tau masih dia pergi kemana
                return RedirectToAction("Index", "Index");
            }
            else
            {
                //Gak tau masih dia pergi kemana
                return RedirectToAction("Index", "Index");
            }
            
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