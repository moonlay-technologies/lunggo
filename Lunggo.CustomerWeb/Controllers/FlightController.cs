using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Logic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.CustomerWeb.Controllers
{
    public class FlightController : Controller
    {
        public ActionResult SearchList(FlightSearchData data)
        {
            data.FlightList = FlightService.GetInstance().SearchFlight(new SearchFlightInput
            {
                Conditions = new SearchFlightConditions
                {
                    AdultCount = data.Adult,
                    ChildCount = data.Child,
                    InfantCount = data.Infant,
                    CabinClass = data.Cabin,
                    TripInfos = new List<TripInfo>
                    {
                        new TripInfo
                        {
                            OriginAirport = data.Ori,
                            DestinationAirport = data.Dest,
                            DepartureDate = data.Date
                        }
                    }
                }
            }).Itineraries;
            data.TotalFlightCount = data.FlightList.Count;
            data.SearchId = FlightSearchIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            for (var i = 0; i < data.FlightList.Count; i++)
            {
                DictionaryService.GetInstance().ItineraryDict.Add(data.SearchId + i, data.FlightList[i]);
            }
            return View(data);
        }

        [HttpPost]
        public ActionResult SearchList(FlightSelectData data)
        {
            var itinerary = DictionaryService.GetInstance().ItineraryDict[data.SearchId + data.ItinIndex];
            var revalidateResult =
                FlightService.GetInstance().RevalidateFlight(new RevalidateFlightInput
                {
                    FareId = itinerary.FareId,
                    TripInfos = new List<TripInfo>
                    {
                        new TripInfo
                        {
                            OriginAirport = itinerary.FlightTrips[0].OriginAirport,
                            DestinationAirport=  itinerary.FlightTrips[0].DestinationAirport,
                            DepartureDate = itinerary.FlightTrips[0].DepartureDate
                        }
                    }
                });
            if (revalidateResult.IsSuccess)
            {
                if (revalidateResult.IsValid)
                {
                    return RedirectToAction("Checkout", new FlightSelectData
                    {
                        AdultCount = data.AdultCount,
                        ChildCount = data.ChildCount,
                        InfantCount = data.InfantCount,
                        IsBirthDateRequired = data.IsBirthDateRequired,
                        IsPassportRequired = data.IsPassportRequired,
                        SearchId = data.SearchId,
                        ItinIndex = data.ItinIndex
                    });
                }
                else
                {
                    if (revalidateResult.Itinerary != null)
                    {
                        return RedirectToAction("Checkout", new FlightSelectData
                        {
                            AdultCount = data.AdultCount,
                            ChildCount = data.ChildCount,
                            InfantCount = data.InfantCount,
                            IsBirthDateRequired = data.IsBirthDateRequired,
                            IsPassportRequired = data.IsPassportRequired,
                            SearchId = data.SearchId,
                            ItinIndex = data.ItinIndex,
                            Message = "Fare is updated to" + revalidateResult.Itinerary.TotalFare
                        });
                    }
                    else
                    {
                        data.Message = "No other fare available.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                data.Message = "Error" + revalidateResult.Errors;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult SearchListOneWay(FlightSearchData data)
        {
            return View(data);
        }

        [HttpPost]
        public ActionResult SearchListOneWay(FlightSelectData data)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.SearchResultCacheName);
            var json = redis.StringGet(data.SearchId);
            var itinerary =
                FlightCacheUtil.DeserializeFlightItin(json);
            var revalidateResult =
                FlightService.GetInstance().RevalidateFlight(new RevalidateFlightInput
                {
                    FareId = itinerary.FareId,
                    TripInfos = new List<TripInfo>
                    {
                        new TripInfo
                        {
                            OriginAirport = itinerary.FlightTrips[0].OriginAirport,
                            DestinationAirport=  itinerary.FlightTrips[0].DestinationAirport,
                            DepartureDate = itinerary.FlightTrips[0].DepartureDate
                        }
                    }
                });
            if (revalidateResult.IsSuccess)
            {
                if (revalidateResult.IsValid)
                {
                    return RedirectToAction("Checkout", new FlightSelectData
                    {
                        AdultCount = data.AdultCount,
                        ChildCount = data.ChildCount,
                        InfantCount = data.InfantCount,
                        IsBirthDateRequired = data.IsBirthDateRequired,
                        IsPassportRequired = data.IsPassportRequired
                    });
                }
                else
                {
                    if (revalidateResult.Itinerary != null)
                    {
                        return RedirectToAction("Checkout", new FlightSelectData
                        {
                            AdultCount = data.AdultCount,
                            ChildCount = data.ChildCount,
                            InfantCount = data.InfantCount,
                            IsBirthDateRequired = data.IsBirthDateRequired,
                            IsPassportRequired = data.IsPassportRequired,
                            Message = "Fare is updated to" + revalidateResult.Itinerary.TotalFare
                        });
                    }
                    else
                    {
                        data.Message = "No other fare available.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                data.Message = "Error" + revalidateResult.Errors;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SearchListReturn(FlightSearchData data)
        {
            return View(data);
        }

        [HttpPost]
        public ActionResult SearchListReturn(FlightSelectData data)
        {
            var redis = RedisService.GetInstance().GetDatabase(ApConstant.SearchResultCacheName);
            var json = redis.StringGet(data.SearchId);
            var itinerary =
                FlightCacheUtil.DeserializeFlightItin(json);
            var revalidateResult =
                FlightService.GetInstance().RevalidateFlight(new RevalidateFlightInput { FareId = itinerary.FareId });
            if (revalidateResult.IsSuccess)
            {
                if (revalidateResult.IsValid)
                {
                    return RedirectToAction("Checkout", new FlightSelectData
                    {
                        AdultCount = data.AdultCount,
                        ChildCount = data.ChildCount,
                        InfantCount = data.InfantCount,
                        IsBirthDateRequired = data.IsBirthDateRequired,
                        IsPassportRequired = data.IsPassportRequired
                    });
                }
                else
                {
                    if (revalidateResult.Itinerary != null)
                    {
                        return RedirectToAction("Checkout", new FlightSelectData
                        {
                            AdultCount = data.AdultCount,
                            ChildCount = data.ChildCount,
                            InfantCount = data.InfantCount,
                            IsBirthDateRequired = data.IsBirthDateRequired,
                            IsPassportRequired = data.IsPassportRequired,
                            Message = "Fare is updated to" + revalidateResult.Itinerary.TotalFare
                        });
                    }
                    else
                    {
                        data.Message = "No other fare available.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                data.Message = "Error" + revalidateResult.Errors;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Checkout(FlightSelectData data)
        {
            var inputData = new FlightCheckoutData
            {
                AdultCount = data.AdultCount,
                ChildCount = data.ChildCount,
                InfantCount = data.InfantCount,
                IsBirthDateRequired = data.IsBirthDateRequired,
                IsPassportRequired = data.IsPassportRequired,
                SearchId = data.SearchId,
                ItinIndex = data.ItinIndex
            };
            return View(inputData);
        }

        [HttpPost]
        public ActionResult Checkout(FlightCheckoutData data)
        {
            data.Itinerary = DictionaryService.GetInstance().ItineraryDict[data.SearchId + data.ItinIndex];
            var passengerInfo = new List<PassengerFareInfo>();
            if (data.AdultPassengerData != null)
            {
                var adultPassengerInfo = data.AdultPassengerData.Select(passenger => new PassengerFareInfo
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
                var childPassengerInfo = data.ChildPassengerData.Select(passenger => new PassengerFareInfo
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
                var infantPassengerInfo = data.InfantPassengerData.Select(passenger => new PassengerFareInfo
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
                    PassengerFareInfos = passengerInfo
                },
                Itinerary = data.Itinerary,
                TripInfos = new List<TripInfo>
                    {
                        new TripInfo
                        {
                            OriginAirport = data.Itinerary.FlightTrips[0].OriginAirport,
                            DestinationAirport=  data.Itinerary.FlightTrips[0].DestinationAirport,
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
                    TripInfos = new List<TripInfo>
                    {
                        new TripInfo
                        {
                            OriginAirport = data.Itinerary.FlightTrips[0].OriginAirport,
                            DestinationAirport = data.Itinerary.FlightTrips[0].DestinationAirport,
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
                                    BookingId = issueResult.BookingId,
                                    TripInfos = new List<TripInfo>
                                    {
                                        new TripInfo
                                        {
                                            OriginAirport = data.Itinerary.FlightTrips[0].OriginAirport,
                                            DestinationAirport = data.Itinerary.FlightTrips[0].DestinationAirport,
                                            DepartureDate = data.Itinerary.FlightTrips[0].DepartureDate
                                        }
                                    }
                                });
                                if (tripDetails.IsSuccess)
                                {
                                    DictionaryService.GetInstance().DetailsDict.Add(data.SearchId, tripDetails.FlightDetails.FlightItineraryDetails);
                                    return RedirectToAction("Eticket", new Id { SearchId = data.SearchId});
                                }
                                else
                                {
                                    data.Message = "Error : " + tripDetails.Errors[0];
                                    return View(data);
                                }
                            }
                            else
                            {
                                data.Message = "Error : " + issueResult.Errors[0];
                                return View(data);
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        data.Message = "Error : " + bookResult.Errors[0];
                        return View(data);
                    }
                }
                else
                {
                    if (revalidateResult.Itinerary != null)
                    {
                        data.Message = "Fare is updated to : " + revalidateResult.Itinerary.TotalFare;
                        return View(data);
                    }
                    else
                    {
                        data.Message = "Error : No other fare available.";
                        return View(data);
                    }
                }
            }
            else
            {
                data.Message = "Error : " + revalidateResult.Errors[0];
                return View(data);
            }
        }

        public ActionResult Eticket(Id id)
        {
            var details = DictionaryService.GetInstance().DetailsDict[id.SearchId];
            return View(details);
        }
    }

    public class Id
    {
        public string SearchId { get; set; }
    }
}