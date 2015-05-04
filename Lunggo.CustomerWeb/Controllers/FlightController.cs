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
            return View(itinerary);
        }

        [HttpPost]
        public ActionResult Checkout(FlightCheckoutData data)
        {
            data.Itinerary = FlightService.GetInstance().GetItineraryFromCache(data.Hash);
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
                    PassportExpiryDate = passenger.PassportExpiryDate
                }).ToList();
                passengerInfo.AddRange(adultPassengerInfo);
            }
            if (data.ChildPassengerData != null)
            {
                var childPassengerInfo = data.ChildPassengerData.Select(passenger => new PassengerInfoFare
                {
                    Type = PassengerType.Adult,
                    Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                    Title = passenger.Title,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    DateOfBirth = passenger.BirthDate,
                    IdNumber = passenger.IdNumber,
                    PassportCountry = passenger.Country,
                    PassportExpiryDate = passenger.PassportExpiryDate
                }).ToList();
                passengerInfo.AddRange(childPassengerInfo);
            }
            if (data.InfantPassengerData != null)
            {
                var infantPassengerInfo = data.InfantPassengerData.Select(passenger => new PassengerInfoFare
                {
                    Type = PassengerType.Adult,
                    Gender = passenger.Title == Title.Mister ? Gender.Male : Gender.Female,
                    Title = passenger.Title,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    DateOfBirth = passenger.BirthDate,
                    IdNumber = passenger.IdNumber,
                    PassportCountry = passenger.Country,
                    PassportExpiryDate = passenger.PassportExpiryDate
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
                OverallTripType = TripType.OneWay,
                PaymentData = data.PaymentData
            };
            var revalidateResult =
                FlightService.GetInstance().RevalidateFlight(new RevalidateFlightInput
                {
                    FareId = data.Itinerary.FareId,
                    TripInfos = new List<FlightTripInfo>
                    {
                        new FlightTripInfo
                        {
                            OriginAirport = data.Itinerary.FlightTrips[0].OriginAirport,
                            DestinationAirport= data.Itinerary.FlightTrips[0].DestinationAirport,
                            DepartureDate = data.Itinerary.FlightTrips[0].DepartureDate
                        }
                    }
                });
            if (revalidateResult.IsSuccess)
            {
                if (revalidateResult.IsValid)
                {
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
                                    BookingId = issueResult.BookingId
                                });
                                if (tripDetails.IsSuccess)
                                {
                                    return RedirectToAction("Eticket", tripDetails.FlightDetails);
                                }
                                else
                                {
                                    ViewBag.Message = "Technical Error. Please try again.");
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Technical Error. Please try again.");
                                return View();
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Technical Error. Please try again.");
                        return View();
                    }
                }
                else
                {
                    if (revalidateResult.Itinerary != null)
                    {
                        ViewBag.Message = "Fare is updated to" + revalidateResult.Itinerary.TotalFare + ".\nPlease submit again.");
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "No other fare available on this flight.");
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.Message = "Technical Error. Please try again.");
                return View();
            }
        }

        public ActionResult Eticket(FlightItineraryDetails itin)
        {
            return View(itin);
        }
    }
}