using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;
using System.Globalization;
using HttpUtility = RestSharp.Extensions.MonoHttp.HttpUtility;

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return Client.BookFlight(bookInfo);
        }

        private partial class GarudaClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                if (bookInfo.Itinerary.FareId == null)
                {
                    //throw new Exception("haloooo 1");
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        ErrorMessages = new List<string> {"FareId is null"}
                    };
                }

                var provider = CultureInfo.InvariantCulture;

                string origin, dest, flightId;
                DateTime depdate;
                int adultCount, childCount, infantCount;
                var bookingTimeLimit = "";
                var bookingReference = "";
                List<string> listflight;
                CabinClass cabinClass;
                var splittedFareId = bookInfo.Itinerary.FareId.Split('+');

                try
                {
                    origin = splittedFareId[0]; //CGK
                    dest = splittedFareId[1]; // SIN
                    depdate = new DateTime(Convert.ToInt32(splittedFareId[4]), Convert.ToInt32(splittedFareId[3]),
                        Convert.ToInt32(splittedFareId[2]));
                    adultCount = Convert.ToInt32(splittedFareId[5]);
                    childCount = Convert.ToInt32(splittedFareId[6]);
                    infantCount = Convert.ToInt32(splittedFareId[7]);
                    cabinClass = FlightService.ParseCabinClass(splittedFareId[8]);
                    listflight = splittedFareId[9].Split('|').ToList();
                }
                catch
                {
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.FareIdNoLongerValid},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false,
                        ErrorMessages = new List<string> {"FareId is no longer valid"}
                    };
                }

                var infants = bookInfo.Passengers.Where(pax => pax.Type == PassengerType.Infant);
                var children = bookInfo.Passengers.Where(pax => pax.Type == PassengerType.Child);
                bool isInfantValid = true;
                bool isChildValid = true;
                foreach (var inft in infants)
                {
                    if (inft.DateOfBirth.Value.AddYears(2) < depdate)
                    {
                        isInfantValid = false;
                    }
                }

                foreach (var child in children)
                {
                    if (
                        !(child.DateOfBirth.Value.AddYears(2) < depdate &&
                          child.DateOfBirth.Value.AddYears(12) > depdate))
                    {
                        isChildValid = false;
                    }
                }

                if (adultCount == 0)
                {
                    //throw new Exception("haloooo 3");
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> {"There Must be one adult"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }
                if (adultCount + childCount > 7)
                {
                    //throw new Exception("haloooo 4");
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Total adult and children passenger must be not more than seven"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }
                if (adultCount < infantCount)
                {
                    //throw new Exception("haloooo 5");
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Each infant must be accompanied by one adult"},
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        }
                    };
                }
                if (depdate > DateTime.Now.AddMonths(331).Date)
                {
                    //throw new Exception("haloooo 6");
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> {"Time of Departure Exceeds"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }
                if (!isInfantValid)
                {
                    //throw new Exception("haloooo 7");

                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Age of infant when traveling must be less than or equal 2 years old"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }
                if (!isChildValid)
                {
                    //throw new Exception("haloooo 8");

                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Age of chil when traveling must be between 2 and 12 years old"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }

                // [GET] Search Flight
                var client = CreateAgentClient();
                client.BaseUrl = new Uri("https://gosga.garuda-indonesia.com");
                string urlweb = @"";
                var searchReqAgent = new RestRequest(urlweb, Method.GET);
                searchReqAgent.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent.AddHeader("Host", "gosga.garuda-indonesia.com");
                var searchResAgent = client.Execute(searchReqAgent);
                var htmlsearchresp = searchResAgent.Content;

                urlweb = @"web/user/login/id";
                var searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                var searchResAgent0 = client.Execute(searchReqAgent0);
                htmlsearchresp = searchResAgent0.Content;


                var dict = DictionaryService.GetInstance();
                var destinationCountry = dict.GetAirportCountryCode(dest);

                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clientx = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/GarudaAccount/ChooseUserId", Method.GET);
                var userName = "";
                var reqTime = DateTime.UtcNow;
                var itin = new FlightItinerary();
                var successLogin = false;
                var counter = 0;

                while (!successLogin && counter < 31)
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && userName.Length == 0)
                    {
                        var accRs = (RestResponse) clientx.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }

                    if (userName.Length == 0)
                    {
                        return new BookFlightResult
                        {
                            Errors = new List<FlightError> {FlightError.TechnicalError},
                            ErrorMessages = new List<string> {"All usernames are used"}
                        };
                    }

                    var password = userName == "SA3ALEU1" ? "Standar123" : "Travorama1234";
                    counter++;
                    successLogin = Login(client, userName, password);
                }

                if (counter >= 31)
                {
                    TurnInUsername(clientx, userName);
                    return new BookFlightResult
                    {

                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false,
                        ErrorMessages = new List<string> {"Can't get id"}
                    };
                }
                
                urlweb = @"web/order/e-retail";
                searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/dashboard/welcome");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchResAgent0 = client.Execute(searchReqAgent0);
                var htmlX = searchResAgent0.Content;

                //POST 

                searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/dashboard/welcome");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");

                var airportScript = htmlX;
                var startIndex = airportScript.IndexOf("var airports");
                var endIndex = airportScript.IndexOf("var airportsdest");
                var scr = airportScript.SubstringBetween(startIndex + 15, endIndex - 3).Replace("\n", "").Replace("\t", "");
                var depAirport = GetGarudaAirportBooking(scr, origin).Replace("+", "%20");
                var arrAirport = GetGarudaAirportBooking(scr, dest).Replace("+", "%20");

                var postdata =
                    "Inputs%5BoriginDetail%5D=" + depAirport +
                    "&Inputs%5Borigin%5D=" + origin +
                    "&Inputs%5BdestDetail%5D=" + arrAirport +
                    "&Inputs%5Bdest%5D=" + dest +
                    "&Inputs%5BtripType%5D=o" +
                    "&Inputs%5BoutDate%5D=" + splittedFareId[4] + "-" + splittedFareId[3] + "-" + splittedFareId[2] + "-" + //2016-06-06
                    "&Inputs%5BretDate%5D=" + splittedFareId[4] + "-" + splittedFareId[3] + "-" + splittedFareId[2] + "-" +
                    "&Inputs%5Badults%5D=" + adultCount +
                    "&Inputs%5Bchilds%5D=" + childCount +
                    "&Inputs%5Binfants%5D=" + infantCount + 
                    "&Inputs%5BserviceClass%5D=eco" +
                    "&btnSubmit=+Cari";

                searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                searchResAgent0 = client.Execute(searchReqAgent0);

                try
                {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        ErrorMessages = new List<string> {"Web Layout Changed!"}
                    };
                }
                catch //(Exception e)
                {
                    LogOut("", client);
                    TurnInUsername(clientx, userName);

                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> {"Web Layout Changed!"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        }
                    };
                }
            }
                
        }

        private static void LogOut(string lasturlweb, RestClient clientAgent)
        {
            var urlweb = @"web/user/logout";
            var logoutReq = new RestRequest(urlweb, Method.GET);
            logoutReq.AddHeader("Referer", "https://gosga.garuda-indonesia.com/" + lasturlweb); //tergantung terakhirnya di mana
            logoutReq.AddHeader("Accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            logoutReq.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
            logoutReq.AddHeader("Host", "gosga.garuda-indonesia.com");
            var logoutResAgent0 = clientAgent.Execute(logoutReq);
        }

        private static void TurnInUsername(RestClient client, string username)
        {
            var accReq = new RestRequest("/api/GarudaAccount/LogOut?userId=" + username, Method.GET);
            var accRs = (RestResponse)client.Execute(accReq);
        }

        private class Airport
        {
            public string AirportCode { get; set; }
            public string AirportName { get; set; }
            public string CityCode { get; set; }
            public string CityName { get; set; }
            public string CountryCode { get; set; }
            public string CountryName { get; set; }

        }
        
        private static string GetGarudaAirportBooking(string scr, string code)
        {
            var airportScr = scr.Deserialize<List<List<string>>>();
            var arpt = "";
            foreach (var arp in airportScr.Where(arp => arp.ElementAt(0) == code))
            {
                arpt =
                    HttpUtility.UrlEncode(arp.ElementAt(3) + " (" + arp.ElementAt(2) + "), " + arp.ElementAt(1)
                                          + " (" + arp.ElementAt(0) + "), " + arp.ElementAt(5));
            }
            return arpt;
        }
        
     }
 }

