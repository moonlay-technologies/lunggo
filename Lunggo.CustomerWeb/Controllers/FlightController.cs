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
using Lunggo.ApCommon.Flight.Query.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Query;
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

        public ActionResult Checkout(FlightSelectData select)
        {
            var service = FlightService.GetInstance();
            var itineraryFare = service.GetItineraryFromCache(select.token);
            var itinerary = service.ConvertToItineraryApi(itineraryFare);
            return View(new FlightCheckoutData
            {
                HashKey = select.token,
                Itinerary = itinerary,
                Message = ""
            });
        }

        [HttpPost]
        public ActionResult Checkout(FlightCheckoutData data)
        {
            data.ItineraryFare = FlightService.GetInstance().GetItineraryFromCache(data.HashKey);
            var passengerInfo = new List<PassengerInfoFare>();
            if (data.AdultPassengerData != null)
            {
                var adultPassengerInfo = data.AdultPassengerData.Select(passenger => new PassengerInfoFare
                {
                    Type = PassengerType.Adult,
                    Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                    Title = passenger.Title,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    DateOfBirth = passenger.BirthDate,
                    PassportNumber = passenger.PassportNumber,
                    PassportCountry = passenger.Country,
                    PassportExpiryDate = passenger.PassportExpiryDate ?? DateTime.Now.AddYears(1).Date
                }).ToList();
                passengerInfo.AddRange(adultPassengerInfo);
            }
            if (data.ChildPassengerData != null)
            {
                var childPassengerInfo = data.ChildPassengerData.Select(passenger => new PassengerInfoFare
                {
                    Type = PassengerType.Child,
                    Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                    Title = passenger.Title,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    DateOfBirth = passenger.BirthDate,
                    PassportNumber = passenger.PassportNumber,
                    PassportCountry = passenger.Country,
                    PassportExpiryDate = passenger.PassportExpiryDate ?? DateTime.Now.AddYears(1).Date
                }).ToList();
                passengerInfo.AddRange(childPassengerInfo);
            }
            if (data.InfantPassengerData != null)
            {
                var infantPassengerInfo = data.InfantPassengerData.Select(passenger => new PassengerInfoFare
                {
                    Type = PassengerType.Infant,
                    Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                    Title = passenger.Title,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    DateOfBirth = passenger.BirthDate,
                    PassportNumber = passenger.PassportNumber,
                    PassportCountry = passenger.Country,
                    PassportExpiryDate = passenger.PassportExpiryDate ?? DateTime.Now.AddYears(1).Date
                }).ToList();
                passengerInfo.AddRange(infantPassengerInfo);
            }
            var bookInfo = new BookFlightInput
            {
                ContactData = new ContactData
                {
                    Name = data.ContactData.Name,
                    CountryCode = data.ContactData.CountryCode,
                    Phone = data.ContactData.Phone,
                    Email = data.ContactData.Email
                },
                PassengerInfoFares = passengerInfo,
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
            if (bookResult.IsSuccess)
            {
                if (bookResult.BookResult.BookingStatus == BookingStatus.Booked)
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


                    string url;
                    PaymentService.GetInstance().ProcessViaThirdPartyWeb(transactionDetails, itemDetails, out url);
                    return Redirect(url);
                }
                else
                {
                    data.Message = "Already Booked. Please try again.";
                    return View(data);
                }
            }
            else
            {
                data.Message = "Already Booked. Please try again.";
                return View(data);
            }
        }

        public ActionResult Checkout1(FlightSelectData select)
        {
            var service = FlightService.GetInstance();
            var itineraryFare = service.GetItineraryFromCache(select.token);
            var itinerary = service.ConvertToItineraryApi(itineraryFare);
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.SearchResultCacheName);
            var data = new FlightCheckoutData
            {
                HashKey = select.token,
                Itinerary = itinerary,
                Message = ""
            };
            redis.StringSet("cekot", Newtonsoft.Json.JsonConvert.SerializeObject(data));
            return View(data);
        }

        [HttpPost]
        public ActionResult Checkout2(FlightCheckoutData data)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.SearchResultCacheName);
            var data2json = redis.StringGet("cekot");
            var data2 = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightCheckoutData>(data2json);
            TryUpdateModel(data2);
            redis.StringSet("cekot", Newtonsoft.Json.JsonConvert.SerializeObject(data2));
            return View(data2);
        }

        [HttpPost]
        public ActionResult Checkout3(FlightCheckoutData data)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.SearchResultCacheName);
            var data2json = redis.StringGet("cekot");
            var data2 = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightCheckoutData>(data2json);
            TryUpdateModel(data2);
            redis.StringSet("cekot", Newtonsoft.Json.JsonConvert.SerializeObject(data2));
            return View(data2);
        }

        [HttpPost]
        public ActionResult Checkout4(FlightCheckoutData data)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.SearchResultCacheName);
            var data2json = redis.StringGet("cekot");
            var data2 = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightCheckoutData>(data2json);
            TryUpdateModel(data2);
            return View(data);
        }

        public ActionResult Thankyou(string rsvNo)
        {
            var service = FlightService.GetInstance();
            var summary = service.GetFlightSummary(rsvNo);
            return View(summary);
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        public ActionResult Eticket(string rsvNo)
                            {
            var service = FlightService.GetInstance();
            var summary = service.GetDetails(rsvNo);
            return View(summary);
        }
    }
}