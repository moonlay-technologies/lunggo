using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

                string origin, dest;
                DateTime depdate;
                int adultCount, childCount, infantCount;
                var bookingTimeLimit = "";
                var bookingReference = "";
               
                List<string> listflight;
                CabinClass cabinClass;
                var splittedFareId = bookInfo.Itinerary.FareId.Split('+');
                string price;

                try
                {
                    origin = splittedFareId[0]; //CGK
                    dest = splittedFareId[1]; // SIN
                    depdate = new DateTime(Convert.ToInt32(splittedFareId[4]), Convert.ToInt32(splittedFareId[3]),
                        Convert.ToInt32(splittedFareId[2]), Convert.ToInt32(splittedFareId[5]), Convert.ToInt32(splittedFareId[6]), 0);
                    adultCount = Convert.ToInt32(splittedFareId[7]);
                    childCount = Convert.ToInt32(splittedFareId[8]);
                    infantCount = Convert.ToInt32(splittedFareId[9]);
                    cabinClass = FlightService.ParseCabinClass(splittedFareId[10]);
                    price = splittedFareId[11];
                    listflight = splittedFareId[12].Split('|').ToList();
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

                var returnPath = "";
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
                if (adultCount + childCount > 9)
                {
                    //throw new Exception("haloooo 4");
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Total adult and children passenger must be not more than nine"},
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
                            new List<string> {"Age of child when traveling must be between 2 and 12 years old"},
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }
                var client = CreateAgentClient();
                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clientx = new RestClient(cloudAppUrl);
                var userName = "";
                var successLogin = false;
                IRestResponse searchResAgent0 = null;
                try
                {
                // [GET] Search Flight
                
                client.BaseUrl = new Uri("https://gosga.garuda-indonesia.com");
                string urlweb = @"";
                var searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchResAgent0 = client.Execute(searchReqAgent0);
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                urlweb = @"web/user/login/id";
                searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchResAgent0 = client.Execute(searchReqAgent0);
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                var dict = DictionaryService.GetInstance();
                    
                var accReq = new RestRequest("/api/GarudaAccount/ChooseUserId", Method.GET);
                
                var reqTime = DateTime.UtcNow;
                var newitin = new FlightItinerary();
                
                var counter = 0;
                
                //successLogin = Login(client, "SA3ALEU1", "Standar123", out returnPath);
                while (!successLogin && counter < 31)
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && returnPath != "/web/dashboard/welcome")
                    {

                        var accRs = (RestResponse)clientx.Execute(accReq);
                        var lastUserId = userName;
                        userName = accRs.Content.Trim('"');
                        if (returnPath != "/web/dashboard/welcome")
                        {
                            TurnInUsername(clientx, lastUserId);
                        }
                        if (userName.Length != 0)
                        {
                            returnPath = "/web/dashboard/welcome";
                        }

                    }

                    if (userName.Length == 0)
                    {
                        return new BookFlightResult
                        {
                            Errors = new List<FlightError> { FlightError.TechnicalError },
                            ErrorMessages = new List<string> { "All usernames are used " + returnPath }
                        };
                    }

                    var password = userName == "SA3ALEU1" ? "Standar123" : "Travorama1234";
                    counter++;
                    successLogin = Login(client, userName, password, out returnPath);
                }

                if (counter >= 31)
                {
                    TurnInUsername(clientx, userName);
                    return new BookFlightResult
                    {

                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Can't get id " + returnPath + userName }
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
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;

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
                var depAirport = GetGarudaAirportBooking(scr, origin);
                var arrAirport = GetGarudaAirportBooking(scr, dest);

                if (depAirport.Length == 0 || arrAirport.Length == 0)
                {
                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> { "Airports are not available in agent site " + returnPath}
                    };
                }
                string cabinstring;

                if (cabinClass == CabinClass.Economy)
                {
                    cabinstring = "eco";
                }
                else if (cabinClass == CabinClass.Business)
                {
                    cabinstring = "exe";
                }
                else
                {
                    cabinstring = "1st";
                }

                var postdata =
                    "Inputs%5BoriginDetail%5D=" + HttpUtility.UrlEncode(depAirport) +
                    "&Inputs%5Borigin%5D=" + origin +
                    "&Inputs%5BdestDetail%5D=" + HttpUtility.UrlEncode(arrAirport) +
                    "&Inputs%5Bdest%5D=" + dest +
                    "&Inputs%5BtripType%5D=o" +
                    "&Inputs%5BoutDate%5D=" + splittedFareId[4] + "-" + splittedFareId[3] + "-" + splittedFareId[2] +  //2016-06-06
                    "&Inputs%5BretDate%5D=" + splittedFareId[4] + "-" + splittedFareId[3] + "-" + splittedFareId[2] + 
                    "&Inputs%5Badults%5D=" + adultCount +
                    "&Inputs%5Bchilds%5D=" + childCount +
                    "&Inputs%5Binfants%5D=" + infantCount + 
                    "&Inputs%5BserviceClass%5D=" + cabinstring +
                    "&btnSubmit=+Cari";

                searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                searchResAgent0 = client.Execute(searchReqAgent0);
                var htmlFlight = searchResAgent0.Content;
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                
                    var htmlFlightList = (CQ) htmlFlight;
                    var tableFlight = htmlFlightList[".gtable"];
                    var rows = tableFlight[0].ChildElements.ToList()[1].ChildElements.ToList();
                    //var selectedRows = (from flightNo in listflight from row in rows let x = flightNo.Replace("-", "") let y = row.ChildElements.ToList()[1].InnerText where flightNo.Replace("-", "") == row.ChildElements.ToList()[1].InnerText select row).Cast<IDomObject>().ToList();
                    var selectedRows =  new List<IDomObject>();
                    
                    var v = 2;
                    while (v < rows.Count())
                    {
                        var currentRow = rows.ElementAt(v);
                        var currentPlane = currentRow.ChildElements.ToList()[1].InnerText;
                        var w = 0;
                        if (currentPlane == listflight.ElementAt(w).Replace("-", ""))
                        {
                            selectedRows.Add(rows[v]);
                            var z =  v + 1;
                            w += 1;
                            while (z < rows.Count && z < v + listflight.Count)
                            {
                                currentPlane = rows.ElementAt(z).ChildElements.ToList()[0].InnerText;
                                if (currentPlane == listflight.ElementAt(w).Replace("-", ""))
                                {
                                    selectedRows.Add(rows[z]);
                                }
                                else
                                {
                                    selectedRows.Clear();
                                    break; // masuk ke loop yg plg luar
                                }
                                z += 1;
                                w += 1;
                            }
                            v = z;
                        }
                        else
                        {
                            v += 1;
                        }
                    }

                    var depdateTemp = depdate;
                    var flightstring = "";
                    var segments = new List<FlightSegment>();
                    var inputsdepoptlist = new List<string>();
                    var valuesdepoptlist = new List<string>();
                    for(var ind = 0; ind < selectedRows.Count; ind++)
                    {
                        var ct = 1;
                        if (ind != 0)
                        {
                            ct = 0;
                        }
                        var airlineCode = selectedRows[ind].ChildElements.ToList()[ct].InnerText.SubstringBetween(0, 2);
                        var flightNumber = selectedRows[ind].ChildElements.ToList()[ct].InnerText.SubstringBetween(2, 5);
                        flightstring += airlineCode + "-" + flightNumber + "|";
                        var depdata = selectedRows[ind].ChildElements.ToList()[ct + 1].InnerText;
                        var oriAirport = depdata.SubstringBetween(0, 3);
                        var oriHour = depdata.SubstringBetween(depdata.Length-5, depdata.Length);
                        DateTime oriHr;
                        if (Convert.ToInt32(oriHour.Split('.')[0]) < depdateTemp.Hour)
                        {
                            var w = depdateTemp.AddDays(1);
                            oriHr = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                Convert.ToInt32(oriHour.Split('.')[0]),
                                Convert.ToInt32(oriHour.Split('.')[1]), 0), DateTimeKind.Utc);
                        }
                        else 
                        {
                            oriHr = DateTime.SpecifyKind(new DateTime(depdateTemp.Year, depdateTemp.Month, depdateTemp.Day,
                                        Convert.ToInt32(oriHour.Split('.')[0]),
                                        Convert.ToInt32(oriHour.Split('.')[1]), 0), DateTimeKind.Utc); 
                        }

                        var arrdata = selectedRows[ind].ChildElements.ToList()[ct + 2].InnerText;
                        var desAirport = arrdata.SubstringBetween(0, 3);
                        var desHour = arrdata.SubstringBetween(arrdata.Length-5, arrdata.Length);
                        DateTime arrHr;
                        if (Convert.ToInt32(desHour.Split('.')[0]) < oriHr.Hour)
                        {
                            var w = oriHr.AddDays(1);
                            arrHr = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                                Convert.ToInt32(desHour.Split('.')[0]),
                                Convert.ToInt32(desHour.Split('.')[1]), 0), DateTimeKind.Utc);
                            depdateTemp = arrHr;
                        }
                        else
                        {
                            var w = oriHr;
                            arrHr = DateTime.SpecifyKind(new DateTime(w.Year, w.Month, w.Day,
                               Convert.ToInt32(desHour.Split('.')[0]),
                               Convert.ToInt32(desHour.Split('.')[1]), 0), DateTimeKind.Utc);
                            depdateTemp = arrHr;
                        }
                        TimeSpan duration = arrHr.AddHours(-(dict.GetAirportTimeZone(desAirport))) -
                                            oriHr.AddHours(-(dict.GetAirportTimeZone(oriAirport)));

                        segments.Add(new FlightSegment
                        {
                            DepartureAirport = oriAirport,
                            DepartureTime = oriHr,
                            AirlineCode = airlineCode,
                            FlightNumber = flightNumber,
                            ArrivalAirport = desAirport,
                            ArrivalTime = arrHr,
                            Duration = duration,
                            CabinClass = cabinClass,
                        });

                        var run = ct + 3;
                        var flag = false;
                        while (run < selectedRows[ind].ChildElements.ToList().Count && !flag)
                        {
                            var temp = selectedRows[ind].ChildElements.ToList()[run].InnerText;
                            if (temp != "-")
                            {
                                var input =
                                    selectedRows[ind].ChildElements.ToList()[run].ChildElements.ToList()[1].GetAttribute(
                                        "name");
                                var value = selectedRows[ind].ChildElements.ToList()[run].ChildElements.ToList()[1].GetAttribute(
                                        "value");
                                inputsdepoptlist.Add(input);
                                valuesdepoptlist.Add(value);
                                flag = true;
                            }
                            else
                            {
                                run++;
                            }
                        }
                    }

                    var classDeptOptKey = selectedRows[0].GetAttribute("class");
                    var valueDeptOptKey = classDeptOptKey[classDeptOptKey.Length - 1];
                    //POST UNTUK CEK HARGA
                    postdata = "";
                    for (var ind = 1; ind < inputsdepoptlist.Count; ind++)
                    {
                        postdata += "&" + HttpUtility.UrlEncode(inputsdepoptlist[ind]) + "="
                            + HttpUtility.UrlEncode(valuesdepoptlist[ind]); 
                    }

                    postdata = HttpUtility.UrlEncode(inputsdepoptlist[0]) + "="
                            + HttpUtility.UrlEncode(valuesdepoptlist[0]) + postdata + "&" 
                            + HttpUtility.UrlEncode("Inputs[depOptKey]") + "="
                            + valueDeptOptKey;

                    urlweb = @"web/order/checkFare";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/selectFlight");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html, */*; q=0.01");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("X-Requested-With", "XMLHttpRequest");
                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = client.Execute(searchReqAgent0);
                    htmlFlight = searchResAgent0.Content;
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    urlweb = @"web/order/checkFare";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/selectFlight");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html, */*; q=0.01");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("X-Requested-With", "XMLHttpRequest");
                    postdata += "&btnSubmit=1";
                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = client.Execute(searchReqAgent0);
                    var htmlPrice = (CQ) searchResAgent0.Content;
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    var classprice = htmlPrice["#sidebarTotal"].Text().Replace(",","");
                    var newprice = Convert.ToDecimal(classprice);

                    var newFareId = origin + "+" + dest + "+" + segments.ElementAt(0).DepartureTime.Day + "+" +
                            segments.ElementAt(0).DepartureTime.Month + "+" +
                            segments.ElementAt(0).DepartureTime.Year + "+" +
                            segments.ElementAt(0).DepartureTime.Hour + "+" + 
                            segments.ElementAt(0).DepartureTime.Minute + "+" +
                            adultCount + "+" + childCount + "+" + infantCount + "+" + splittedFareId[10] + "+" +
                            price + "+" + flightstring.SubstringBetween(0, flightstring.Length - 1);

                    newitin = new FlightItinerary
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
                        RequestedCabinClass = cabinClass,
                        TripType = TripType.OneWay,
                        Supplier = Supplier.Garuda,
                        SupplierCurrency = "IDR",
                        SupplierPrice = newprice,
                        FareId = newFareId,
                        LocalCurrency = "IDR",
                        Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = origin,
                                    DestinationAirport = dest,
                                    DepartureDate = depdate.Date,
                                    Segments = segments
                                }
                            }
                    };

                    var isItinChanged = !newitin.Identical(bookInfo.Itinerary);

                    if (isItinChanged)
                    {
                        LogOut(returnPath, client);
                        TurnInUsername(clientx, userName);

                        return new BookFlightResult
                        {
                            IsSuccess = false,
                            Errors = new List<FlightError> { FlightError.TechnicalError },
                            Status = null,
                            ErrorMessages = new List<string> { "Itinerary is Changed! " + returnPath },
                            NewPrice = newprice,
                            IsValid = true,
                            IsPriceChanged = newprice != bookInfo.Itinerary.SupplierPrice,
                            NewItinerary = newitin,
                            IsItineraryChanged = isItinChanged

                        };
                    }

                    if (newprice != Convert.ToDecimal(price))
                    {
                        LogOut(returnPath, client);
                        TurnInUsername(clientx, userName);

                        return new BookFlightResult
                        {
                            IsSuccess = false,
                            Errors = new List<FlightError> { FlightError.TechnicalError },
                            Status = null,
                            ErrorMessages = new List<string> { "Price is Changed! " + returnPath },
                            NewPrice = newprice,
                            IsValid = true,
                            IsPriceChanged = newprice != bookInfo.Itinerary.SupplierPrice,
                            NewItinerary = newitin,
                            IsItineraryChanged = isItinChanged

                        };
                    }

                    urlweb = @"web/order/pax";
                    searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/selectFlight");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchResAgent0 = client.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    var orderedPassengers = bookInfo.Passengers.OrderBy(x => x.Type);
                    var dataPassenger = "";
                    var adultIndex = 1;
                    var childIndex = 1;
                    var infantIndex = 1;

                   
                    for (var i = 0; i < orderedPassengers.Count(); i++)
                    {
                        var title = "";
                        switch (orderedPassengers.ElementAt(i).Title)
                        {
                            case Title.Miss:
                                title = orderedPassengers.ElementAt(i).Type == PassengerType.Adult ? "MS" : "MISS";
                                break;
                            case Title.Mister:
                                title = orderedPassengers.ElementAt(i).Type == PassengerType.Adult ? "MR" : "MSTR";
                                break;
                            case Title.Mistress:
                                title = "MRS";
                                break;
                        }

                        var name = orderedPassengers.ElementAt(i).FirstName.Split(' ').ToList();
                        var firstname = name[0];
                        var middlename = String.Join("", name.Skip(1).ToArray());
                        switch (orderedPassengers.ElementAt(i).Type)
                        {
                            case PassengerType.Adult:
                                dataPassenger +=
                                    "&Inputs%5Badt%5D%5B"+ adultIndex + "%5D%5Bnameprefix%5D=" + title +
                                    "&Inputs%5Badt%5D%5B"+ adultIndex + "%5D%5Bfirstname%5D=" + firstname +
                                    "&Inputs%5Badt%5D%5B"+ adultIndex + "%5D%5Bmiddlename%5D=" + middlename +
                                    "&Inputs%5Badt%5D%5B"+ adultIndex + "%5D%5Blastname%5D=" + 
                                        orderedPassengers.ElementAt(i).LastName +
                                    "&Inputs%5Badt%5D%5B"+ adultIndex + "%5D%5Bgff%5D=";

                                adultIndex += 1;
                                        
                                break;
                            case PassengerType.Child:
                                dataPassenger +=
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5Bnameprefix%5D=" + title +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5Bfirstname%5D=" + firstname +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5Bmiddlename%5D=" + middlename +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5Blastname%5D=" + 
                                        orderedPassengers.ElementAt(i).LastName +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5Bgff%5D=" +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5BbirthDD%5D=" + 
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5BbirthMM%5D=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MM") +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5BbirthYY%5D=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year +
                                    "&Inputs%5Bchd%5D%5B"+ childIndex + "%5D%5BChildDate%5D="
                                        + orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year + "-" 
                                        + orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MM") + "-"
                                        + orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") ;

                                childIndex++;
                                break;
                            case PassengerType.Infant:
                                dataPassenger +=
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5Bnameprefix%5D=" + title +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5Bfirstname%5D=" + firstname +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5Bmiddlename%5D=" + middlename +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5Blastname%5D=" + 
                                        orderedPassengers.ElementAt(i).LastName +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5Bgff%5D=" +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5BbirthDD%5D=" + 
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5BbirthMM%5D=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MM") +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5BbirthYY%5D=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year +
                                    "&Inputs%5Binf%5D%5B"+ infantIndex + "%5D%5BChildDate%5D="
                                        + orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year + "-" 
                                        + orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MM") + "-"
                                        + orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") ;
                                infantIndex++;
                                break;
                        }
                    }

                    dataPassenger +=
                        "&Inputs%5BaddAdult%5D=" +
                        "&Inputs%5BaddChild%5D=" +
                        "&Inputs%5BaddInfant%5D=" +
                        "&Inputs%5BcontactPaxKey%5D=1" +
                        "&Inputs%5Bcontact_phone%5D=" +
                        "&Inputs%5Bcontact_mobileph%5D=" + bookInfo.ContactData.CountryCode + bookInfo.ContactData.Phone +
                        "&btnContinue=++++++Lanjutkan++++++";

                    urlweb = @"web/order/pax";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/pax");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Cache-Control", "max-age=0");
                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded",dataPassenger, ParameterType.RequestBody);
                    searchResAgent0 = client.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    if (returnPath != "/web/order/purchase")
                    {
                        LogOut(returnPath, client);
                        TurnInUsername(clientx, userName);
                        return new BookFlightResult
                        {
                            IsSuccess = false,
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            Status = new BookingStatusInfo
                            {
                                BookingStatus = BookingStatus.Failed
                            },
                            ErrorMessages = new List<string> { "Total characters of infant+adult name may exceed 32 " + returnPath }
                        };
                    }

                    postdata = "Inputs%5BpayType%5D=OL&Inputs%5BdebitBank%5D=&Inputs%5BkbUsername%5D=&Inputs%5BacceptTerm%5D=yes&btnContinue=++++++Lanjutkan++++++";
                    urlweb = @"web/order/purchase";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/purchase");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Cache-Control", "max-age=0");
                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = client.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    var htmlConfirmation = (CQ) searchResAgent0.Content;
                    var dataConfirmation = htmlConfirmation["#Confirm"];
                    bookingReference = dataConfirmation[0].ChildElements.ToList()[2].ChildElements.ToList()[0].
                        ChildElements.ToList()[3].InnerText;
                    bookingTimeLimit = dataConfirmation[0].ChildElements.ToList()[2].ChildElements.ToList()[0].
                        ChildElements.ToList()[9].InnerText;

                    var xy = bookingTimeLimit.SubstringBetween(0, bookingTimeLimit.Length - 4);

                    const string format = "ddMMMyyyy HH:mm";
                    var timeLimit =
                        DateTime.SpecifyKind(
                            DateTime.ParseExact(
                                bookingTimeLimit.SubstringBetween(0, bookingTimeLimit.Length - 4), format,
                                CultureInfo.InvariantCulture), DateTimeKind.Utc).AddHours(-7);

                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);
                    if (bookingReference.Length != 0)
                    {
                        return new BookFlightResult
                        {
                            IsSuccess = true,
                            Status = new BookingStatusInfo
                            {
                                BookingId = bookingReference,
                                TimeLimit = timeLimit,
                                BookingStatus = BookingStatus.Booked
                            },
                            IsValid = true,
                            IsItineraryChanged = false,
                            IsPriceChanged = false,
                            NewItinerary = newitin
                        };
                    }

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
                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);

                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> {"Web Layout Changed! " + returnPath + successLogin + searchResAgent0.Content},
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
            logoutReq.AddHeader("Referer", "https://gosga.garuda-indonesia.com" + lasturlweb); //tergantung terakhirnya di mana
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

