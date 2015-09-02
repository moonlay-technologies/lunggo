using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Database;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Filter;
using Lunggo.Framework.Payment.Data;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
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
                var tripType = FlightSearchData.ParseTripType(trips);
                switch (tripType)
                {
                    case TripType.OneWay:
                        return View("SearchResultList-Single", search);
                    case TripType.Return:
                        return View("SearchResultList-Return", search);
                    default :
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
            var tokens = token.Split('.').ToList();
            var flightService = FlightService.GetInstance();
            var newToken = flightService.BundleFlight(tokens);
            return RedirectToAction("Checkout", "Flight", new {token = newToken});
        }

        public ActionResult Checkout(string token)
        {
            var service = FlightService.GetInstance();
            var itinerary = service.GetItineraryForDisplay(token);
            var expiryTime = service.GetItineraryExpiry(token);
            return View(new FlightCheckoutData
            {
                Token = token,
                Itinerary = itinerary,
                ExpiryTime = expiryTime
            });
        }

        [HttpPost]
        public ActionResult Checkout(FlightCheckoutData data)
        {
            data.Payment.Medium = data.Payment.Method == PaymentMethod.BankTransfer 
                ? PaymentMedium.Direct 
                : PaymentMedium.Veritrans;

            var service = FlightService.GetInstance();
            data.Itinerary = service.GetItineraryForDisplay(data.Token);
            var passengerInfo = data.Passengers.Select(passenger => new FlightPassenger
            {
                Type = passenger.Type,
                Title = passenger.Title,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                DateOfBirth = passenger.BirthDate,
                PassportNumber = passenger.PassportNumber,
                PassportExpiryDate = passenger.PassportExpiryDate,
                PassportCountry = passenger.Country
            });
            var bookInfo = new BookFlightInput
            {
                ItinCacheId = data.Token,
                Contact = new ContactData
                {
                    Name = data.Contact.Name,
                    CountryCode = data.Contact.CountryCode,
                    Phone = data.Contact.Phone,
                    Email = data.Contact.Email
                },
                Passengers = passengerInfo.ToList(),
                Payment = data.Payment,
                OverallTripType = data.Itinerary.TripType,
                DiscountCode = data.DiscountCode
            };
            var bookResult = FlightService.GetInstance().BookFlight(bookInfo);
            if (bookResult.IsSuccess)
            {
                var transactionDetails = new TransactionDetails
                {
                    OrderId = bookResult.RsvNo,
                    Amount = bookResult.FinalPrice
                };
                var itemDetails = new List<ItemDetails>
                    {
                        new ItemDetails
                        {
                            Id = "1",
                            Name = 
                                data.Itinerary.TripType + " " +
                                data.Itinerary.FlightTrips[0].OriginAirport + "-" +
                                data.Itinerary.FlightTrips[0].DestinationAirport + " " +
                                data.Itinerary.FlightTrips[0].DepartureDate.ToString("d MMM yy") +
                                (data.Itinerary.TripType == TripType.Return
                                ? "-" + data.Itinerary.FlightTrips[1].DepartureDate.ToString("d MMM yy")
                                : ""),
                            Quantity = 1,
                            Price = bookResult.FinalPrice
                        }
                    };

                var url = PaymentService.GetInstance().GetPaymentUrl(transactionDetails, itemDetails, data.Payment.Method);
                if (url == null && data.Payment.Method == PaymentMethod.BankTransfer)
                    return RedirectToAction("Confirmation", "Flight", new {RsvNo = bookResult.RsvNo});
                else
                    return Redirect(url);
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
    }
}