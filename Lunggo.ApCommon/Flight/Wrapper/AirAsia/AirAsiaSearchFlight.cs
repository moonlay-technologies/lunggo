using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Web;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;
using FareType = Lunggo.ApCommon.Flight.Constant.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            return Client.SearchFlight(conditions);
        }

        private partial class AirAsiaClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                var client = new ExtendedWebClient();

                // Airport Generalizing
                if (conditions.Trips[0].OriginAirport == "JKT")
                    conditions.Trips[0].OriginAirport = "CGK";
                if (conditions.Trips[0].DestinationAirport == "JKT")
                    conditions.Trips[0].DestinationAirport = "CGK";

                // [GET] Search Flight
                var trip0 = conditions.Trips[0];
                var dict = DictionaryService.GetInstance();
                var originCountry = dict.GetAirportCountryCode(trip0.OriginAirport);
                var destinationCountry = dict.GetAirportCountryCode(trip0.DestinationAirport);
                var availableFares = new CQ();
                if (originCountry == "ID")
                {
                    var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                              @"?o1=" + trip0.OriginAirport +
                              @"&d1=" + trip0.DestinationAirport +
                              @"&dd1=" + trip0.DepartureDate.ToString("yyyy-MM-dd") +
                              @"&ADT=" + conditions.AdultCount +
                              @"&CHD=" + conditions.ChildCount +
                              @"&inl=" + conditions.InfantCount +
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

                    if (client.ResponseUri.AbsolutePath != "/Flight/Select" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };

                    var searchedHtml = (CQ)html;
                    availableFares = searchedHtml[".radio-markets"];
                }
                //else if (destinationCountry == "ID")
                //{
                //    var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                //              @"?o1=" + trip0.DestinationAirport +
                //              @"&d1=" + trip0.OriginAirport +
                //              @"&dd1=" + trip0.DepartureDate.ToString("yyyy-MM-dd") +
                //              @"&dd2=" + trip0.DepartureDate.ToString("yyyy-MM-dd") +
                //              @"&ADT=" + conditions.AdultCount +
                //              @"&CHD=" + conditions.ChildCount +
                //              @"&inl=" + conditions.InfantCount +
                //              @"&r=true" +
                //              @"&s=true" +
                //              @"&mon=true" +
                //              @"&culture=id-ID" +
                //              @"&cc=IDR";
                //    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                //    client.Headers["Upgrade-Insecure-Requests"] = "1";
                //    client.Headers["User-Agent"] =
                //        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                //    client.Headers["Origin"] = "https://booking2.airasia.com";
                //    client.Headers["Referer"] = "https://booking2.airasia.com/Payment.aspx";
                //    var html = client.DownloadString(url);
                //
                //    if (client.ResponseUri.AbsolutePath != "/Flight/Select" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                //        return new SearchFlightResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };
                //
                //    var searchedHtml = (CQ)html;
                //    availableFares = searchedHtml[".js_availability_container:nth-child(2) .radio-markets"];
                //}
                else
                {
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };
                }
                
                // [Scrape]

                try
                {
                    
                    IEnumerable<string> fareIds;
                    switch (conditions.CabinClass)
                    {
                        case CabinClass.Economy:
                            fareIds = availableFares.Where(dom => dom.Id.Last() == '0').Select(dom => dom.Value);
                            break;
                        case CabinClass.Business:
                            fareIds = availableFares.Where(dom => dom.Id.Last() == '1').Select(dom => dom.Value);
                            break;
                        default:
                            fareIds = new List<string>();
                            break;
                    }
                    var itins = new List<FlightItinerary>();
                    var fareIdPrefix = trip0.OriginAirport + "." + trip0.DestinationAirport + "." +
                                       trip0.DepartureDate.ToString("dd.MM.yyyy") + "." + conditions.AdultCount + "." +
                                       conditions.ChildCount + "." + conditions.InfantCount + "." +
                                       FlightService.ParseCabinClass(conditions.CabinClass) + ".";
                    foreach (var fareId in fareIds)
                    {
                        var url = "https://booking.airasia.com/Flight/PriceItinerary" +
                              "?SellKeys%5B%5D=" + HttpUtility.UrlEncode(fareId);
                        var itinHtml = (CQ) client.DownloadString(url);
                        var price =
                            decimal.Parse(itinHtml[".section-total-display-price > span:first"].Text().Trim(' ', '\n'),
                                CultureInfo.CreateSpecificCulture("id-ID"));
                        var segmentFareIds = fareId.Split('|').Last().Split('^');
                        var segments = new List<FlightSegment>();
                        foreach (var segmentFareId in segmentFareIds)
                        {
                            var splittedSegmentFareId = segmentFareId.Split('~').ToArray();
                            segments.Add(new FlightSegment
                            {
                                AirlineCode = splittedSegmentFareId[0],
                                FlightNumber = splittedSegmentFareId[1].Trim(),
                                CabinClass = conditions.CabinClass,
                                Rbd = fareId.Split('~')[1],
                                DepartureAirport = splittedSegmentFareId[4],
                                DepartureTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[5]),DateTimeKind.Utc),
                                ArrivalAirport = splittedSegmentFareId[6],
                                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[7]),DateTimeKind.Utc),
                                OperatingAirlineCode = splittedSegmentFareId[0],
                                StopQuantity = 0
                            });
                        }
                        var itin = new FlightItinerary
                        {
                            AdultCount = conditions.AdultCount,
                            ChildCount = conditions.ChildCount,
                            InfantCount = conditions.InfantCount,
                            CanHold = true,
                            FareType = FareType.Published,
                            RequireBirthDate = true,
                            RequirePassport = RequirePassport(segments),
                            RequireSameCheckIn = false,
                            RequireNationality = true,
                            RequestedCabinClass = CabinClass.Economy,
                            TripType = TripType.OneWay,
                            Supplier = Supplier.AirAsia,
                            SupplierCurrency = "IDR",
                            SupplierPrice = price,
                            FareId = fareIdPrefix + price.ToString("0") + "." + fareId,
                            Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = trip0.OriginAirport,
                                    DestinationAirport = trip0.DestinationAirport,
                                    DepartureDate = DateTime.SpecifyKind(trip0.DepartureDate, DateTimeKind.Utc),
                                    Segments = segments
                                }
                            }
                        };
                        itins.Add(itin);
                    }
                    foreach (var itin in itins)
                    {
                        var oriFareId = itin.FareId.Split('.').Last();
                        var radio = availableFares.Single(fare => fare.Value == oriFareId).Cq();
                        var fareRow = radio.Parent().Parent().Parent().Parent().Parent();
                        var durationRows = fareRow.Children().First().MakeRoot()["tr:even"];
                        var segments = itin.Trips[0].Segments;
                        var newSegments = segments.Zip(durationRows, (segment, durationRow) =>
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
                        itin.Trips[0].Segments = newSegments;
                    }
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = itins
                    };
                }
                catch
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Web Layout Changed!" }
                    };
                }
            }

            private bool RequirePassport(List<FlightSegment> segments)
            {
                var dict = DictionaryService.GetInstance();
                var segmentDepartureAirports = segments.Select(s => s.DepartureAirport);
                var segmentArrivalAirports = segments.Select(s => s.ArrivalAirport);
                var segmentAirports = segmentDepartureAirports.Concat(segmentArrivalAirports);
                var segmentCountries = segmentAirports.Select(dict.GetAirportCountryCode).Distinct();
                return segmentCountries.Count() > 1;
            }
        }
    }
}
