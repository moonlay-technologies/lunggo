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
            return View(search);
        }

        [RequireHttps]
        public ActionResult Checkout(FlightSelectData select)
        {
            var service = FlightService.GetInstance();
            var itineraryFare = service.GetItineraryFromCache(select.token);
            var itinerary = service.ConvertToItineraryApi(itineraryFare);
            return View(new FlightCheckoutData
            {
                HashKey = select.token,
                Itinerary = itinerary,
            });
        }

        [RequireHttps]
        [HttpPost]
        public ActionResult Checkout(FlightCheckoutData data)
        {
            data.ItineraryFare = FlightService.GetInstance().GetItineraryFromCache(data.HashKey);
            var passengerInfo = data.Passengers.Select(passenger => new PassengerInfoFare
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
                ContactData = new ContactData
                {
                    Name = data.Contact.Name,
                    CountryCode = data.Contact.CountryCode,
                    Phone = data.Contact.Phone,
                    Email = data.Contact.Email
                },
                PassengerInfoFares = passengerInfo.ToList(),
                Itinerary = data.ItineraryFare,
                TripInfos = new List<FlightTripInfo>
                {
                    new FlightTripInfo
                    {
                        OriginAirport = data.ItineraryFare.FlightTrips[0].OriginAirport,
                        DestinationAirport = data.ItineraryFare.FlightTrips[0].DestinationAirport,
                        DepartureDate = data.ItineraryFare.FlightTrips[0].DepartureDate
                    }
                },
                OverallTripType = data.ItineraryFare.TripType
            };
            var bookResult = FlightService.GetInstance().BookFlight(bookInfo);
            if (bookResult.IsSuccess && bookResult.BookResult.BookingStatus == BookingStatus.Booked)
            {
                var transactionDetails = new TransactionDetails
                {
                    OrderId = bookResult.RsvNo,
                    Amount = data.ItineraryFare.LocalPrice
                };
                var itemDetails = new List<ItemDetails>
                    {
                        new ItemDetails
                        {
                            Id = "1",
                            Name = 
                                data.ItineraryFare.TripType + " " +
                                data.ItineraryFare.FlightTrips[0].OriginAirport + "-" +
                                data.ItineraryFare.FlightTrips[0].DestinationAirport + " " +
                                data.ItineraryFare.FlightTrips[0].DepartureDate.ToString("d MMM yy") +
                                (data.ItineraryFare.TripType == TripType.Return
                                ? "-" + data.ItineraryFare.FlightTrips[1].DepartureDate.ToString("d MMM yy")
                                : ""),
                            Quantity = 1,
                            Price = data.ItineraryFare.LocalPrice
                        }
                    };

                var url = PaymentService.GetInstance().GetPaymentUrl(transactionDetails, itemDetails, data.Payment.Method);
                if (url != null)
                    return Redirect(url);
                else
                    return RedirectToAction("Confirmation", "Flight", new {RsvNo = bookResult.RsvNo});
            }
            else
            {
                return RedirectToAction("Checkout", new FlightSelectData
                {
                    token = data.HashKey,
                    error = bookResult.Errors[0]
                });
            }
        }

        [RequireHttps]
        public ActionResult Thankyou(string rsvNo)
        {
            var service = FlightService.GetInstance();
            var summary = service.GetReservation(rsvNo);
            return View(summary);
        }

        public ActionResult Confirmation()
        {
            return View();
        }
    }
}