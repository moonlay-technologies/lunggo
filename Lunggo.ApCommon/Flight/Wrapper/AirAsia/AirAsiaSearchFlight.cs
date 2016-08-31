using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using RestSharp;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;
using FareType = Lunggo.ApCommon.Flight.Constant.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;
using System.Diagnostics;

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
                if (conditions.Trips.Count > 1)
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };

                var client = CreateCustomerClient();

                // Airport Generalizing
                var trip0 = new FlightTrip
                {
                    OriginAirport = conditions.Trips[0].OriginAirport,
                    DestinationAirport = conditions.Trips[0].DestinationAirport,
                    DepartureDate = conditions.Trips[0].DepartureDate
                };
                if (trip0.OriginAirport == "JKT")
                    trip0.OriginAirport = "CGK";
                if (trip0.DestinationAirport == "JKT")
                    trip0.DestinationAirport = "CGK";

                // [GET] Search Flight
                var originCountry = FlightService.GetInstance().GetAirportCountryCode(trip0.OriginAirport);
                CQ availableFares;
                CQ flightTable;
                string mataUang;

                switch (originCountry)
                {
                    case "ID":
                        mataUang = "IDR";
                        break;
                    case "MY":
                        mataUang = "MYR";
                        break;
                    case "SG":
                        mataUang = "SGD";
                        break;
                    case "TH":
                        mataUang = "THB";
                        break;
                    case "SA":
                        mataUang = "SAR";
                        break;
                    case "AU":
                        mataUang = "AUD";
                        break;
                    case "BN":
                        mataUang = "BWD";
                        break;
                    case "CN":
                        mataUang = "CNY";
                        break;
                    case "PH":
                        mataUang = "PHP";
                        break;
                    case "HK":
                        mataUang = "HKD";
                        break;
                    case "IN":
                        mataUang = "INR";
                        break;
                    case "JP":
                        mataUang = "JPY";
                        break;
                    case "KR":
                        mataUang = "KRW";
                        break;
                    case "MO":
                        mataUang = "MOP";
                        break;
                    case "NP":
                        mataUang = "NPR";
                        break;
                    case "OM":
                        mataUang = "OMR";
                        break;
                    case "NZ":
                        mataUang = "NZD";
                        break;
                    case "TW":
                        mataUang = "TWD";
                        break;
                    case "LK":
                        mataUang = "LKR";
                        break;
                    default:
                        mataUang = "USD";
                        break;
                }

                //if (originCountry == "ID")
                {
                    string url = @"Flight/Select";
                    var searchRequest = new RestRequest(url, Method.GET);
                    searchRequest.AddHeader("Referer", "http://www.airasia.com/id/id/home.page?cid=1");
                    searchRequest.AddQueryParameter("o1", trip0.OriginAirport);
                    searchRequest.AddQueryParameter("d1", trip0.DestinationAirport);
                    searchRequest.AddQueryParameter("dd1", trip0.DepartureDate.ToString("yyyy-MM-dd"));
                    searchRequest.AddQueryParameter("ADT", conditions.AdultCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("CHD", conditions.ChildCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("inl", conditions.InfantCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("s", "true");
                    searchRequest.AddQueryParameter("mon", "true");
                    searchRequest.AddQueryParameter("culture", "id-ID");
                    searchRequest.AddQueryParameter("cc", mataUang);
                    var searchResponse = client.Execute(searchRequest);

                    var html = searchResponse.Content;
                    Debug.Print(html.ToString());

                    if (searchResponse.ResponseUri.AbsolutePath != "/Flight/Select" && (searchResponse.StatusCode == HttpStatusCode.OK || searchResponse.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };
                    var searchedHtml = (CQ)html;
                    availableFares = searchedHtml[".radio-markets"];
                    flightTable = searchedHtml[".avail-table-detail-table"];
                }

                //else
                //{
                //    return new SearchFlightResult
                //    {
                //        IsSuccess = true,
                //        Itineraries = new List<FlightItinerary>()
                //    };
                //}

                // [Scrape]

                try
                {
                    List<string> fareIds;
                    switch (conditions.CabinClass)
                    {
                        case CabinClass.Economy:
                            fareIds = availableFares.Where(dom => dom.Id.Last() == '0').Select(dom => dom.Value).ToList();
                            break;
                        case CabinClass.Business:
                            fareIds = availableFares.Where(dom => dom.Id.Last() == '1').Select(dom => dom.Value).ToList();
                            break;
                        default:
                            fareIds = new List<string>();
                            break;
                    }
                    
                    var itins = new List<FlightItinerary>();
                    var fareIdPrefix = trip0.OriginAirport + "." + trip0.DestinationAirport + "." +
                                       trip0.DepartureDate.ToString("dd.MM.yyyy") + "." + conditions.AdultCount + "." +
                                       conditions.ChildCount + "." + conditions.InfantCount + "." +
                                       FlightService.GetInstance().ParseCabinClass(conditions.CabinClass) + ".";
                    foreach (var fareId in fareIds)
                    {
                        var url = @"Flight/PriceItinerary?SellKeys%5B%5D=" + HttpUtility.UrlEncode(fareId);
                        var fareRequest = new RestRequest(url, Method.GET);
                        fareRequest.AddHeader("Referer", "http://www.airasia.com/id/id/home.page?cid=1");
                        var itinHtml = (CQ)client.Execute(fareRequest).Content;
                        var price =
                            decimal.Parse(itinHtml[".section-total-display-price > span:first"].Text().Trim(' ', '\n'),
                                CultureInfo.CreateSpecificCulture("id-ID"));
                        var breakdownPrice = itinHtml["[data-accordion-id='priceFareTaxesFeesContent0']"].Single().ChildElements.ToList();
                        var adultPrice = 0M;
                        var childPrice = 0M;
                        var infantPrice = 0M;
                        try
                        {
                            var x = breakdownPrice[0].LastElementChild;
                            var y = x.InnerText.Trim();
                            adultPrice = decimal.Parse(breakdownPrice[0].LastElementChild.InnerText.Trim().Split(' ')[2], CultureInfo.CreateSpecificCulture("id-ID"));
                            childPrice = decimal.Parse(breakdownPrice[1].LastElementChild.InnerText.Trim().Split(' ')[2], CultureInfo.CreateSpecificCulture("id-ID"));
                            infantPrice = decimal.Parse(breakdownPrice[2].LastElementChild.InnerText.Trim().Split(' ')[2], CultureInfo.CreateSpecificCulture("id-ID"));
                        }
                        catch { }
                        var currency = itinHtml[".section-total-display-currency>span>strong"].Text().Substring(0,3);
                        var itinHtmlText = itinHtml.Text();
                        var isPscIncluded = itinHtmlText.Contains("Pajak Bandara") ||
                                            itinHtmlText.Contains("Biaya Layanan Penumpang") ||
                                            itinHtmlText.Contains("Airport Tax") ||
                                            itinHtmlText.Contains("Passenger Service Fee");
                        var segmentFareIds = fareId.Split('|').Last().Split('^');
                        var segments = new List<FlightSegment>();
                        var stops = Regex.Matches(fareId, "THRU").Count;
                        
                        foreach (var segmentFareId in segmentFareIds)
                        {
                            var splittedSegmentFareId = segmentFareId.Split('~').ToArray();
                            var segmentStop = 0;
                            if (splittedSegmentFareId[splittedSegmentFareId.Length - 1].Length != 0)
                            {
                                segmentStop = stops;
                            }
                            var deptTime = DateTime.Parse(splittedSegmentFareId[5]).AddHours(-(FlightService.GetInstance().GetAirportTimeZone(splittedSegmentFareId[4])));
                            var arrTime = DateTime.Parse(splittedSegmentFareId[7]).AddHours(-(FlightService.GetInstance().GetAirportTimeZone(splittedSegmentFareId[6])));
                            var duration = arrTime - deptTime;

                            var baggage = GetBaggage(splittedSegmentFareId[4], splittedSegmentFareId[6]);
                            var isBaggageIncluded = false;

                            if (baggage != null)
                            {
                                isBaggageIncluded = true;
                            }

                            segments.Add(new FlightSegment
                            {
                                AirlineCode = splittedSegmentFareId[0],
                                FlightNumber = splittedSegmentFareId[1].Trim(),
                                CabinClass = conditions.CabinClass,
                                AirlineType = AirlineType.Lcc,
                                Rbd = fareId.Split('~')[1],
                                DepartureAirport = splittedSegmentFareId[4],
                                DepartureTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[5]), DateTimeKind.Utc),
                                ArrivalAirport = splittedSegmentFareId[6],
                                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(splittedSegmentFareId[7]), DateTimeKind.Utc),
                                OperatingAirlineCode = splittedSegmentFareId[0],
                                Duration =  duration,
                                StopQuantity = 0,
                                IsMealIncluded = false,
                                IsPscIncluded = isPscIncluded,
                                BaggageCapacity = baggage,
                                IsBaggageIncluded = isBaggageIncluded
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
                            Price = new Price(),
                            AdultPricePortion = adultPrice * conditions.AdultCount / price,
                            ChildPricePortion = childPrice * conditions.ChildCount / price,
                            InfantPricePortion = infantPrice * conditions.InfantCount / price,
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

                        Currency curr;

                       var currencyList = Currency.GetAllCurrencies(Payment.Constant.Supplier.AirAsia);
                        if (!currencyList.TryGetValue(currency, out curr))
                        {
                            return new SearchFlightResult
                            {
                                IsSuccess = true,
                                Itineraries = new List<FlightItinerary>()
                            };
                        }
                        itin.Price.SetSupplier(price, new Currency(currency, Payment.Constant.Supplier.AirAsia));
                        itins.Add(itin);

                        //ambil row2nya (.fare-light-row)
                        var m = fareIds.IndexOf(fareId);
                        var rows = flightTable[m].ChildElements.ToList()[0].ChildElements.ToList();
                        //cek apa jumlah rownya sama dengan segmen
                        if (rows.Count > segments.Count)
                        {
                            for (var x = 1; x < rows.Count - 1; x++)
                            {
                                //ambil data kode airport nya dan jamnya
                                var deptArpt =
                                    rows[x].ChildElements.ToList()[1].
                                        ChildElements.ToList()[0].
                                            ChildElements.ToList()[1].InnerHTML.SubstringBetween(1, 4);
                                var arrvArpt =
                                    rows[x].ChildElements.ToList()[3].
                                        ChildElements.ToList()[0].
                                            ChildElements.ToList()[1].InnerHTML.SubstringBetween(1, 4);

                                DateTime depTimeStop;
                                DateTime arrTimeStop;

                                if (deptArpt == segments.ElementAt(0).ArrivalAirport)
                                {
                                    //STOP ADA DI SEGMEN 2
                                    var arrvHrAtStop = rows[x].ChildElements.ToList()[3].
                                            ChildElements.ToList()[0].
                                                ChildElements.ToList()[0].InnerHTML;
                                    var deptHrAtStop = rows[x + 1].ChildElements.ToList()[1].
                                            ChildElements.ToList()[0].
                                                ChildElements.ToList()[0].InnerHTML;

                                    var jambrg = segments.ElementAt(1).DepartureTime.Hour;
                                    var jamdtg = Convert.ToInt32(arrvHrAtStop.Split(':')[0]);

                                    if (jambrg > jamdtg)
                                    {
                                        //Kedatangan di stop udah beda hari dgn keberangkatan segmen
                                        var w = segments.ElementAt(1).DepartureTime.AddDays(1);
                                        arrTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }
                                    else
                                    {
                                        var w = segments.ElementAt(1).DepartureTime;
                                        arrTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }

                                    jambrg = Convert.ToInt32(deptHrAtStop.Split(':')[0]);
                                    jamdtg = segments.ElementAt(1).ArrivalTime.Hour;

                                    if (jambrg > jamdtg)
                                    {
                                        //Kedatangan di segmen1 udah beda hari dgn keberangkatan di stop
                                        var w = segments.ElementAt(1).ArrivalTime.AddDays(-1);
                                        depTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(deptHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(deptHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }
                                    else
                                    {
                                        var w = segments.ElementAt(1).ArrivalTime;
                                        depTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(deptHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(deptHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }

                                    var durationAtStop = depTimeStop - arrTimeStop;

                                    var segmentStops = new List<FlightStop>
                                    {
                                        new FlightStop
                                        {
                                            Airport = arrvArpt,
                                            ArrivalTime = arrTimeStop,
                                            Duration = durationAtStop,
                                            DepartureTime = depTimeStop                        
                                        }
                                    };
                                    itin.Trips[0].Segments.ElementAt(1).Stops = segmentStops;

                                }

                                if (arrvArpt == segments.ElementAt(0).ArrivalAirport)
                                        {
                                    //STOP ADA DI SEGMEN 1
                                    var arrvHrAtStop = rows[x - 1].ChildElements.ToList()[3].
                                             ChildElements.ToList()[0].
                                                 ChildElements.ToList()[0].InnerHTML;
                                    var deptHrAtStop = rows[x].ChildElements.ToList()[1].
                                            ChildElements.ToList()[0].
                                                ChildElements.ToList()[0].InnerHTML;

                                    var jambrg = segments.ElementAt(0).DepartureTime.Hour;
                                    var jamdtg = Convert.ToInt32(arrvHrAtStop.Split(':')[0]);

                                    if (jambrg > jamdtg)
                                    {
                                        //Kedatangan di stop udah beda hari dgn keberangkatan segmen
                                        var w = segments.ElementAt(0).DepartureTime.AddDays(1);
                                        arrTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }
                                    else
                                    {
                                        var w = segments.ElementAt(0).DepartureTime;
                                        arrTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(arrvHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }
                        
                                    jambrg = Convert.ToInt32(deptHrAtStop.Split(':')[0]);
                                    jamdtg = segments.ElementAt(0).ArrivalTime.Hour;

                                    if (jambrg > jamdtg)
                                    {

                                        var w = segments.ElementAt(0).ArrivalTime.AddDays(-1);
                                        depTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(deptHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(deptHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }
                                    else
                                    {
                                        var w = segments.ElementAt(0).ArrivalTime;
                                        depTimeStop = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                            Convert.ToInt32(deptHrAtStop.Split(':')[0]),
                                            Convert.ToInt32(deptHrAtStop.Split(':')[1]), 0), DateTimeKind.Utc);
                                    }

                                    var durationAtStop = depTimeStop - arrTimeStop;

                                    var segmentStops = new List<FlightStop>
                                    {
                                        new FlightStop
                                        {
                                            Airport = deptArpt,
                                            ArrivalTime = arrTimeStop,
                                            Duration = durationAtStop,
                                            DepartureTime = depTimeStop
                                        }
                                    };
                                    itin.Trips[0].Segments.ElementAt(0).Stops = segmentStops;
                                }
                            }
                        }
                        itins.Add(itin);
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
                var segmentDepartureAirports = segments.Select(s => s.DepartureAirport);
                var segmentArrivalAirports = segments.Select(s => s.ArrivalAirport);
                var segmentAirports = segmentDepartureAirports.Concat(segmentArrivalAirports);
                var segmentCountries = segmentAirports.Select(FlightService.GetInstance().GetAirportCountryCode).Distinct();
                return segmentCountries.Count() > 1;
            }
        }
    }
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

//var oriFareId = itin.FareId.Split('.').Last();
//var radio = availableFares.Single(fare => fare.Value == oriFareId).Cq();
//var fareRow = searchedHtml[".carrier-hover-header,.carrier-hover-return"].Children().Last();//radio.Parent().Parent().Parent().Parent().Parent();
//var durationRows = searchedHtml[".carrier-hover-oneway-header>div:last-child"];
//var durationForLayover = searchedHtml[".carrier-hover"].ToList();
//var durs = new List<IDomObject>();
//foreach (var childrow in durationForLayover.Select(row => row.ChildElements.ToList()))
//{
//    durs.AddRange((from child in childrow where child.ChildElements.ToList().Count == 3 select child.ChildElements.ToList()[2]).Cast<IDomObject>());
//}
//var flattenedSegments = itins.SelectMany(itin => itin.Trips[0].Segments).ToList();
//for (var i = 0; i < flattenedSegments.Count; i++)
//{
//    var durationRow = durationRows[i] ?? durs.ElementAt(i);
//    var segment = flattenedSegments[i];

//    var durationTexts =
//        durationRow.InnerHTML.Trim().Split(':').ToList();
//    var splitduration = durationTexts[1].Trim().Split(' ').ToList();
//    var duration = new TimeSpan();
//    duration =
//        duration.Add(TimeSpan.ParseExact(splitduration[0], "h'h'", CultureInfo.InvariantCulture));
//    if (splitduration.Count > 1)
//        duration =
//            duration.Add(TimeSpan.ParseExact(splitduration[1], "m'm'",
//                CultureInfo.InvariantCulture));
//    //segment.Duration = duration;
//};
