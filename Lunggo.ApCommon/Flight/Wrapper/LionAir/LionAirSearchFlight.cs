using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.UI;
using Newtonsoft.Json;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using RestSharp;
//using RestSharp.Deserializers;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;
using FareType = Lunggo.ApCommon.Flight.Constant.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            return Client.SearchFlight(conditions);
        }

        private partial class LionAirClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                var client = CreateCustomerClient();

                if (conditions.AdultCount == 0)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"There must be at least one adult passenger"}
                    };
                }
                if (conditions.AdultCount + conditions.ChildCount > 7)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Total adult and children passenger must be not more than seven"}
                    };
                }
                if (conditions.AdultCount < conditions.InfantCount)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Every infant must be accompanied by one adult"}
                    };
                }
                if (conditions.Trips[0].DepartureDate > DateTime.Now.AddDays(331).Date)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Booking is allowed to max 331 days before the departure date"}
                    };
                }

                // Airport Generalizing
                var trip0 = new FlightTrip
                {
                    OriginAirport = conditions.Trips[0].OriginAirport,
                    DestinationAirport = conditions.Trips[0].DestinationAirport,
                    DepartureDate = conditions.Trips[0].DepartureDate
                };

                var originAirport = trip0.OriginAirport == "JKT" ? "CGK" : trip0.OriginAirport;
                    
                var destinationAirport = trip0.DestinationAirport == "JKT" ? "CGK" : trip0.DestinationAirport;

                // [GET] Search Flight
                var dict = DictionaryService.GetInstance();
                var originCountry = dict.GetAirportCountryCode(trip0.OriginAirport);
                CQ availableFares;
                CQ departureDate;
                string scr;
                var depDateText = "";

                if (originCountry == "ID")
                {
                    // Calling The Zeroth Page
                    client.BaseUrl = new Uri("http://www.lionair.co.id");
                    const string url0 = @"Default.aspx";
                    var searchRequest0 = new RestRequest(url0, Method.GET);
                    searchRequest0.AddHeader("Referer", "http://www.lionair.co.id");

                    var searchResponse0 = client.Execute(searchRequest0);

                    if (searchResponse0.ResponseUri.AbsolutePath != "/Default.aspx" &&
                        (searchResponse0.StatusCode == HttpStatusCode.OK ||
                         searchResponse0.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};

                    // Calling The First Page
                    client.BaseUrl = new Uri("https://secure2.lionair.co.id");
                    const string url = @"lionairibe2/OnlineBooking.aspx";
                    var searchRequest = new RestRequest(url, Method.GET);
                    searchRequest.AddHeader("Referer", "http://www.lionair.co.id");
                    searchRequest.AddQueryParameter("trip_type", "one way");
                    searchRequest.AddQueryParameter("date_flexibility", "fixed");
                    searchRequest.AddQueryParameter("depart", originAirport);
                    searchRequest.AddQueryParameter("dest.1", destinationAirport);
                    searchRequest.AddQueryParameter("date.0", trip0.DepartureDate.ToString("ddMMM"));
                    searchRequest.AddQueryParameter("date.1", trip0.DepartureDate.ToString("ddMMM"));
                    searchRequest.AddQueryParameter("persons.0",
                        conditions.AdultCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("persons.1",
                        conditions.ChildCount.ToString(CultureInfo.InvariantCulture));
                    searchRequest.AddQueryParameter("persons.2",
                        conditions.InfantCount.ToString(CultureInfo.InvariantCulture));
                    //searchRequest.AddQueryParameter("origin", "EN");
                    //searchRequest.AddQueryParameter("usercountry", "ID");

                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                           SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                    var searchResponse = client.Execute(searchRequest);

                    if (searchResponse.ResponseUri.AbsolutePath != "/lionairibe2/OnlineBooking.aspx" &&
                        (searchResponse.StatusCode == HttpStatusCode.OK ||
                         searchResponse.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};

                    //Calling The Second Page
                    const string url2 = @"lionairibe2/OnlineBooking.aspx";
                    var searchRequest2 = new RestRequest(url2, Method.GET);
                    searchRequest2.AddHeader("Referer", "https://secure2.lionair.co.id");

                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                           SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                    var searchResponse2 = client.Execute(searchRequest2);
                    var html2 = searchResponse2.Content;

                    if (searchResponse2.ResponseUri.AbsolutePath != "/lionairibe2/OnlineBooking.aspx" &&
                        (searchResponse2.StatusCode == HttpStatusCode.OK ||
                         searchResponse2.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};

                    var searchedHtml = (CQ) html2;
                    //Getting rows of all flights
                    availableFares = searchedHtml[".flight-matrix-row"];

                    //Getting departure date in DD MMM YYYY format (23 Jun 2016)
                    departureDate = searchedHtml[
                        ".box-content.searchdetails .row .col-md-6.col-sm-12.col-xs-12.border-right.rel-pos .row " +
                        ".col-md-6.col-sm-6.col-xs-6.pr20>p>label"].Text();
                    var departureDateText = departureDate.Text().Trim(' ');
                    var departureDateText1 = departureDateText.Trim('\n').Trim(' ').Split(' ');
                    depDateText = String.Join(" ", departureDateText1.Skip(1));

                    //Getting price formula script
                    var pageScript = html2;
                    var startIndex = pageScript.IndexOf("{'fares'");
                    var endIndex = pageScript.IndexOf("})");
                    scr = pageScript.Substring(startIndex, endIndex + 1 - startIndex);

                    var x = availableFares.Length;
                    var y = availableFares.Count();
                }
                else
                {
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };
                }

                var pricefunc = new GetLionAirPrice(scr);
                try
                {
                    //var retidx = 0;
                    var fareIds = new List<IDomObject>();
                    //var index = new List<int>();
                    switch (conditions.CabinClass)
                    {
                        case CabinClass.Economy:
                        {
                            var j = 0;
                            var it = 0;
                            
                            while (it < availableFares.Length)
                            {
                                if (availableFares[it].ChildElements.ToList().Count == 4)
                                {
                                    if ((availableFares[it].ChildElements.ToList()[1].GetAttribute("class") !=
                                         "flight-class sold-flight" &&
                                         availableFares[it].ChildElements.ToList()[1].GetAttribute("class") !=
                                         "flight-class not-available")
                                        || (availableFares[it].ChildElements.ToList()[2].GetAttribute("class") !=
                                         "flight-class sold-flight" &&
                                         availableFares[it].ChildElements.ToList()[2].GetAttribute("class") !=
                                         "flight-class not-available"))
                                    {
                                        fareIds.Add(availableFares[it]);
                                        j = it + 1;
                                        while ((j != availableFares.Count()) &&
                                               (availableFares[j - 1].GetAttribute("id").SubstringBetween(0, (availableFares[j - 1].GetAttribute("id").Length - 2))
                                                == availableFares[j].GetAttribute("id").SubstringBetween(0, availableFares[j].GetAttribute("id").Length - 2))
                                            )
                                        {
                                            fareIds.Add(availableFares[j]);
                                            j += 1;
                                        }
                                    }
                                    else
                                    {
                                        j = it + 1;
                                        //Skipping the second etc rows after getting the first row, because price is 1st row
                                        while ((j != availableFares.Count()) &&
                                               (availableFares[j - 1].GetAttribute("id")
                                                   .SubstringBetween(0,
                                                       (availableFares[j - 1].GetAttribute("id").Length - 2))
                                                ==
                                                availableFares[j].GetAttribute("id")
                                                    .SubstringBetween(0, availableFares[j].GetAttribute("id").Length - 2))
                                               && j != availableFares.Count())
                                        {
                                            j += 1;
                                        }
                                    }
                                }
                                it = j;
                            }
                        }
                            break;
                        case CabinClass.Business:
                        {
                            var j = 0;
                            var it = 0;
                            while (it < availableFares.Count())
                            {
                                if (availableFares[it].ChildElements.ToList().Count == 4)
                                {
                                    if ((availableFares[it].ChildElements.ToList()[3].GetAttribute("class") !=
                                         "flight-class sold-flight" &&
                                         availableFares[it].ChildElements.ToList()[3].GetAttribute("class") !=
                                         "flight-class not-available"))
                                    {
                                        fareIds.Add(availableFares[it]);
                                        j = it + 1;
                                            
                                        while ((j != availableFares.Count()) &&
                                                (availableFares[j - 1].GetAttribute("id")
                                                    .SubstringBetween(0,
                                                        (availableFares[j - 1].GetAttribute("id").Length - 2))
                                                == availableFares[j].GetAttribute("id").SubstringBetween(0,availableFares[j].GetAttribute("id").Length - 2)))
                                        {
                                            fareIds.Add(availableFares[j]);
                                            j += 1;
                                        }
                                        
                                    }
                                    else
                                    {
                                        j = it + 1;
                                        while ((j != availableFares.Count()) &&
                                               (availableFares[j - 1].GetAttribute("id")
                                                   .SubstringBetween(0,
                                                       (availableFares[j - 1].GetAttribute("id").Length - 2))
                                                ==
                                                availableFares[j].GetAttribute("id")
                                                    .SubstringBetween(0, availableFares[j].GetAttribute("id").Length - 2))
                                               && j != availableFares.Count())
                                        {
                                            j += 1;
                                        }
                                    }
                                }
                                it = j;
                            }
                        }
                            break;
                        default:
                            fareIds = new List<IDomObject>();
                            break;
                    }

                    var itins = new List<FlightItinerary>();
                    
                    for (var ind = 0; ind < fareIds.Count; ind++)
                    {
                        //For index 0 or all segment 1
                        var segments = new List<FlightSegment>();
                        var cityArrival2 = "";
                        var airportArrival2 = "";
                        //var cityDeparture2 = "";
                        var listFlightNo = new List<string>();
                        var listDepHr = new List<string>();
                        var subst1 = fareIds.ElementAt(ind).GetAttribute("id");
                        var dicty = DictionaryService.GetInstance();
                        if (ind == 0 ||
                            (subst1.SubstringBetween(0, subst1.Length - 2) !=
                             fareIds.ElementAt(ind - 1)
                                 .GetAttribute("id")
                                 .SubstringBetween(0, fareIds.ElementAt(ind - 1).GetAttribute("id").Length - 2)))
                        {
                            // Column 0a (Departure Data)
                            var departureInfo = fareIds.ElementAt(ind).ChildElements.ToList()[0].ChildElements.ToList()[0];
                            var airportDeparture = departureInfo.ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                            var cityDeparture = dicty.GetAirportCity(airportDeparture);
                            var timeDeparture = departureInfo.ChildElements.ToList()[1].InnerText;
                            listDepHr.Add(timeDeparture);
                            var flightNo = departureInfo.ChildElements.ToList()[2].InnerText.TrimEnd(' ');
                            listFlightNo.Add(flightNo);
                            var airplaneName = departureInfo.ChildElements.ToList()[2].ChildElements.ToList()[0].InnerText;

                            const string format = "dd MMM yyyy HH:mm";
                            var provider = CultureInfo.InvariantCulture;
                            var depDates = depDateText + " " + timeDeparture; // 28 Jan 2016 10:30 //
                            var depDate = DateTime.SpecifyKind(DateTime.ParseExact(depDates, format, provider), DateTimeKind.Utc);
                            
                            // Column 0c (Arrival Data)

                            var arrivalInfo = fareIds.ElementAt(ind).ChildElements.ToList()[0].ChildElements.ToList()[2];
                            var airportArrival = arrivalInfo.ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                            var cityArrival = dicty.GetAirportCity(airportArrival);
                            var timeArrival = arrivalInfo.ChildElements.ToList()[1].InnerText;
                            var duration = arrivalInfo.ChildElements.ToList()[2].InnerText.Split(' ');
                            var durHour = Int32.Parse(duration[1].SubstringBetween(0, 1));
                            var durMin = Int32.Parse(duration[2].SubstringBetween(0, 2));
                            var dur = new TimeSpan(0, durHour, durMin, 0, 0);

                            var jamdatang = Convert.ToInt32(timeArrival.Split(':')[0]);
                            var jambrgkt = Convert.ToInt32(timeDeparture.Split(':')[0]);
                            DateTime arrDate;
                            if (jamdatang < jambrgkt)
                            {
                                var x = depDate.AddDays(1);
                                arrDate = DateTime.SpecifyKind(new DateTime(x.Year, x.Month, x.Day,
                                    Convert.ToInt32(timeArrival.Split(':')[0]),
                                    Convert.ToInt32(timeArrival.Split(':')[1]), 0), DateTimeKind.Utc);
                            }
                            else
                            {
                                arrDate = DateTime.SpecifyKind(new DateTime(depDate.Year, depDate.Month, depDate.Day,
                                    Convert.ToInt32(timeArrival.Split(':')[0]),
                                    Convert.ToInt32(timeArrival.Split(':')[1]), 0), DateTimeKind.Utc);
                            }

                            
                            //Calculate Price

                            string priceId; //FR00_C0_SLOT0
                            if (conditions.CabinClass == CabinClass.Economy)
                            {
                                if (fareIds.ElementAt(ind).ChildElements.ToList()[1].InnerText != "Sold Out" &&
                                    fareIds.ElementAt(ind).ChildElements.ToList()[1].InnerText != "N/A")
                                {
                                    priceId = fareIds.ElementAt(ind).ChildElements.ToList()[1].GetAttribute("id");
                                }
                                else
                                {
                                    priceId = fareIds.ElementAt(ind).ChildElements.ToList()[2].GetAttribute("id");
                                }
                            }
                            else
                            {
                                priceId = fareIds.ElementAt(ind).ChildElements.ToList()[3].GetAttribute("id");
                            }

                            pricefunc.SetId(priceId);
                            decimal price = pricefunc.GetPrice(conditions.AdultCount, conditions.ChildCount,
                                conditions.InfantCount);

                            segments.Add(new FlightSegment
                            {
                                AirlineCode = flightNo.Split(' ')[0],
                                FlightNumber = flightNo.Split(' ')[1],
                                CabinClass = conditions.CabinClass,
                                DepartureAirport = airportDeparture,
                                DepartureTime = depDate, //ASK
                                ArrivalAirport = airportArrival,
                                ArrivalTime = arrDate, //ASK
                                OperatingAirlineCode = flightNo.Split(' ')[0],
                                //StopQuantity = Convert.ToInt32(stopNo),
                                Duration = dur,
                                //AircraftCode = aircraftNo,
                                DepartureCity = cityDeparture,
                                ArrivalCity = cityArrival,
                                AirlineName = airplaneName,
                                OperatingAirlineName = airplaneName,
                                
                            });
                            var j = ind + 1;
                            while ((j != fareIds.Count) && (subst1.SubstringBetween(0, subst1.Length - 2) ==
                                                            fareIds.ElementAt(j).GetAttribute("id").SubstringBetween(0,
                                                                    fareIds.ElementAt(ind + 1).GetAttribute("id").Length -2)))
                            {
                                // Column 0.a (Departure)
                                departureInfo = fareIds.ElementAt(j).ChildElements.ToList()[0].ChildElements.ToList()[0];
                                var airportDeparture2 = departureInfo.ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                                var cityDeparture2 = dicty.GetAirportCity(airportDeparture2);
                                var timeDeparture2 = departureInfo.ChildElements.ToList()[1].InnerText;
                                listDepHr.Add(timeDeparture2);
                                flightNo = departureInfo.ChildElements.ToList()[2].InnerText.TrimEnd(' ');
                                listFlightNo.Add(flightNo);
                                airplaneName = departureInfo.ChildElements.ToList()[2].ChildElements.ToList()[0].InnerText;

                                jamdatang = Convert.ToInt32(timeArrival.Split(':')[0]);
                                jambrgkt = Convert.ToInt32(timeDeparture2.Split(':')[0]);
                                DateTime depDate2;
                                if (jambrgkt < jamdatang)
                                {
                                    DateTime x = arrDate.AddDays(1);
                                    depDate2 = DateTime.SpecifyKind(new DateTime(x.Year, x.Month, x.Day,
                                        Convert.ToInt32(timeDeparture2.Split(':')[0]),
                                        Convert.ToInt32(timeDeparture2.Split(':')[1]), 0), DateTimeKind.Utc);
                                }
                                else
                                {
                                    depDate2 = DateTime.SpecifyKind(new DateTime(arrDate.Year, arrDate.Month, arrDate.Day,
                                        Convert.ToInt32(timeDeparture2.Split(':')[0]),
                                        Convert.ToInt32(timeDeparture2.Split(':')[1]), 0), DateTimeKind.Utc);
                                }

                                // Column 0.c (Arrival)

                                arrivalInfo = fareIds.ElementAt(j).ChildElements.ToList()[0].ChildElements.ToList()[2];
                                airportArrival2 = arrivalInfo.ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                                cityArrival2 = dicty.GetAirportCity(airportArrival2);
                                var timeArrival2 = arrivalInfo.ChildElements.ToList()[1].InnerText;
                                duration = arrivalInfo.ChildElements.ToList()[2].InnerText.Split(' ');
                                durHour = Int32.Parse(duration[1].SubstringBetween(0, 1));
                                durMin = Int32.Parse(duration[2].SubstringBetween(0, 2));
                                dur = new TimeSpan(0, durHour, durMin, 0, 0);

                                jamdatang = Convert.ToInt32(timeArrival2.Split(':')[0]);
                                jambrgkt = Convert.ToInt32(timeDeparture2.Split(':')[0]);

                                DateTime arrDate2;
                                if (jamdatang < jambrgkt)
                                {
                                    var x = depDate2.AddDays(1);
                                    arrDate2 = DateTime.SpecifyKind(new DateTime(x.Year, x.Month, x.Day,
                                        Convert.ToInt32(timeArrival2.Split(':')[0]),
                                        Convert.ToInt32(timeArrival2.Split(':')[1]), 0), DateTimeKind.Utc);
                                }
                                else
                                {
                                    arrDate2 = DateTime.SpecifyKind(new DateTime(depDate2.Year, depDate2.Month, depDate2.Day,
                                        Convert.ToInt32(timeArrival2.Split(':')[0]),
                                        Convert.ToInt32(timeArrival2.Split(':')[1]), 0), DateTimeKind.Utc);
                                }

                                arrDate = arrDate2;
                                timeArrival = timeArrival2;

                                segments.Add(new FlightSegment
                                {
                                    AirlineCode = flightNo.Split(' ')[0],
                                    FlightNumber = flightNo.Split(' ')[1],
                                    CabinClass = conditions.CabinClass,
                                    DepartureAirport = airportDeparture2,
                                    DepartureTime =
                                        depDate2,
                                    ArrivalAirport = airportArrival2,
                                    ArrivalTime = arrDate2,
                                    OperatingAirlineCode = flightNo.Split(' ')[0],
                                    //StopQuantity = Int32.Parse(stopNo),
                                    Duration = dur,
                                    //AircraftCode = aircraftNo,
                                    DepartureCity = cityDeparture2,
                                    ArrivalCity = cityArrival2,
                                    AirlineName = airplaneName,
                                    OperatingAirlineName = airplaneName,
                                });
                                j += 1;
                            }

                            var depHrJoin = String.Join("|", listDepHr.ToArray());
                            var flightNoJoin = String.Join("|", listFlightNo.ToArray());

                            string lastDest;
                            string lastAirport;
                            if (segments.Count > 1)
                            {
                                lastDest = cityArrival2; lastAirport = airportArrival2;
                            }
                            else
                            {
                                lastDest = cityArrival; lastAirport = airportArrival;
                            }
                                
                            var importantData = originAirport + "+"
                                                   + destinationAirport + "+"
                                                   + depDateText + "+"
                                                   + conditions.AdultCount + "+"
                                                   + conditions.ChildCount + "+"
                                                   + conditions.InfantCount + "+"
                                                   + FlightService.ParseCabinClass(conditions.CabinClass) + "+"
                                                   + price + "+" + priceId.SubstringBetween(21, priceId.Length) + "+" +
                                                   + segments.Count + "+" + flightNoJoin + "+" + depHrJoin;

                            var itin = new FlightItinerary
                            {
                                AdultCount = conditions.AdultCount,
                                ChildCount = conditions.ChildCount,
                                InfantCount = conditions.InfantCount,
                                CanHold = true,
                                FareType = FareType.Published,
                                RequireBirthDate = false,
                                RequirePassport = RequirePassport(segments),
                                RequireSameCheckIn = false,
                                RequireNationality = true,
                                RequestedCabinClass = conditions.CabinClass,
                                TripType = TripType.OneWay,
                                Supplier = Supplier.LionAir,
                                SupplierCurrency = "IDR",
                                SupplierPrice = price,
                                FareId = importantData,
                                Trips = new List<FlightTrip>
                                {
                                    new FlightTrip
                                    {
                                        OriginAirport = airportDeparture,
                                        DestinationAirport = lastAirport,
                                        DepartureDate = depDate,
                                        DestinationCity = lastDest,
                                        OriginCity = cityDeparture,
                                        Segments = segments
                                    }
                                }
                            };
                            itins.Add(itin);
                        }
                    }

                    //itins = itins.Where(itin => !itin.Trips[0].Segments.Exists(seg => seg.AirlineCode == "ID")).ToList();
                    itins = itins.Where(itin => !itin.Trips[0].Segments.Exists(seg => seg.AirlineCode == "OD")).ToList();
                    itins = itins.Where(itin => !itin.Trips[0].Segments.Exists(seg => seg.AirlineCode == "SL")).ToList();
                    if (trip0.DestinationAirport != "JKT")
                    {
                        itins = itins.Where(itin => itin.Trips[0].Segments.Last().ArrivalAirport == trip0.DestinationAirport).ToList();
                    }

                    if (trip0.OriginAirport != "JKT")
                    {
                        itins = itins.Where(itin => itin.Trips[0].Segments.First().DepartureAirport == trip0.OriginAirport).ToList();
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
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> {"Web Layout Changed!"}
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

            private class GetLionAirPrice
            {
                private readonly dynamic _priceScript;
                private string _id;

                public void SetId(string id)
                {
                    _id = id.SubstringBetween(21, id.Length);
                }

                public GetLionAirPrice(string scr)
                {
                    _priceScript = JsonConvert.DeserializeObject(scr, typeof (object));
                }

                private object WorkOutTripTotals()
                {

                    //GET WHICH FARE USED
                    var codeFlight = ParseId(_id);
                    var id1 = "";
                    var id2 = "";
                    /*var outId = "";
                    var inId = "";*/
                    if (codeFlight[0] == 0)
                    {
                        //outId = Id;
                        id1 = _id;
                    }
                    else
                    {
                        //inId = Id;
                        id2 = _id;
                    }

                    return SearchForBreakdown(id1, id2);
                }

                private static List<int> ParseId(string id)
                {
                    var codeFlight = new List<int>();

                    var splitId = id.Split('_');
                    if (splitId.Length == 1 && splitId[0] == "")
                    {
                        codeFlight.Add(1000);
                    }
                    else
                    {
                        codeFlight.Add(Convert.ToInt32(splitId[0].Substring(2)));
                        codeFlight.Add(Convert.ToInt32(splitId[1].Substring(1)));
                        codeFlight.Add(Convert.ToInt32(splitId[2].Substring(4)));
                    }
                    return codeFlight;
                }

                private object SearchForBreakdown(string outId, string inId)
                {
                   var ps = _priceScript.fares;
                    if (ps.Count == 0)
                    {
                        return null;
                    }

                    var noOutId = ParseId(outId);
                    var noInId = ParseId(inId);
                    if (noOutId.Count == 1)
                    {
                        noOutId.Clear();
                        noOutId.Add(0);
                        noOutId.Add(-1);
                        noOutId.Add(-1);
                    }
                    if (noInId.Count == 1)
                    {
                        noInId.Clear();
                        noInId.Add(1);
                        noInId.Add(-1);
                        noInId.Add(-1);
                    }

                    var temp = new object();
                    for (var i = 0; i < _priceScript.fares.Count; i++)
                    {
                        var fare = _priceScript.fares[i];
                        for (var j = 0; j < fare.Indices.Count; j++)
                        {
                            var ind = fare.Indices[j];
                            if (Convert.ToInt32(ind.obI) == noOutId[1] && Convert.ToInt32(ind.obJ) == noOutId[2]
                                && Convert.ToInt32(ind.ibI) == noInId[1] && Convert.ToInt32(ind.ibJ) == noInId[2])
                                temp = fare;
                        }
                    }
                    return temp;
                }

                public int GetPrice(int adult, int child, int infant)
                {
                    dynamic fare = WorkOutTripTotals();
                    var totalprice = 0;
                    int adultPrice;
                    int childPrice;
                    int infantPrice;
                    if (child != 0 && infant != 0)
                    {
                        adultPrice = Convert.ToInt32(fare.PaxFares[0].Base) +
                                     Convert.ToInt32(fare.PaxFares[0].Taxes);
                        childPrice = Convert.ToInt32(fare.PaxFares[1].Base) +
                                     Convert.ToInt32(fare.PaxFares[1].Taxes);
                        infantPrice = Convert.ToInt32(fare.PaxFares[2].Base) +
                                      Convert.ToInt32(fare.PaxFares[2].Taxes);

                        totalprice = adult*adultPrice + child*childPrice + infant*infantPrice;
                    }
                    else if (child != 0 && infant == 0)
                    {
                        adultPrice = Convert.ToInt32(fare.PaxFares[0].Base) +
                                     Convert.ToInt32(fare.PaxFares[0].Taxes);
                        childPrice = Convert.ToInt32(fare.PaxFares[1].Base) +
                                     Convert.ToInt32(fare.PaxFares[1].Taxes);

                        totalprice = adult*adultPrice + child*childPrice;
                    }
                    else if (child == 0 && infant != 0)
                    {

                        adultPrice = Convert.ToInt32(fare.PaxFares[0].Base) +
                                     Convert.ToInt32(fare.PaxFares[0].Taxes);
                        infantPrice = Convert.ToInt32(fare.PaxFares[1].Base) +
                                      Convert.ToInt32(fare.PaxFares[1].Taxes);

                        totalprice = adult*adultPrice + infant*infantPrice;
                    }
                    else if (child == 0 && infant == 0)
                    {

                        adultPrice = Convert.ToInt32(fare.PaxFares[0].Base) +
                                     Convert.ToInt32(fare.PaxFares[0].Taxes);
                        totalprice = adult*adultPrice;
                    }

                    return totalprice;
                }
            }
        }

    }
}

//}

