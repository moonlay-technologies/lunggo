using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
                if (conditions.Trips[0].DepartureDate > DateTime.Now.AddMonths(12).Date)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Time of Departure exceeds"}
                    };
                }

                // Airport Generalizing
                var trip0 = new FlightTrip
                {
                    OriginAirport = conditions.Trips[0].OriginAirport,
                    DestinationAirport = conditions.Trips[0].DestinationAirport,
                    DepartureDate = conditions.Trips[0].DepartureDate
                };

                string originAirport, destinationAirport;
                if (trip0.OriginAirport == "JKT")
                {
                    originAirport= "CGK";
                }
                else
                {
                   originAirport = trip0.OriginAirport;
                }
                    
                if (trip0.DestinationAirport == "JKT")
                {
                    destinationAirport = "CGK";
                }
                else
                {
                    destinationAirport = trip0.DestinationAirport;
                }

                // [GET] Search Flight
                var dict = DictionaryService.GetInstance();
                var originCountry = dict.GetAirportCountryCode(trip0.OriginAirport);
                CQ availableFares;
                CQ departureDate;
                string scr;


                if (originCountry == "ID")
                {
                    // Calling The Zeroth Page
                    client.BaseUrl = new Uri("http://www.lionair.co.id");
                    var url0 = @"Default.aspx";
                    var searchRequest0 = new RestRequest(url0, Method.GET);
                    searchRequest0.AddHeader("Referer", "http://www.lionair.co.id");

                    var searchResponse0 = client.Execute(searchRequest0);

                    if (searchResponse0.ResponseUri.AbsolutePath != "/Default.aspx" &&
                        (searchResponse0.StatusCode == HttpStatusCode.OK ||
                         searchResponse0.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};

                    // Calling The First Page
                    client.BaseUrl = new Uri("https://secure2.lionair.co.id");
                    var url = @"lionairibe/OnlineBooking.aspx";
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
                    searchRequest.AddQueryParameter("origin", "EN");
                    searchRequest.AddQueryParameter("usercountry", "ID");

                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                           SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                    var searchResponse = client.Execute(searchRequest);

                    if (searchResponse.ResponseUri.AbsolutePath != "/lionairibe/OnlineBooking.aspx" &&
                        (searchResponse.StatusCode == HttpStatusCode.OK ||
                         searchResponse.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};

                    //Calling The Second Page
                    var url2 = @"lionairibe/OnlineBooking.aspx";
                    var searchRequest2 = new RestRequest(url2, Method.GET);
                    searchRequest2.AddHeader("Referer", "https://secure2.lionair.co.id");

                    ServicePointManager.ServerCertificateValidationCallback +=
                        (sender, certificate, chain, sslPolicyErrors) => true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                           SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                    var searchResponse2 = client.Execute(searchRequest2);

                    var html2 = searchResponse2.Content;

                    if (searchResponse2.ResponseUri.AbsolutePath != "/lionairibe/OnlineBooking.aspx" &&
                        (searchResponse2.StatusCode == HttpStatusCode.OK ||
                         searchResponse2.StatusCode == HttpStatusCode.Redirect))
                        return new SearchFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};

                    var searchedHtml = (CQ) html2;
                    availableFares = searchedHtml[".flighttable_Row"];
                    departureDate = searchedHtml["#UcFlightSelection_txtDepartureDate"];
                    var json = searchedHtml["#Head1"].Text();
                    var startIndex = json.IndexOf("{'fares'");
                    var endIndex = json.IndexOf("})");
                    scr = json.Substring(startIndex, endIndex + 1 - startIndex);
                }
                else
                {
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };
                }

                GetLionAirPrice pricefunc = new GetLionAirPrice(scr);
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
                            while (it < availableFares.Count())
                            {
                                if (availableFares[it].ChildElements.ToList().Count != 4)
                                {
                                    if ((availableFares[it].ChildElements.ToList()[4].GetAttribute("class") ==
                                         "step2farecell flightInfo_middle" &&
                                         availableFares[it].ChildElements.ToList()[4].InnerText != "N/A")
                                        || (availableFares[it].ChildElements.ToList()[5].GetAttribute("class") ==
                                            "step2farecell flightInfo_middle" &&
                                            availableFares[it].ChildElements.ToList()[5].InnerText != "N/A"))
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
                                if (availableFares[it].ChildElements.ToList().Count != 4)
                                {
                                    if ((availableFares[it].ChildElements.ToList()[6].GetAttribute("class") ==
                                         "step2farecell flightInfo_right" &&
                                         availableFares[it].ChildElements.ToList()[6].InnerText != "N/A"))
                                    {
                                        fareIds.Add(availableFares[it]);
                                        j = it + 1;
                                            
                                        while ((j != availableFares.Count()) &&
                                                (availableFares[j - 1].GetAttribute("id")
                                                    .SubstringBetween(0,
                                                        (availableFares[j - 1].GetAttribute("id").Length - 2))
                                                == availableFares[j].GetAttribute("id").SubstringBetween(0,                                                        availableFares[j].GetAttribute("id").Length - 2)))
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

                    //var coba = fareIds.Count();
                    var itins = new List<FlightItinerary>();
                    
                    for (var ind = 0; ind < fareIds.Count; ind++)
                    {
                        //For index 0 or all segment 1
                        var segments = new List<FlightSegment>();
                        decimal price;
                        string importantData;
                        string cityArrival2 = "";
                        string airportArrival2 = "";
                        var listFlightNo = new List<string>();
                        var listDepHr = new List<string>();
                        var subst1 = fareIds.ElementAt(ind).GetAttribute("id");
                        if (ind == 0 ||
                            (subst1.SubstringBetween(0, subst1.Length - 2) !=
                             fareIds.ElementAt(ind - 1)
                                 .GetAttribute("id")
                                 .SubstringBetween(0, fareIds.ElementAt(ind - 1).GetAttribute("id").Length - 2)))
                        {
                            // Column 0 (the left-est)
                            var flight = fareIds.ElementAt(ind).ChildElements.ToList()[0];
                            var flightNo = "JT 34";
                            var aircraftNo = "737-900ER";
                            var airplaneName = "Lion Air";
                            if (flight.ChildElements.ToList().Count == 3)
                            {
                                flightNo = flight.ChildElements.ToList()[0].InnerText;
                                aircraftNo = flight.ChildElements.ToList()[1].InnerText;
                                airplaneName = flight.ChildElements.ToList()[2].InnerText;

                            }
                            else if (flight.ChildElements.ToList().Count == 2)
                            {
                                flightNo = flight.ChildElements.ToList()[0].InnerText;
                                airplaneName = flight.ChildElements.ToList()[1].InnerText;
                                aircraftNo = "Unknown";
                            }

                            listFlightNo.Add(flightNo);
                            // Column 1

                            var trip = fareIds.ElementAt(ind).ChildElements.ToList()[1];
                            var stopNo = trip.InnerText.Split(' ')[0];
                            var tess = trip.InnerText.Split(' ')[1];
                            var duration = trip.InnerText.Split(' ')[1].SubstringBetween(5, tess.Length) + " " +
                                           trip.InnerText.Split(' ')[2];
                            TimeSpan dur = TimeSpan.ParseExact(duration, "h'h 'm'm'", CultureInfo.InvariantCulture);

                            // Column 2
                            var format = "dd MMM yyyy HH:mm";
                            CultureInfo provider = CultureInfo.InvariantCulture;

                            var departure = fareIds.ElementAt(ind).ChildElements.ToList()[2];
                            var temp = new string[10];
                            temp = departure.ChildElements.ToList()[0].InnerText.Split(' ');
                            var cityDeparture = String.Join(" ", temp.Take(temp.Length - 1));
                            var airportDeparture = temp[temp.Length - 1].SubstringBetween(1,temp[temp.Length - 1].Length - 1);
                            var timeDeparture = departure.LastChild.ToString().Split(' ')[1];
                            listDepHr.Add(timeDeparture);

                            // still in string type (HH:mm)
                            var dateDepart = departureDate.Val();
                            var depDates = dateDepart + " " + timeDeparture; // 28 Jan 2016 10:30 //
                            var depDate = DateTime.SpecifyKind(DateTime.ParseExact(depDates, format, provider), DateTimeKind.Utc);

                            // Column 3
                            var arrival = fareIds.ElementAt(ind).ChildElements.ToList()[3];
                            string cityArrival;
                            string airportArrival;
                            string timeArrival;
                            if (arrival.ChildElements.ToList()[0].InnerText.Length != 0)
                            {
                                temp = arrival.ChildElements.ToList()[0].InnerText.Split(' ');
                                cityArrival = String.Join(" ", temp.Take(temp.Length - 1));
                                airportArrival = temp[temp.Length - 1].SubstringBetween(1,
                                    temp[temp.Length - 1].Length - 1);
                                timeArrival = arrival.LastChild.ToString().Split(' ')[1];
                            }
                            else
                            {
                                temp = arrival.InnerText.Split(' ');
                                cityArrival = String.Join(" ", temp.Take(temp.Length - 2));
                                airportArrival = arrival.InnerText.Split(' ')[temp.Length - 2].SubstringBetween(1, 4);
                                timeArrival = arrival.InnerText.Split(' ')[temp.Length - 1];
                            }

                            var jamdatang = Convert.ToInt32(timeArrival.Split(':')[0]);
                            var jambrgkt = Convert.ToInt32(timeDeparture.Split(':')[0]);
                            DateTime arrDate;
                            if (jamdatang < jambrgkt)
                            {
                                DateTime x = depDate.AddDays(1);
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
                                if (fareIds.ElementAt(ind).ChildElements.ToList()[4].InnerText != "Sold Out" &&
                                    fareIds.ElementAt(ind).ChildElements.ToList()[4].InnerText != "N/A")
                                {
                                    priceId = fareIds.ElementAt(ind).ChildElements.ToList()[4].GetAttribute("id");
                                }
                                else
                                {
                                    priceId = fareIds.ElementAt(ind).ChildElements.ToList()[5].GetAttribute("id");
                                }
                            }
                            else
                            {
                                priceId = fareIds.ElementAt(ind).ChildElements.ToList()[6].GetAttribute("id");
                            }

                            pricefunc.SetId(priceId);
                            price = pricefunc.GetPrice(conditions.AdultCount, conditions.ChildCount,
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
                                StopQuantity = Convert.ToInt32(stopNo),
                                Duration = dur,
                                AircraftCode = aircraftNo,
                                DepartureCity = cityDeparture,
                                ArrivalCity = cityArrival,
                                AirlineName = airplaneName,
                                OperatingAirlineName = airplaneName,
                                
                            });
                            var j = ind + 1;
                            while ((j != fareIds.Count) && (subst1.SubstringBetween(0, subst1.Length - 2) ==
                                                            fareIds.ElementAt(j)
                                                                .GetAttribute("id")
                                                                .SubstringBetween(0,
                                                                    fareIds.ElementAt(ind + 1).GetAttribute("id").Length -
                                                                    2)))
                            {
                                // Column 0 (the left-est)
                                flight = fareIds.ElementAt(j).ChildElements.ToList()[0];
                                if (flight.ChildElements.ToList().Count == 3)
                                {
                                    flightNo = flight.ChildElements.ToList()[0].InnerText;
                                    aircraftNo = flight.ChildElements.ToList()[1].InnerText;
                                    airplaneName = flight.ChildElements.ToList()[2].InnerText;

                                }
                                else if (flight.ChildElements.ToList().Count == 2)
                                {
                                    flightNo = flight.ChildElements.ToList()[0].InnerText;
                                    airplaneName = flight.ChildElements.ToList()[1].InnerText;
                                    aircraftNo = "Unknown";
                                }
                                // Column 1
                                listFlightNo.Add(flightNo);

                                trip = fareIds.ElementAt(j).ChildElements.ToList()[1];
                                stopNo = trip.InnerText.Split(' ')[0];
                                tess = trip.InnerText.Split(' ')[1];
                                duration = trip.InnerText.Split(' ')[1].SubstringBetween(5, tess.Length) + " " +
                                           trip.InnerText.Split(' ')[2];
                                dur = TimeSpan.ParseExact(duration, "h'h 'm'm'", CultureInfo.InvariantCulture);

                                // Column 2

                                departure = fareIds.ElementAt(j).ChildElements.ToList()[2];
                                string cityDeparture2;
                                string airportDeparture2;
                                string timeDeparture2;

                                if (departure.ChildElements.ToList()[0].InnerText.Length != 0)
                                {
                                    temp = departure.ChildElements.ToList()[0].InnerText.Split(' ');
                                    cityDeparture2 = String.Join(" ", temp.Take(temp.Length - 1));
                                    airportDeparture2 = temp[temp.Length - 1].SubstringBetween(1,
                                        temp[temp.Length - 1].Length - 1);
                                    timeDeparture2 = departure.LastChild.ToString().Split(' ')[1];
                                }
                                else
                                {
                                    temp = departure.InnerText.Split(' ');
                                    cityDeparture2 = String.Join(" ", temp.Take(temp.Length - 2));
                                    airportDeparture2 =
                                        departure.InnerText.Split(' ')[temp.Length - 2].SubstringBetween(1, 4);
                                    timeDeparture2 = departure.InnerText.Split(' ')[temp.Length - 1];
                                }
                                listDepHr.Add(timeDeparture2);
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

                                // Column 3
                                arrival = fareIds.ElementAt(j).ChildElements.ToList()[3];
                                string timeArrival2;
                                if (arrival.ChildElements.ToList()[0].InnerText.Length != 0)
                                {
                                    temp = arrival.ChildElements.ToList()[0].InnerText.Split(' ');
                                    cityArrival2 = String.Join(" ", temp.Take(temp.Length - 1));
                                    airportArrival2 = temp[temp.Length - 1].SubstringBetween(1,
                                        temp[temp.Length - 1].Length - 1);
                                    timeArrival2 = arrival.LastChild.ToString().Split(' ')[1];
                                }
                                else
                                {
                                    temp = arrival.InnerText.Split(' ');
                                    cityArrival2 = String.Join(" ", temp.Take(temp.Length - 2));
                                    airportArrival2 = arrival.InnerText.Split(' ')[temp.Length - 2].SubstringBetween(1,
                                        4);
                                    timeArrival2 = arrival.InnerText.Split(' ')[temp.Length - 1];
                                }
                                jamdatang = Convert.ToInt32(timeArrival2.Split(':')[0]);
                                jambrgkt = Convert.ToInt32(timeDeparture2.Split(':')[0]);

                                DateTime arrDate2;
                                if (jamdatang < jambrgkt)
                                {
                                    DateTime x = depDate2.AddDays(1);
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
                                // Column 4

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
                                    StopQuantity = Int32.Parse(stopNo),
                                    Duration = dur,
                                    AircraftCode = aircraftNo,
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


                            importantData = originAirport + "+"
                                           + destinationAirport + "+"
                                           + depDate + "+"
                                           + conditions.AdultCount + "+"
                                           + conditions.ChildCount + "+"
                                           + conditions.InfantCount + "+"
                                           + FlightService.ParseCabinClass(conditions.CabinClass) + "+"
                                           + price + "+" + priceId.SubstringBetween(3, priceId.Length) + "+" +
                                           +segments.Count + "+" + flightNoJoin + "+" + depHrJoin;

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

                    itins = itins.Where(itin => !itin.Trips[0].Segments.Exists(seg => seg.AirlineCode == "ID")).ToList();
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
            
            public class GetLionAirPrice
            {
                public dynamic priceScript;
                public string Id;

                public void SetId(string id)
                {
                    Id = id.SubstringBetween(3, id.Length);
                }

                public GetLionAirPrice(string scr)
                {
                    priceScript = JsonConvert.DeserializeObject(scr, typeof (object));
                }

                public object WorkOutTripTotals()
                {

                    //GET WHICH FARE USED
                    var codeFlight = ParseID(Id);
                    var id1 = "";
                    var id2 = "";
                    /*var outId = "";
                    var inId = "";*/
                    if (codeFlight[0] == 0)
                    {
                        //outId = Id;
                        id1 = Id;
                    }
                    else
                    {
                        //inId = Id;
                        id2 = Id;
                    }

                    return SearchForBreakdown(id1, id2);
                }

                public List<int> ParseID(string id)
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

                public object SearchForBreakdown(string outId, string inId)
                {
                    dynamic ps = priceScript.fares;
                    if (ps.Count == 0)
                    {
                        return null;
                    }

                    var noOutId = ParseID(outId);
                    var noInId = ParseID(inId);
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
                    for (int i = 0; i < priceScript.fares.Count; i++)
                    {
                        var fare = priceScript.fares[i];
                        for (int j = 0; j < fare.Indices.Count; j++)
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
                    int totalprice = 0;
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

