using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Extension;
using Newtonsoft.Json;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Newtonsoft.Json.Linq;
using RestSharp;
//using RestSharp.Deserializers;
using RestSharp.Extensions.MonoHttp;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;
using FareType = Lunggo.ApCommon.Flight.Constant.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            return Client.SearchFlight(conditions);
        }

        private partial class GarudaClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {


                if (conditions.AdultCount == 0)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages =
                            new List<string> { "There must be at least one adult passenger" }
                    };
                }
                if (conditions.AdultCount + conditions.ChildCount > 9)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages =
                            new List<string> { "Total adult and children passenger must be not more than nine" }
                    };
                }
                if (conditions.AdultCount < conditions.InfantCount)
                {
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages =
                            new List<string> { "Every infant must be accompanied by one adult" }
                    };
                }

                var client = CreateCustomerClient();
                string flightResultPage;
                // Airport Generalizing
                var trip0 = new FlightTrip
                {
                    OriginAirport = conditions.Trips[0].OriginAirport,
                    DestinationAirport = conditions.Trips[0].DestinationAirport,
                    DepartureDate = conditions.Trips[0].DepartureDate
                };

                var originAirport = trip0.OriginAirport == "JKT" ? "CGK" : trip0.OriginAirport;
                var destinationAirport = trip0.DestinationAirport == "JKT" ? "CGK" : trip0.DestinationAirport;
                string cabincode;
                if (conditions.CabinClass == CabinClass.Economy)
                    cabincode = "E";
                else if (conditions.CabinClass == CabinClass.Business)
                    cabincode = "B";
                else
                {
                    cabincode = "F";
                }

                // [GET] Search Flight
                var dict = DictionaryService.GetInstance();
                var originCountry = dict.GetAirportCountryCode(trip0.OriginAirport);

                // Calling The Zeroth Page
                client.BaseUrl = new Uri("https://www.garuda-indonesia.com");
                string url0 = @"";
                var searchRequest0 = new RestRequest(url0, Method.GET);
                searchRequest0.AddHeader("Host", "www.garuda-indonesia.com");
                searchRequest0.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchRequest0.AddHeader("Upgrade-Insecure-Requests", "1");
                var searchResponse0 = client.Execute(searchRequest0);

                url0 = @"id/id/index.page";
                searchRequest0 = new RestRequest(url0, Method.GET);
                searchResponse0 = client.Execute(searchRequest0);
                var html0 = searchResponse0.Content;
                Console.WriteLine(html0);
                if (searchResponse0.ResponseUri.AbsolutePath != "/id/id/index.page" &&
                    (searchResponse0.StatusCode == HttpStatusCode.OK ||
                        searchResponse0.StatusCode == HttpStatusCode.Redirect))
                    return new SearchFlightResult { Errors = new List<FlightError> { FlightError.InvalidInputData } };


                var startIndex = html0.IndexOf("var citylist =");
                var endIndex = html0.LastIndexOf("var cities");
                var scr = html0.SubstringBetween(startIndex + 15, endIndex - 2).Replace("\n", "").Replace("\t", "");

                var depAirport = GetGarudaAirport(scr, originAirport).Replace("+", "%20");
                var arrAirport = GetGarudaAirport(scr, destinationAirport).Replace("+", "%20");

                if (depAirport.Length == 0 || arrAirport.Length == 0)
                {
                    return new SearchFlightResult
                    {

                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()

                    };
                }
                var adParam = (CQ)html0;
                var idParam = adParam["#paramforads"].Attr("value");
                var iw_component = adParam[".iw_component"].Attr("id");

                if (originCountry == "ID")
                {

                    //POST 0
                    string url = @"/id/id/index/1414649500712.ajax";
                    var searchRequestAjax = new RestRequest(url, Method.POST);
                    searchRequestAjax.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchRequestAjax.AddHeader("Accept", "*/*");
                    searchRequestAjax.AddHeader("Referer", "https://www.garuda-indonesia.com/id/id/index.page");
                    searchRequestAjax.AddHeader("Origin", "https://www.garuda-indonesia.com");
                    searchRequestAjax.AddHeader("Host", "www.garuda-indonesia.com");
                    var postData = "function=jsessionSecure";
                    searchRequestAjax.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var searchResponseAjax = client.Execute(searchRequestAjax);
                    var html1 = searchResponseAjax.Content;

                    // POST 1
                    url = @"id/id/index/1410947344734.ajax";
                    var searchRequest = new RestRequest(url, Method.POST);
                    searchRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchRequest.AddHeader("Accept", "*/*");
                    searchRequest.AddHeader("Referer", "https://www.garuda-indonesia.com/id/id/index.page");
                    searchRequest.AddHeader("Origin", "https://www.garuda-indonesia.com");
                    searchRequest.AddHeader("Host", "www.garuda-indonesia.com");
                    postData =
                        @"=%2Fid%2Fid%2Findex%2F1410947344734.ajax" +
                        @"&TRIP_TYPE=O" +
                        @"&originairportcode=" + depAirport +
                        @"&destairportcode=" + arrAirport +
                        @"&BOOKING_DATE_TIME_1=" + trip0.DepartureDate.ToString("dd") + "%2F" + trip0.DepartureDate.ToString("MM") + "%2F"
                        + trip0.DepartureDate.ToString("yyyy") +
                        @"&BOOKING_DATE_TIME_2=" +
                        @"&originairportcode2=" +
                        @"&destairportcode2=" +
                        @"&BOOKING_DATE_TIME_2=" +
                        @"&originairportcode3=" +
                        @"&destairportcode3=" +
                        @"&BOOKING_DATE_TIME_3=" +
                        @"&originairportcode4=" +
                        @"&destairportcode4=" +
                        @"&BOOKING_DATE_TIME_4=" +
                        @"&originairportcode5=" +
                        @"&destairportcode5=" +
                        @"&BOOKING_DATE_TIME_5=" +
                        @"&originairportcode6=" +
                        @"&destairportcode6=" +
                        @"&BOOKING_DATE_TIME_6=" +
                        @"&guestTypes%5B0%5D.amount=" + conditions.AdultCount.ToString(CultureInfo.InvariantCulture) +
                        @"&=ADT" +
                        @"&guestTypes%5B1%5D.amount=" + conditions.ChildCount.ToString(CultureInfo.InvariantCulture) +
                        @"&=CHD" +
                        @"&guestTypes%5B2%5D.amount=" + conditions.InfantCount.ToString(CultureInfo.InvariantCulture) +
                        @"&=INF" +
                        @"&CABIN=" + cabincode +
                        @"&promoCode=" +
                        @"&lang=ID" +
                        @"&bookingType=IBE" +
                        @"&external_id4=" +
                        @"&BOOKING_DATE_TIME=&" +
                        @"adsParam=" + idParam +
                        @"&function=book" +
                        @"&fromCaptcha=undefined";
                    searchRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var searchResponse = client.Execute(searchRequest);
                    var html2 = searchResponse.Content;

                    var dataAnehStartIndex = html2.IndexOf("ENC=");
                    var dataAnehEndIndex = html2.IndexOf("</PA>");
                    var enc = html2.SubstringBetween(dataAnehStartIndex + 4, dataAnehEndIndex);

                    var priceAvailabilityStartIndex = html2.IndexOf("EMBEDDED_TRANSACTION");
                    var priceAvailabilityEndIndex = html2.LastIndexOf("ENCT");
                    var priceAvailability = html2.SubstringBetween(priceAvailabilityStartIndex + 21,
                        priceAvailabilityEndIndex - 5);
                    //POST DATA 2
                    client.BaseUrl = new Uri("https://booking.garuda-indonesia.com");

                    url = @"plnext/garudaindonesiaDX/Override.action";
                    url += "?__utma=46826104.185345349.1464840325.1464852689.1464858170.3";
                    url += "&__utmb=46826104.13.10.1464858170";
                    url += "&__utmc=46826104";
                    url += "&__utmx=-";
                    url += "&__utmz=46826104.1464840325.1.1.utmcsr=google|utmccn=(organic)|utmcmd=organic|utmctr=(not provided)";
                    url += "&__utmv=-";
                    url += "&__utmk=258818063";
                    var search = new RestRequest(url, Method.POST);
                    postData =
                        @"SITE=CBEECNEW" +
                        @"&LANGUAGE=ID" +
                        @"&EMBEDDED_TRANSACTION=" + priceAvailability +
                        @"&ENCT=1" +
                        @"&ENC=" + enc;

                    search.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);

                    search.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    search.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    search.AddHeader("Referer", "https://www.garuda-indonesia.com/id/id/index.page");
                    search.AddHeader("Origin", "https://www.garuda-indonesia.com");
                    search.AddHeader("Cache-Control", "max-age=0");
                    search.AddHeader("Upgrade-Insecure-Requests", "1");

                    var searchResult = client.Execute(search);
                    flightResultPage = searchResult.Content;
                }
                else
                {
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };
                }
                try
                {
                    //Get All Itins
                    var itinScriptStartIndex = flightResultPage.IndexOf("proposedFlightsGroup");
                    var itinScriptEndIndex = flightResultPage.LastIndexOf("arrangeBy");
                    var itinScript = flightResultPage.SubstringBetween(itinScriptStartIndex + 22, itinScriptEndIndex - 4);
                    var priceScriptStartIndex = itinScriptEndIndex + 33;
                    var priceScriptEndIndex = flightResultPage.IndexOf("currencyBean") - 2;
                    var priceScript = flightResultPage.SubstringBetween(priceScriptStartIndex, priceScriptEndIndex);

                    if (itinScriptStartIndex == -1)
                    {
                        itinScriptStartIndex = flightResultPage.IndexOf("listFlight");
                        itinScriptEndIndex = flightResultPage.IndexOf("displayNextDayLink");
                        itinScript = flightResultPage.SubstringBetween(itinScriptStartIndex + 12, itinScriptEndIndex - 2);
                        return new SearchFlightResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages =
                                new List<string> { "Price does not exist" }
                        };
                    }
                    var dataItins = GetItins(itinScript);
                    var priceClass = GetPrices(priceScript);
                    const string format = "MMM d, yyyy h:mm:ss tt";
                    var itins = new List<FlightItinerary>();
                    foreach (var itin in dataItins)
                    {
                        var segments = new List<FlightSegment>();
                        var fareId = "";
                        foreach (var segment in itin.Segments)
                        {
                            var deptTime =
                                DateTime.SpecifyKind(
                                    DateTime.ParseExact(segment.BeginDate, format, CultureInfo.InvariantCulture),
                                    DateTimeKind.Utc);
                            var arrTime =
                                DateTime.SpecifyKind(
                                    DateTime.ParseExact(segment.EndDate, format, CultureInfo.InvariantCulture),
                                    DateTimeKind.Utc);
                            fareId = fareId + segment.Airline.Code + "-" + segment.FlightNumber + "|";
                            segments.Add(new FlightSegment
                            {
                                AirlineCode = segment.Airline.Code,
                                FlightNumber = segment.FlightNumber,
                                CabinClass = conditions.CabinClass,
                                DepartureAirport = segment.BeginLocation.LocationCode,
                                DepartureTime = deptTime,
                                ArrivalAirport = segment.EndLocation.LocationCode,
                                ArrivalTime = arrTime,
                                OperatingAirlineCode = segment.Airline.Code,
                                StopQuantity = Convert.ToInt32(segment.NbrOfStops),
                                AircraftCode = segment.Equipment.Name.Split(' ').Last(),
                                DepartureTerminal = segment.BeginTerminal,
                                ArrivalTerminal = segment.EndTerminal,
                                Duration = arrTime.AddHours(-(dict.GetAirportTimeZone(segment.EndLocation.LocationCode))) -
                                    deptTime.AddHours(-(dict.GetAirportTimeZone(segment.BeginLocation.LocationCode))),
                                Meal = true
                            });

                            if (segment.ListLegs != null)
                            {
                                var stops = new List<FlightStop>();
                                for (var x = 1; x < segment.ListLegs.Count(); x++)
                                {
                                    stops.Add(new FlightStop
                                    {
                                        Airport = segment.ListLegs.ElementAt(x).BoardPoint.LocationCode,
                                        ArrivalTime =
                                            DateTime.SpecifyKind(
                                                DateTime.ParseExact(segment.ListLegs[x - 1].ArrivalDate, format,
                                                    CultureInfo.InvariantCulture),
                                                DateTimeKind.Utc),
                                        DepartureTime =
                                            DateTime.SpecifyKind(
                                                DateTime.ParseExact(segment.ListLegs[x].DepartureDate, format,
                                                    CultureInfo.InvariantCulture),
                                                DateTimeKind.Utc),
                                        Duration = DateTime.SpecifyKind(
                                                DateTime.ParseExact(segment.ListLegs[x].DepartureDate, format,
                                                    CultureInfo.InvariantCulture),
                                                DateTimeKind.Utc) - DateTime.SpecifyKind(
                                                DateTime.ParseExact(segment.ListLegs[x - 1].ArrivalDate, format,
                                                    CultureInfo.InvariantCulture),
                                                DateTimeKind.Utc)
                                    });
                                }
                                segments.Last().Stops = stops;
                            }
                        }



                        var flightid = itin.ProposedBoundId;
                        var listPrice = new List<Int32>();
                        foreach (var priceclass in priceClass)
                        {
                            foreach (var f in priceclass.Bounds[0].FlightGroupList)
                            {
                                if (f.FlightId == flightid)
                                {
                                    listPrice.Add(Convert.ToInt32(priceclass.PnrPrices[0].RecoAmounts[0].RecoAmount.TotalAmount));
                                }
                            }

                            string adultPriceEach;
                            string childPriceEach;
                            string infantPriceEach;

                            adultPriceEach =
                                priceclass.PnrPrices[0].TravellerPrices[0].TravellersPrice[0].RecoAmount.TotalAmount;
                            if (priceclass.PnrPrices[0].TravellerPrices.Count() == 3)
                            {
                                childPriceEach =
                                priceclass.PnrPrices[0].TravellerPrices[1].TravellersPrice[0].RecoAmount.TotalAmount;
                                infantPriceEach =
                                priceclass.PnrPrices[0].TravellerPrices[2].TravellersPrice[0].RecoAmount.TotalAmount;
                            }

                            else if (priceclass.PnrPrices[0].TravellerPrices.Count() == 2)
                            {
                                childPriceEach =
                                priceclass.PnrPrices[0].TravellerPrices[1].TravellersPrice[0].RecoAmount.TotalAmount;
                                infantPriceEach = "0";
                            }
                            else if (priceclass.PnrPrices[0].TravellerPrices.Count() == 1)
                            {
                                childPriceEach = "0";
                                infantPriceEach = "0";
                            }

                        }

                        var price = listPrice.Min();

                        fareId = segments.ElementAt(0).DepartureAirport + "+" + segments.ElementAt(segments.Count - 1).ArrivalAirport
                            + "+" + trip0.DepartureDate.Day + "+" + trip0.DepartureDate.Month + "+" +
                            trip0.DepartureDate.Year + "+" + segments.ElementAt(0).DepartureTime.Hour + "+" +
                            segments.ElementAt(0).DepartureTime.Minute + "+" +
                            conditions.AdultCount + "+" +
                            conditions.ChildCount + "+" + conditions.InfantCount + "+" +
                            FlightService.ParseCabinClass(conditions.CabinClass) + "+" +
                            price + "+" + fareId.SubstringBetween(0, fareId.Length - 1);

                        var itinerary = new FlightItinerary
                        {
                            AdultCount = conditions.AdultCount,
                            ChildCount = conditions.ChildCount,
                            InfantCount = conditions.InfantCount,
                            CanHold = true,
                            FareType = FareType.Published,
                            RequireBirthDate = false,
                            RequirePassport = RequirePassport(segments),
                            RequireSameCheckIn = false,
                            RequireNationality = false,
                            RequestedCabinClass = conditions.CabinClass,
                            TripType = TripType.OneWay,
                            Supplier = Supplier.Garuda,
                            SupplierCurrency = "IDR",
                            SupplierPrice = price,
                            SupplierRate = 1,
                            FareId = fareId,
                            Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = segments.ElementAt(0).DepartureAirport,
                                    DestinationAirport = segments.ElementAt(segments.Count - 1).ArrivalAirport,
                                    DepartureDate = DateTime.SpecifyKind(trip0.DepartureDate, DateTimeKind.Utc),
                                    Segments = segments
                                }
                            }
                        };
                        itins.Add(itinerary);
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

            private string GetGarudaAirport(string scr, string code)
            {
                var airportScr = scr.Deserialize<List<GarudaAirport>>();
                var arpt = "";
                foreach (GarudaAirport arp in airportScr)
                {
                    if (arp.Desc.Contains(code))
                    {
                        arpt = HttpUtility.UrlEncode(arp.Label + ", " + arp.Desc);
                    }
                }
                return arpt;
            }

            private List<Itin> GetItins(string itinScr)
            {
                return itinScr.Deserialize<List<Itin>>();

            }

            private List<PriceClass> GetPrices(string priceScr)
            {
                return priceScr.Deserialize<List<PriceClass>>();
            }

            private class GarudaAirport
            {
                public string Value { get; set; }
                public string Desc { get; set; }
                public String Country { get; set; }
                public String Label { get; set; }
            }

            private class Location
            {
                public string CountryName { get; set; }
                public string LocationName { get; set; }
                public string CityName { get; set; }
                public string CityCode { get; set; }
                public string CountryCode { get; set; }
                public string LocationCode { get; set; }
            }

            private class Airline
            {
                public string Name { get; set; }
                public string Code { get; set; }
            }

            private class Equipment
            {
                public string Name { get; set; }
            }

            private class segments
            {
                public string id { get; set; }
                public Location BeginLocation { get; set; }
                public Location EndLocation { get; set; }
                public string BeginDate { get; set; }
                public string EndDate { get; set; }
                public Airline Airline { get; set; }
                public string FlightNumber { get; set; }
                public string BeginTerminal { get; set; }
                public string EndTerminal { get; set; }
                public Equipment Equipment { get; set; }
                public string NbrOfStops { get; set; }
                public List<Legs> ListLegs { get; set; }

            }

            private class Legs
            {
                public string ArrivalDate { get; set; }
                public string DepartureDate { get; set; }
                public Location BoardPoint { get; set; }
                public Location OffPoint { get; set; }
            }
            private class Itin
            {
                public List<segments> Segments { get; set; }
                public string ProposedBoundId { get; set; }
            }

            private class RecoAmt
            {
                public string TotalAmount { get; set; }
            }

            private class RecoAmounts
            {
                public RecoAmt RecoAmount { get; set; }
            }

            private class PnrPrices
            {
                public List<RecoAmounts> RecoAmounts { get; set; }
                public List<TravellerType> TravellerPrices { get; set; }
            }

            private class TravellerType
            {
                public List<TravellersPrice> TravellersPrice { get; set; }
            }

            private class TravellersPrice
            {
                public RecoAmt RecoAmount { get; set; }
            }

            private class FlightGrpList
            {
                public String FlightId { get; set; }
            }

            private class Bound
            {
                public List<FlightGrpList> FlightGroupList { get; set; }
            }

            private class PriceClass
            {
                public List<Bound> Bounds { get; set; }
                public List<PnrPrices> PnrPrices { get; set; }
                public string RecoId { get; set; }
            }


        }

    }
}



