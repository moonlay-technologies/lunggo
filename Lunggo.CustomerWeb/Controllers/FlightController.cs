using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
using Lunggo.Framework.Payment.Data;
using Lunggo.Framework.Database;
using RestSharp.Serializers;

namespace Lunggo.CustomerWeb.Controllers
{
    public class FlightController : Controller
    {
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
                    IdNumber = passenger.IdNumber,
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
                    IdNumber = passenger.IdNumber,
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
                    IdNumber = passenger.IdNumber,
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
                OverallTripType = TripType.OneWay
            };
            var bookResult = FlightService.GetInstance().BookFlight(bookInfo);
            if (bookResult.IsSuccess)
            {
                if (bookResult.BookResult.BookingStatus == BookingStatus.Booked)
                {
                    var transactionDetails = new TransactionDetails
                    {
                        OrderId = bookResult.ReservationDetails.RsvNo,
                        Amount = data.ItineraryFare.IdrPrice
                    };
                    var itemDetails = new List<ItemDetails>
                    {
                        new ItemDetails
                        {
                            Id = "1",
                            Name = "pesawat",
                            Quantity = 1,
                            Price = data.ItineraryFare.IdrPrice
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

        public ActionResult Eticket()
        {
            var itin = FlightService.GetInstance().GetItineraryFromCache("111", "a");
            return View(itin);
        }

        public void PaymentConfirmation(FlightPaymentConfirmationData data)
        {
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    /*var bookingIds = GetBookingIdAndTripInfoQuery.GetInstance().Execute(conn, data.RsvNo).ToList();

                    foreach (var bookingId in bookingIds)
                    {
                        var issueResult = FlightService.GetInstance().IssueTicket(new IssueTicketInput
                            {
                                BookingId = bookingId
                            });
                        if (issueResult.IsSuccess)
                        {
                            var detailsResult = FlightService.GetInstance().GetDetails(new GetDetailsInput
                            {
                                BookingId = bookingId,
                            });
                        }
                    }*/
                }
            }
        }
    }
}