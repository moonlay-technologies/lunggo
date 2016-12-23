using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Web;
using RestSharp;

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
                var client = CreateCustomerClient();

                if (conditions.Itinerary.FareId == null)
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
                    var splittedFareId = conditions.Itinerary.FareId.Split('.').ToList();
                    origin = splittedFareId[0];
                    dest = splittedFareId[1];
                    date = new DateTime(int.Parse(splittedFareId[4]), int.Parse(splittedFareId[3]),
                        int.Parse(splittedFareId[2]));
                    adultCount = int.Parse(splittedFareId[5]);
                    childCount = int.Parse(splittedFareId[6]);
                    infantCount = int.Parse(splittedFareId[7]);
                    cabinClass = FlightService.GetInstance().ParseCabinClass(splittedFareId[8]);
                    price = decimal.Parse(splittedFareId[9]);
                    coreFareId = splittedFareId[10];
                }
                catch
                {
                    return new RevalidateFareResult {Errors = new List<FlightError> {FlightError.FareIdNoLongerValid}};
                }

                // [GET] Search Flight

                var originCountry = FlightService.GetInstance().GetAirportCountryCode(origin);
                var destinationCountry = FlightService.GetInstance().GetAirportCountryCode(dest);
                var searchedHtml = new CQ();
                if (originCountry == "ID")
                {
                    var url = @"Flight/Select";
                    var searchRequest = new RestRequest(url, Method.GET);
                    searchRequest.AddHeader("Referer", "http://www.airasia.com/id/id/home.page?cid=1");
                    searchRequest.AddQueryParameter("o1", origin);
                    searchRequest.AddQueryParameter("d1", dest);
                    searchRequest.AddQueryParameter("dd1", date.ToString("yyyy-MM-dd"));
                    searchRequest.AddQueryParameter("ADT", adultCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("CHD", childCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("inl", infantCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("s", "true");
                    searchRequest.AddQueryParameter("mon", "true");
                    searchRequest.AddQueryParameter("culture", "id-ID");
                    searchRequest.AddQueryParameter("cc", "IDR");
                    var searchResponse = client.Execute(searchRequest);

                    var html = searchResponse.Content;

                    if (searchResponse.ResponseUri.AbsolutePath != "/Flight/Select" && (searchResponse.StatusCode == HttpStatusCode.OK || searchResponse.StatusCode == HttpStatusCode.Redirect))
                        return new RevalidateFareResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[AirAsia] || " + searchResponse.Content}
                        };

                    searchedHtml = (CQ)html;
                }
                //else if (destinationCountry == "ID")
                //{
                //    var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                //              @"?o1=" + dest +
                //              @"&d1=" + origin +
                //              @"&dd1=" + date.ToString("yyyy-MM-dd") +
                //              @"&dd2=" + date.ToString("yyyy-MM-dd") +
                //              @"&ADT=" + adultCount +
                //              @"&CHD=" + childCount +
                //              @"&inl=" + infantCount +
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
                //        return new RevalidateFareResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };
                //
                //    searchedHtml = (CQ)html;
                //}
                else
                {
                    return new RevalidateFareResult
                    {
                        IsSuccess = true,
                        IsValid = false,
                        NewItinerary = null
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
                                           childCount + "." + infantCount + "." + FlightService.GetInstance().ParseCabinClass(cabinClass) + ".";

                        var url = @"Flight/PriceItinerary?SellKeys%5B%5D=" + HttpUtility.UrlEncode(foundFareId);
                        var fareRequest = new RestRequest(url, Method.GET);
                        fareRequest.AddHeader("Referer", "http://www.airasia.com/id/id/home.page?cid=1");
                        var itinHtml = (CQ)client.Execute(fareRequest).Content;

                        if (itinHtml.Children().Elements.Count() <= 1)
                            return new RevalidateFareResult
                            {
                                IsSuccess = true,
                                IsValid = false,
                                NewItinerary = null
                            };

                        var newPrice =
                            decimal.Parse(itinHtml[".section-total-display-price > span:first"].Text().Trim(' ', '\n'),
                                CultureInfo.CreateSpecificCulture("id-ID"));
                        var breakdownPrice = itinHtml["[data-accordion-id='priceFareTaxesFeesContent0']"].Single().ChildElements.ToList();
                        var adultPrice = 0M;
                        var childPrice = 0M;
                        var infantPrice = 0M;
                        try
                        {
                            adultPrice = decimal.Parse(breakdownPrice[0].LastElementChild.InnerText.Split(' ')[2], CultureInfo.CreateSpecificCulture("id-ID"));
                            childPrice = decimal.Parse(breakdownPrice[1].LastElementChild.InnerText.Split(' ')[2], CultureInfo.CreateSpecificCulture("id-ID"));
                            infantPrice = decimal.Parse(breakdownPrice[2].LastElementChild.InnerText.Split(' ')[2], CultureInfo.CreateSpecificCulture("id-ID"));
                        }
                        catch { }
                        var currency = itinHtml[".section-total-display-currency>span>strong"].Text();
                        var isPscIncluded =
                            itinHtml.Is(":contains('Pajak Bandara'), :contains('Biaya Layanan Penumpang')");
                        var segmentFareIds = foundFareId.Split('|').Last().Split('^');
                        var segments = new List<FlightSegment>();
                        var flight = FlightService.GetInstance();
                        foreach (var segmentFareId in segmentFareIds)
                        {
                            var splittedSegmentFareId = segmentFareId.Split('~').ToArray();
                            var deptTime = DateTime.Parse(splittedSegmentFareId[5]).AddHours(-(flight.GetAirportTimeZone(splittedSegmentFareId[4])));
                            var arrTime = DateTime.Parse(splittedSegmentFareId[7]).AddHours(-(flight.GetAirportTimeZone(splittedSegmentFareId[6])));
                            segments.Add(new FlightSegment
                            {
                                AirlineCode = splittedSegmentFareId[0],
                                FlightNumber = splittedSegmentFareId[1].Trim(),
                                CabinClass = cabinClass,
                                AirlineType = AirlineType.Lcc,
                                Rbd = foundFareId.Split('~')[1],
                                DepartureAirport = splittedSegmentFareId[4],
                                DepartureTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[5]), DateTimeKind.Utc),
                                ArrivalAirport = splittedSegmentFareId[6],
                                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[7]),DateTimeKind.Utc),
                                OperatingAirlineCode = splittedSegmentFareId[0],
                                Duration = arrTime-deptTime,
                                StopQuantity = 0,
                                IsMealIncluded = false,
                                IsPscIncluded = isPscIncluded
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
                            RequirePassport = RequirePassport(segments),
                            RequireSameCheckIn = false,
                            RequireNationality = true,
                            RequestedCabinClass = CabinClass.Economy,
                            RequestedTripType = conditions.Itinerary.RequestedTripType,
                            TripType = TripType.OneWay,
                            Supplier = Supplier.AirAsia,
                            Price = new Price(),
                            AdultPricePortion = adultPrice/newPrice,
                            ChildPricePortion = childPrice/newPrice,
                            InfantPricePortion = infantPrice/newPrice,
                            FareId = fareIdPrefix + newPrice.ToString("0") + "." + foundFareId,
                            Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = origin,
                                    DestinationAirport = dest,
                                    DepartureDate = DateTime.SpecifyKind(date,DateTimeKind.Utc),
                                    Segments = segments
                                }
                            }
                        };
                        itin.Price.SetSupplier(newPrice, new Currency(currency));

                        /*var durationRows = searchedHtml[".carrier-hover-oneway-header>div:last-child"];
                        itin.Trips[0].Segments = segments.Zip(durationRows, (segment, durationRow) =>
                        {
                            var durationTexts =
                                durationRow.InnerHTML.Trim().Split(':').ToList();
                            var splitduration = durationTexts[1].Trim().Split(' ').ToList();
                            var duration = new TimeSpan();
                            duration =
                                duration.Add(TimeSpan.ParseExact(splitduration[0], "h'h'", CultureInfo.InvariantCulture));
                            if (splitduration.Count > 1)
                                duration =
                                    duration.Add(TimeSpan.ParseExact(splitduration[1], "m'm'",
                                        CultureInfo.InvariantCulture));
                            segment.Duration = duration;
                            return segment;
                        }).ToList();*/
                        var result = new RevalidateFareResult
                        {
                            IsSuccess = true,
                            IsValid = true,
                            IsPriceChanged = price != newPrice,
                            NewItinerary = itin,
                            IsItineraryChanged = !conditions.Itinerary.Identical(itin)
                        };
                        if (result.IsPriceChanged)
                            result.NewPrice = newPrice;
                        return result;
                    }
                    else
                    {
                        return new RevalidateFareResult
                        {
                            IsSuccess = true,
                            IsValid = false,
                            NewItinerary = null
                        };
                    }
                }
                catch
                {
                    return new RevalidateFareResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> { "[AirAsia] Web Layout Changed!" }
                    };
                }
            }
        }
    }
}
