using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.CustomerWeb.Models;

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
            var itinerary = service.GetItineraryFromCache(select.token);
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
            data.Itinerary = FlightService.GetInstance().GetItineraryFromCache(data.HashKey);
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
                    PassportExpiryDate = passenger.PassportExpiryDate ?? DateTime.Now.AddYears(1)
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
                    PassportExpiryDate = passenger.PassportExpiryDate ?? DateTime.Now.AddYears(1)
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
                    PassportExpiryDate = passenger.PassportExpiryDate ?? DateTime.Now.AddYears(1)
                }).ToList();
                passengerInfo.AddRange(infantPassengerInfo);
            }
            var bookInfo = new BookFlightInput
            {
                BookingInfo = new FlightBookingInfo
                {
                    FareId = data.Itinerary.FareId,
                    ContactData = new ContactData
                    {
                        Name = data.ContactData.Name,
                        Phone = data.ContactData.Phone,
                        Email = data.ContactData.Email
                    },
                    PassengerInfoFares = passengerInfo
                },
                Itinerary = data.Itinerary,
                TripInfos = new List<FlightTripInfo>
                    {
                        new FlightTripInfo
                        {
                            OriginAirport = data.Itinerary.FlightTrips[0].OriginAirport,
                            DestinationAirport= data.Itinerary.FlightTrips[0].DestinationAirport,
                            DepartureDate = data.Itinerary.FlightTrips[0].DepartureDate
                        }
                    },
                OverallTripType = TripType.OneWay
            };
            var bookResult = FlightService.GetInstance().BookFlight(bookInfo);
            if (bookResult.IsSuccess)
            {
                if (bookResult.BookResult.BookingStatus == BookingStatus.Booked)
                {
                    var issueResult = FlightService.GetInstance().IssueTicket(new IssueTicketInput
                    {
                        BookingId = bookResult.BookResult.BookingId
                    });
                    if (issueResult.IsSuccess)
                    {
                        var tripDetails = FlightService.GetInstance().GetDetails(new GetDetailsInput
                        {
                            BookingId = issueResult.BookingId,
                            TripInfos = data.Itinerary.FlightTrips.Select(trip => new FlightTripInfo
                            {
                                OriginAirport = trip.OriginAirport,
                                DestinationAirport = trip.DestinationAirport,
                                DepartureDate = trip.DepartureDate
                            }).ToList()
                        });
                        if (tripDetails.IsSuccess)
                        {
                            FlightService.GetInstance().SaveItineraryToCache(tripDetails.FlightDetails.FlightItineraryDetails, "111");
                            return RedirectToAction("Eticket");
                        }
                        else
                        {
                            data.Message = "Technical Error. Please try again.";
                            return View(data);
                        }
                    }
                    else
                    {
                        data.Message = "Already Booked. Please try again.";
                        return View(data);
                    }
                }
                else
                {
                    data.Message = "Booking Failed. Please try again.";
                    return View(data);
                }
            }
            else
            {
                data.Message = "Technical Error. Please try again.";
                return View(data);
            }
        }

        public ActionResult PaymentResult(string result)
        {
            return null;
        }

        public ActionResult Eticket()
        {
            var itin = FlightService.GetInstance().GetItineraryFromCache("111","a");
            return View(itin);
        }
    }
}