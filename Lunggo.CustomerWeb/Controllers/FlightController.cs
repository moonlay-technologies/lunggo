using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Logic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Redis;
using StackExchange.Redis;

namespace Lunggo.CustomerWeb.Controllers
{
    public class FlightController : Controller
    {
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
                FlightService.GetInstance().RevalidateFlight(new RevalidateFlightInput {FareId = itinerary.FareId});
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
                        IsPassportRequired = data.IsPassportRequired,
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
                IsPassportRequired = data.IsPassportRequired
            };
            return View(inputData);
        }

        [HttpPost]
        public ActionResult Checkout(FlightCheckoutData data)
        {
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
                    IdNumber = passenger.PassportOrIdNumber,
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
                    IdNumber = passenger.PassportOrIdNumber,
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
                    IdNumber = passenger.PassportOrIdNumber,
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
                }
            };
            var revalidateResult =
                FlightService.GetInstance().RevalidateFlight(new RevalidateFlightInput { FareId = data.Itinerary.FareId });
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
                                    data.Message = "Error" + tripDetails.Errors;
                                    return View(data);
                                }
                            }
                            else
                            {
                                data.Message = "Error" + issueResult.Errors;
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
                        data.Message = "Error" + bookResult.Errors;
                        return View(data);
                    }
                }
                else
                {
                    if (revalidateResult.Itinerary != null)
                    {
                        data.Message = "Fare is updated to" + revalidateResult.Itinerary.TotalFare;
                        return View(data);
                    }
                    else
                    {
                        data.Message = "No other fare available.";
                        return View(data);
                    }
                }
            }
            else
            {
                data.Message = "Error" + revalidateResult.Errors;
                return View(data);
            }
        }

        public ActionResult Eticket(FlightItineraryDetails itin)
        {
            return View(itin);
        }
    }
}