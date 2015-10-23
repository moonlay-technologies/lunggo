﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Web;
using Microsoft.WindowsAzure.Storage;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return Client.RevalidateFare(conditions);
        }

        private partial class AirAsiaClientHandler
        {
            internal RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
            {
                var client = new ExtendedWebClient();

                if (conditions.FareId == null)
                {
                    return new RevalidateFareResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};
                }

                string origin, dest, coreFareId;
                DateTime date;
                int adultCount, childCount, infantCount;
                CabinClass cabinClass;
                decimal price;
                try
                {
                    var splittedFareId = conditions.FareId.Split('.').ToList();
                    origin = splittedFareId[0];
                    dest = splittedFareId[1];
                    date = new DateTime(int.Parse(splittedFareId[4]), int.Parse(splittedFareId[3]),
                        int.Parse(splittedFareId[2]));
                    adultCount = int.Parse(splittedFareId[5]);
                    childCount = int.Parse(splittedFareId[6]);
                    infantCount = int.Parse(splittedFareId[7]);
                    cabinClass = FlightService.ParseCabinClass(splittedFareId[8]);
                    price = decimal.Parse(splittedFareId[9]);
                    coreFareId = splittedFareId[10];
                }
                catch
                {
                    return new RevalidateFareResult {Errors = new List<FlightError> {FlightError.FareIdNoLongerValid}};
                }

                // [GET] Search Flight

                var dict = DictionaryService.GetInstance();
                var originCountry = dict.GetAirportCountryCode(origin);
                var destinationCountry = dict.GetAirportCountryCode(dest);
                var searchedHtml = new CQ();
                if (originCountry == "ID")
                {
                    var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                              @"?o1=" + origin +
                              @"&d1=" + dest +
                              @"&dd1=" + date.ToString("yyyy-MM-dd") +
                              @"&ADT=" + adultCount +
                              @"&CHD=" + childCount +
                              @"&inl=" + infantCount +
                              @"&s=true" +
                              @"&mon=true" +
                              @"&culture=id-ID" +
                              @"&cc=IDR";
                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Origin"] = "https://booking2.airasia.com";
                    client.Headers["Referer"] = "https://booking2.airasia.com/Payment.aspx";
                    var html = client.DownloadString(url);

                    if (client.ResponseUri.AbsolutePath != "/Flight/Select" && client.StatusCode == HttpStatusCode.OK)
                        return new RevalidateFareResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };

                    searchedHtml = (CQ)html;
                }
                else if (destinationCountry == "ID")
                {
                    var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                              @"?o1=" + dest +
                              @"&d1=" + origin +
                              @"&dd1=" + date.ToString("yyyy-MM-dd") +
                              @"&dd2=" + date.ToString("yyyy-MM-dd") +
                              @"&ADT=" + adultCount +
                              @"&CHD=" + childCount +
                              @"&inl=" + infantCount +
                              @"&r=true" +
                              @"&s=true" +
                              @"&mon=true" +
                              @"&culture=id-ID" +
                              @"&cc=IDR";
                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Origin"] = "https://booking2.airasia.com";
                    client.Headers["Referer"] = "https://booking2.airasia.com/Payment.aspx";
                    var html = client.DownloadString(url);

                    if (client.ResponseUri.AbsolutePath != "/Flight/Select" && client.StatusCode == HttpStatusCode.OK)
                        return new RevalidateFareResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };

                    searchedHtml = (CQ)html;
                }
                else
                {
                    return new RevalidateFareResult
                    {
                        IsSuccess = true,
                        IsValid = false,
                        Itinerary = null
                    };
                }

                // [SCRAPE]

                try
                {
                    var fareIdSuffix = coreFareId.Split('|').Last();
                    CQ radio;
                    switch (cabinClass)
                    {
                        case CabinClass.Economy:
                            radio = searchedHtml["[value$=" + fareIdSuffix + "][id$=0]"];
                            break;
                        case CabinClass.Business:
                            radio = searchedHtml["[value$=" + fareIdSuffix + "][id$=1]"];
                            break;
                        default:
                            radio = null;
                            break;
                    }

                    if (radio.FirstElement() != null)
                    {
                        var foundFareId = radio.Attr("value");
                        var fareIdPrefix = origin + "." + dest + "." + date.ToString("dd.MM.yyyy") + "." + adultCount +
                                           "." +
                                           childCount + "." + infantCount + "." + FlightService.ParseCabinClass(cabinClass) + ".";

                        var url = "https://booking.airasia.com/Flight/PriceItinerary" +
                              "?SellKeys%5B%5D=" + HttpUtility.UrlEncode(foundFareId);
                        var itinHtml = (CQ) client.DownloadString(url);

                        if (itinHtml.Children().Elements.Count() <= 1)
                            return new RevalidateFareResult
                            {
                                IsSuccess = true,
                                IsValid = false,
                                Itinerary = null
                            };

                        var newPrice =
                            decimal.Parse(itinHtml[".section-total-display-price > span:first"].Text().Trim(' ', '\n'),
                                CultureInfo.CreateSpecificCulture("id-ID"));
                        var segmentFareIds = foundFareId.Split('|').Last().Split('^');
                        var segments = new List<FlightSegment>();
                        foreach (var segmentFareId in segmentFareIds)
                        {
                            var splittedSegmentFareId = segmentFareId.Split('~').ToArray();
                            segments.Add(new FlightSegment
                            {
                                AirlineCode = splittedSegmentFareId[0],
                                FlightNumber = splittedSegmentFareId[1].Trim(),
                                CabinClass = cabinClass,
                                Rbd = foundFareId.Split('~')[1],
                                DepartureAirport = splittedSegmentFareId[4],
                                DepartureTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[5]), DateTimeKind.Utc),
                                ArrivalAirport = splittedSegmentFareId[6],
                                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[7]),DateTimeKind.Utc),
                                OperatingAirlineCode = splittedSegmentFareId[0],
                                StopQuantity = 0
                            });
                        }
                        var itin = new FlightItinerary
                        {
                            AdultCount = adultCount,
                            ChildCount = childCount,
                            InfantCount = infantCount,
                            CanHold = true,
                            FareType = FareType.Published,
                            RequireBirthDate = true,
                            RequirePassport = false,
                            RequireSameCheckIn = false,
                            RequireNationality = true,
                            RequestedCabinClass = CabinClass.Economy,
                            TripType = TripType.OneWay,
                            Supplier = Supplier.AirAsia,
                            SupplierCurrency = "IDR",
                            SupplierPrice = newPrice,
                            FareId = fareIdPrefix + price.ToString("0") + "." + foundFareId,
                            Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = origin,
                                    DestinationAirport = dest,
                                    DepartureDate = date,
                                    Segments = segments
                                }
                            }
                        };

                        var fareRow = radio.Parent().Parent().Parent().Parent().Parent();
                        var durationRows = fareRow.Children().First().MakeRoot()["tr:even"];
                        itin.Trips[0].Segments = segments.Zip(durationRows, (segment, durationRow) =>
                        {
                            var durationTexts =
                                durationRow.LastElementChild.FirstElementChild.InnerHTML.Trim().Split(' ').ToList();
                            var duration = new TimeSpan();
                            duration =
                                duration.Add(TimeSpan.ParseExact(durationTexts[0], "h'h'", CultureInfo.InvariantCulture));
                            if (durationTexts.Count > 1)
                                duration =
                                    duration.Add(TimeSpan.ParseExact(durationTexts[1], "m'm'",
                                        CultureInfo.InvariantCulture));
                            segment.Duration = duration;
                            return segment;
                        }).ToList();
                        return new RevalidateFareResult
                        {
                            IsSuccess = true,
                            IsValid = price == newPrice,
                            Itinerary = itin
                        };
                    }
                    else
                    {
                        return new RevalidateFareResult
                        {
                            IsSuccess = true,
                            IsValid = false,
                            Itinerary = null
                        };
                    }
                }
                catch
                {
                    return new RevalidateFareResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> {"Web Layout Changed!"}
                    };
                }
            }
        }
    }
}
