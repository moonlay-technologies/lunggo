using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using Lunggo.ApCommon.Log;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            //var sementarai = new SearchFlightConditions
            //{
            //    AdultCount = 2,
            //    ChildCount = 2,
            //    InfantCount = 2,
            //    CabinClass = CabinClass.Economy,
            //    Trips = new List<FlightTrip>
            //        {
            //            new FlightTrip
            //            {
            //                OriginAirport = "KNO",
            //                DestinationAirport = "WGP",
            //                DepartureDate = new DateTime(2015,11,4)
            //            }
            //        },
            //
            //};
            return Client.SearchFlight(conditions);
        }

        private partial class SriwijayaClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                if (conditions.Trips.Count > 1)
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };

                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");



                var trip0 = conditions.Trips[0];
                trip0.OriginAirport = trip0.OriginAirport == "JKT" ? "CGK" : trip0.OriginAirport;
                trip0.DestinationAirport = trip0.DestinationAirport == "JKT" ? "CGK" : trip0.DestinationAirport;
                var client = CreateCustomerClient();
                var hasil = new SearchFlightResult();
                var convertBulan = trip0.DepartureDate.ToString("MMMM");
                var bulan3 = convertBulan.Substring(0, 3);
                var depDate = trip0.DepartureDate.ToString("dd-MMM-yyyy");
                //Get Home Page

                var url0 = "/SJ";
                var rq0 = new RestRequest(url0, Method.GET);
                rq0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                rq0.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var searchResponse0 = client.Execute(rq0);
                if (searchResponse0.ResponseUri.AbsolutePath != "/SJ/" &&
                           (searchResponse0.StatusCode == HttpStatusCode.OK ||
                            searchResponse0.StatusCode == HttpStatusCode.Redirect))
                {

                }
                var htmlPageHome = (CQ) searchResponse0.Content;
                var gibberishValue = htmlPageHome["#searchFlightForm_ #cityDetail"].Val();
                var encodedValue = HttpUtility.UrlEncode(gibberishValue);

                //Get Data
                const string urlGetData = "/SJ/Flights/getData";

                var postdata0 = "cityData=" + encodedValue 
                    +"&cityDetailTo=" + encodedValue
                    + "&roundTrip=NO&typeAction=search_flight&TheCheckBox=on&fromSrc="+ trip0.OriginAirport + 
                    "&toSrc="+ trip0.DestinationAirport + "&departureDate="+ depDate + 
                    "&AdultSrc="+ conditions.AdultCount + "&ChildSrc="+ conditions.ChildCount + "&InfantSrc=" + conditions.InfantCount 
                    + "&PromoCode=&cityData=" + encodedValue + "&cityDetailTo=" + encodedValue
                    + "&roundTrip=NO&typeAction=search_flight&TheCheckBox=on&fromSrc=" + trip0.OriginAirport + "&fromSrc2=" + trip0.OriginAirport
                    + "&toSrc=" + trip0.DestinationAirport + "&toSrc2=" + trip0.DestinationAirport + "&departureDate=" + depDate + "&AdultSrc=" + conditions.AdultCount + "&ChildSrc=" + conditions.ChildCount
                    + "&InfantSrc=" + conditions.InfantCount + "&PromoCode=" + "&departureDate=" + depDate + "&returnDate="; 
               
                var getDataRq = new RestRequest(urlGetData, Method.POST);
                getDataRq.AddHeader("Accept-Encoding", "gzip, deflate, br");
                getDataRq.AddHeader("Accept", "*/*");
                getDataRq.AddParameter("application/x-www-form-urlencoded", postdata0, ParameterType.RequestBody);
                var getDataSearchResponse = client.Execute(getDataRq);
                if (getDataSearchResponse.ResponseUri.AbsolutePath != "/SJ/Flights/getData" &&
                           (getDataSearchResponse.StatusCode == HttpStatusCode.OK ||
                            getDataSearchResponse.StatusCode == HttpStatusCode.Redirect))
                {
                    
                }
                var encodedGetDataResponse = HttpUtility.UrlEncode(getDataSearchResponse.Content);

                var flightUrl = "/SJ/Flights";
                var flightRq = new RestRequest(flightUrl, Method.POST);
                flightRq.AddHeader("Accept-Encoding", "gzip, deflate, br");
                flightRq.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                flightRq.AddParameter("application/x-www-form-urlencoded", "param=" + encodedGetDataResponse, ParameterType.RequestBody);
                var flightResponse = client.Execute(flightRq);
                if (flightResponse.ResponseUri.AbsolutePath != "/SJ/Flights" &&
                           (flightResponse.StatusCode == HttpStatusCode.OK ||
                            flightResponse.StatusCode == HttpStatusCode.Redirect))
                {
                   
                }
                var flightContent = (CQ) flightResponse.Content;

                var listFlight = flightContent[".booking-item"];
                var listItin = new List<FlightItinerary>();
                try
                {
                    for (var i = 0; i < listFlight.Length - 1; i++)
                    {
                        
                        long price;
                        IDomObject table;
                        if ( conditions.CabinClass == CabinClass.Economy && !listFlight[i].ChildElements.ToList()[0].
                            ChildElements.ToList()[0].ChildElements.ToList()[3].
                            ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText.Contains("SOLD OUT"))
                        {
                            table = listFlight[i].ChildElements.ToList()[1].ChildElements.ToList()[0].ChildElements.ToList()[0]
                                   .ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[1];
                        }
                        else if (conditions.CabinClass == CabinClass.Business && !listFlight[i].ChildElements.ToList()[0].
                        ChildElements.ToList()[0].ChildElements.ToList()[3].ChildElements.ToList()[0].ChildElements.
                            ToList()[1].ChildElements.ToList()[0].InnerText.Contains("SOLD OUT"))
                        {
                            table =
                                listFlight[i].ChildElements.ToList()[1].ChildElements.ToList()[0].ChildElements.ToList()[0]
                                    .ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[1];
                        }
                        else
                        {
                            continue;
                        }

                        var segments = new List<FlightSegment>();
                        var rows = table.ChildElements.ToList();
                        foreach (var row in rows)
                        {
                            var data = row.ChildElements.ToList();
                            var planenumber = data[0].InnerText.Trim(' ');
                            var departuredate = data[1].InnerText;
                            var originAirport = data[2].InnerText.Split('-')[0];
                            var destAirport = data[2].InnerText.Split('-')[1];
                            var departureHr = data[3].InnerText;
                            var arrivalHr = data[4].InnerText;
                            var duration = data[5].InnerText;
                            var stopAmount = data[6].InnerText;

                            var splittedDepDate = departuredate.Split('-');
                            departuredate = splittedDepDate[0] + '-' + splittedDepDate[1].Substring(0, 1) +
                                            splittedDepDate[1].Substring(1, 2).
                                                ToLower() + '-' + splittedDepDate[2];
                            var splittedDptTime = departureHr.Split(':');
                            var d = DateTime.ParseExact(departuredate, "dd-MMM-yy",
                                CultureInfo.InvariantCulture);
                            var dprtTime = DateTime.SpecifyKind(new DateTime(d.Year, d.Month, d.Day,
                                Convert.ToInt32(splittedDptTime[0]), Convert.ToInt32(splittedDptTime[1]), 0),
                                DateTimeKind.Utc);
                            var splittedArrTime = arrivalHr.Split(':');

                            var a = d;
                            if (Convert.ToInt32(splittedDptTime[0]) > Convert.ToInt32(splittedArrTime[0]))
                            {
                                a = d.AddDays(1);
                            }

                            var arrTime = DateTime.SpecifyKind(new DateTime(a.Year, a.Month, a.Day,
                                Convert.ToInt32(splittedArrTime[0]), Convert.ToInt32(splittedArrTime[1]), 0),
                                DateTimeKind.Utc);

                            var baggage = GetBaggage(originAirport, destAirport);
                            var segment = new FlightSegment
                            {
                                AirlineCode = planenumber.Substring(0, 2),
                                AirlineName = planenumber.Substring(0, 2) == "IN" ? "Nam Air" : "Sriwijaya Air",
                                ArrivalAirport = destAirport,
                                ArrivalTime = arrTime,
                                BaggageCapacity = baggage,
                                CabinClass = conditions.CabinClass,
                                DepartureAirport = originAirport,
                                DepartureTime = dprtTime,
                                Duration = new TimeSpan(Convert.ToInt32(duration.Split(':')[0]), Convert.ToInt32(duration.Split(':')[1]), 0),
                                FlightNumber = planenumber.Substring(2, planenumber.Length - 2),
                                StopQuantity = Convert.ToInt32(stopAmount),
                                IsBaggageIncluded = true,
                                IsMealIncluded = true,
                                IsPscIncluded = true,
                                OperatingAirlineCode = planenumber.Substring(0, 2),
                                OperatingAirlineName = planenumber.Substring(0, 2) == "IN" ? "Nam Air" : "Sriwijaya Air"
                            };
                            segments.Add(segment);
                        }

                        IDomObject button;

                        if (conditions.CabinClass == CabinClass.Economy)
                        {
                            button = listFlight[i].ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[3].
                            ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[3];
                        }
                        else if (conditions.CabinClass == CabinClass.Business)
                        {
                            button = listFlight[i].ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[3].
                                ChildElements.ToList()[0].ChildElements.ToList()[1].ChildElements.ToList()[4];
                        }
                        else
                        {
                            button = null;
                        }
                                
                        var buttonData = button.GetAttribute("onclick");
                        var buttonDataEnd = buttonData.IndexOf(',');
                        var buttonDataSummary = buttonData.Substring(13, buttonDataEnd - 14);

                        var bdsDecoded = buttonDataSummary.Base64Decode().Deserialize<dynamic>();

                        const string urlSummary = "/SJ/Flights/detail_price_view";
                        var postdataRsv = "DETAIL_PRICE=" + buttonDataSummary + "&status_sti=0";

                        var getSummaryRq = new RestRequest(urlSummary, Method.POST);
                        getSummaryRq.AddHeader("Accept-Encoding", "gzip, deflate, br");
                        getSummaryRq.AddHeader("Accept", "*/*");
                        getSummaryRq.AddParameter("application/x-www-form-urlencoded", postdataRsv,
                            ParameterType.RequestBody);
                        var getSummaryResponse = client.Execute(getSummaryRq);
                        if (getSummaryResponse.ResponseUri.AbsolutePath != "/SJ/Flights/detail_price_view" &&
                           (getSummaryResponse.StatusCode == HttpStatusCode.OK ||
                            getSummaryResponse.StatusCode == HttpStatusCode.Redirect))
                        {
                           
                        }
                        var summaryPrice = (CQ) getSummaryResponse.Content;

                        var adultp = summaryPrice[".col-md-12:first-child tr:first-child tr:last-child td:nth-child(3) font"][0].
                            InnerText.Trim().Replace(".","");

                        var childp = "";
                        var infantp = "";
                        if (conditions.ChildCount > 0)
                        {
                            childp = summaryPrice[".col-md-12:first-child tr:nth-child(3) tr:last-child td:nth-child(3) font"][0].
                                InnerText.Trim().Replace(".", "");
                            if (conditions.InfantCount > 0)
                            {
                                infantp = summaryPrice[".col-md-12:first-child tr:nth-child(5) tr:last-child td:nth-child(3) font"]
                                    [0].InnerText.Trim().Replace(".", "");
                            }
                        }
                        else
                        {
                            if (conditions.InfantCount > 0)
                            {
                                infantp = summaryPrice[".col-md-12:first-child tr:nth-child(5) tr:last-child td:nth-child(3) font"]
                                    [0].InnerText.Trim().Replace(".", "");
                            }
                        }

                        var adultPrice = (adultp.Length == 0) ? 0M : Convert.ToInt64(adultp);
                        var childPrice = (childp.Length == 0) ? 0M : Convert.ToInt64(childp);
                        var infantPrice = (infantp.Length == 0) ? 0M : Convert.ToInt64(infantp);
                        var totalPrice =
                            summaryPrice[".col-md-12"].ToList()[1].ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText.
                            Trim().Replace(".","").Split(' ')[3];
                        price = Convert.ToInt64(totalPrice);
                        var isInternational = CheckInternationality(segments);

                        var keyDep = bdsDecoded.KEY_DEP;
                        var departureStation = bdsDecoded.DepartureStation;
                        var arrivalStation = bdsDecoded.ArrivalStation;
                        //var Fare =
                        //    "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.0.0|2346000.0.97174,3853813,1953461:X,M,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
                        var flightNumbers = segments.Aggregate("", (current, segment) => current + (segment.AirlineCode + "." + segment.FlightNumber + "."));
                        var dataTrip = trip0.OriginAirport + "." + trip0.DestinationAirport
                            + "?" + trip0.DepartureDate.ToString("yyyy-MM-dd") + "|" + conditions.AdultCount + "." +
                            conditions.ChildCount + "." + conditions.InfantCount + "|" + totalPrice + ".";
                        if (conditions.CabinClass == CabinClass.Economy)
                        {
                            dataTrip += "1.";
                        }
                        else if (conditions.CabinClass == CabinClass.Business)
                        {
                            dataTrip += "2.";
                        }

                        var segmentCode = "";
                        var classCode = "";
                        string unknown;
                        if (segments.Count == 1)
                        {
                            var keydepSplitted = keyDep.ToString().Split(':');
                            segmentCode = keydepSplitted[0] + ":";
                            classCode = keydepSplitted[1] + ":";
                            unknown = keydepSplitted[2] + ":";
                        }
                        else
                        {
                            var keydepSplitted = keyDep.ToString().Split('@');
                            foreach (var key in keydepSplitted)
                            {
                                var data = key.ToString().Split(':');
                                segmentCode += data[0] + ",";
                                classCode += data[1] + ",";
                            }
                            segmentCode = segmentCode.Substring(0, segmentCode.Length - 1) + ":";
                            classCode = classCode.Substring(0, classCode.Length - 1) + ":";
                            unknown = keydepSplitted[0].ToString().Split(':')[2] + ":";
                        }

                        var fareId = flightNumbers + dataTrip + segmentCode + classCode + unknown + trip0.OriginAirport +
                                     ":" + trip0.DestinationAirport
                                     + ":" + "U2s5VlVrNUZXUT09";

                        var itin = new FlightItinerary
                        {
                            AdultCount = conditions.AdultCount,
                            InfantCount = conditions.InfantCount,
                            ChildCount = conditions.ChildCount,
                            CanHold = true,
                            FareType = FareType.Published,
                            RequireBirthDate = isInternational,
                            RequirePassport = isInternational,
                            TripType = TripType.OneWay,
                            Supplier = Supplier.Sriwijaya,
                            FareId = fareId,
                            Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    Segments = segments,
                                    OriginAirport = conditions.Trips[0].OriginAirport, //
                                    DestinationAirport = conditions.Trips[0].DestinationAirport, //
                                    DepartureDate = trip0.DepartureDate
                                }
                            },
                            Price = new Price(),
                            AdultPricePortion = adultPrice / price,
                            ChildPricePortion = childPrice / price,
                            InfantPricePortion = infantPrice / price,
                            RequestedCabinClass = conditions.CabinClass
                        };

                        itin.Price.SetSupplier(price, new Currency("IDR", Payment.Constant.Supplier.Sriwijaya));
                        listItin.Add(itin);                       
                    }
                    
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = listItin.Where(it => it.Price.SupplierCurrency.Rate != 0).ToList(),
                    };
                }
                catch(Exception e)
                {
                    
                    return new SearchFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError>
                        {
                            FlightError.TechnicalError
                        },
                        ErrorMessages = new List<string>
                        {
                            "[Sriwijaya Air] " + e.Message
                        }

                    };
                }
                
            }

            private bool CheckInternationality(List<FlightSegment> segments)
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
