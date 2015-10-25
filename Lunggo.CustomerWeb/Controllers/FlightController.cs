using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Database;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Filter;
using Lunggo.Framework.Payment.Data;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
using Lunggo.Framework.SharedModel;
using Microsoft.Data.OData.Query;
using RestSharp.Extensions;
using RestSharp.Serializers;

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
        public ActionResult Checkout(FlightCheckoutData data)
        {
            data.Payment.Medium = data.Payment.Method == PaymentMethod.BankTransfer
                ? PaymentMedium.Direct
                : PaymentMedium.Veritrans;

            var thisUrl = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
            var apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var bookApi = apiUrl + "/api/v1/flights/book";
            var client = new WebClient();
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
            client.Headers["Accept-Encoding"] = "";
            client.Headers["Upgrade-Insecure-Requests"] = "1";
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            client.Headers["Origin"] = thisUrl;
            client.Headers["Referer"] = thisUrl;
            var postData = data.Serialize();

            BookFlightOutput bookResult;
            try
            {
                bookResult = client.UploadString(bookApi, postData).Deserialize<BookFlightOutput>();
            }
            catch
            {
                bookResult = new BookFlightOutput
                {
                    IsSuccess = false
                };
            }

            if (bookResult.IsSuccess)
            {
                if (bookResult.PaymentUrl == null)
                    return Redirect(bookResult.PaymentUrl);
                else
                    return RedirectToAction("Thankyou", "Flight", new { bookResult.RsvNo });

            }
            else
            {
                return RedirectToAction("Checkout", new FlightSelectData
                {
                    token = data.Token,
                    error = bookResult.Errors[0]
                });
            }
        }

        public ActionResult Thankyou(string rsvNo)
        {
            var service = FlightService.GetInstance();
            var summary = service.GetReservationForDisplay(rsvNo);
            return View(summary);
        }

        public ActionResult Confirmation(string rsvNo)
        {
            if (rsvNo == null)
                return RedirectToAction("Index", "UW100TopPage");
            var flightService = FlightService.GetInstance();
            var reservation = flightService.GetReservationForDisplay(rsvNo);
            var viewModel = new FlightPaymentConfirmationData
            {
                RsvNo = rsvNo,
                TimeLimit = reservation.Payment.TimeLimit.GetValueOrDefault(),
                FinalPrice = reservation.Payment.FinalPrice
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Confirmation(TransferConfirmationReport report, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Confirmation", "Flight", report.RsvNo);
            var fileInfo = file.ContentLength > 0
                ? new FileInfo
                {
                    FileData = file.InputStream.ReadAsBytes(),
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