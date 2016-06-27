using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;
using HttpUtility = RestSharp.Extensions.MonoHttp.HttpUtility;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink

{
    internal partial class CitilinkWrapper
    {
        internal override decimal CurrencyGetter(string currency)
        {
            return Client.CurrencyGetter(currency);
        }

        private partial class CitilinkClientHandler
        {
            internal decimal CurrencyGetter(string currencyName)
            {

                DateTime depdate = DateTime.Now.AddDays(1);
                var returnPath = "";
                var client = CreateAgentClient();
                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clientx = new RestClient(cloudAppUrl);
                var userName = "";
                var successLogin = false;
                string origin = "SIN";
                string dest = "CGK";
                Decimal exchangeRate = 0;

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
                    IRestResponse searchResAgent0 = client.Execute(searchReqAgent0);
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
                    
                    var accReq = new RestRequest("/api/GarudaAccount/ChooseUserId", Method.GET);
                    var reqTime = DateTime.UtcNow;
                    var counter = 0;
                
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
                            return 0;
                        }
                        string msgLogin = "";
                        string viewstate = "";
                        var eventval = "";
                        string userId;
                        string currentDeposit;
                        const string password = "Standar123";
                        counter++;
                        Login(client);
                    }

                    if (counter >= 31)
                    {
                        TurnInUsername(clientx, userName);
                        return 0;
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
                        return 0;
                    }

                    var postdata =
                        "Inputs%5BoriginDetail%5D=" + HttpUtility.UrlEncode(depAirport) +
                        "&Inputs%5Borigin%5D=" + origin +
                        "&Inputs%5BdestDetail%5D=" + HttpUtility.UrlEncode(arrAirport) +
                        "&Inputs%5Bdest%5D=" + dest +
                        "&Inputs%5BtripType%5D=o" +
                        "&Inputs%5BoutDate%5D=" + depdate.Year + "-" + depdate.Month + "-" + depdate.Day +  //2016-06-06
                        "&Inputs%5BretDate%5D=" + depdate.Year + "-" + depdate.Month + "-" + depdate.Day +
                        "&Inputs%5Badults%5D=" + 1 +
                        "&Inputs%5Bchilds%5D=" + 0 +
                        "&Inputs%5Binfants%5D=" + 0 +
                        "&Inputs%5BserviceClass%5D=" + "eco" +
                        "&btnSubmit=+Cari";

                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = client.Execute(searchReqAgent0);
                    var htmlFlight = searchResAgent0.Content;
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    var htmlFlightList = (CQ)htmlFlight;
                    var tableFlight = htmlFlightList[".gtable"];
                    var rows = tableFlight[0].ChildElements.ToList()[1].ChildElements.ToList();
                    var selectedRows = new List<IDomObject>();

                    var currencyGot = false;
                    
                    var v = 2;
                    while (!currencyGot)
                    {
                        selectedRows.Clear();
                        var gotRows = false;
                        //pilih row/flight dulu dari paling atas
                        while (v < rows.Count() && !gotRows)
                        {
                            var rowClass = rows.ElementAt(v).GetAttribute("class");
                            var classSplitted = rowClass.Split(' ');
                            rowClass = String.Join(" ", classSplitted[0], classSplitted[1]);
                            selectedRows.Add(rows[v]);
                            var z = v + 1;
                            
                            while (z < rows.Count)
                            {
                                var thisRowClass = rows.ElementAt(z).GetAttribute("class");
                                var thisClassSplitted = thisRowClass.Split(' ');
                                thisRowClass = String.Join(" ", thisClassSplitted[0], thisClassSplitted[1]);
                                if (rowClass == thisRowClass)
                                {
                                    selectedRows.Add(rows[z]);
                                    z += 1;
                                }
                                else
                                {
                                    break; // masuk ke loop yg plg luar
                                }
                            }
                            v = z;
                            gotRows = true;
                        }

                        
                        var inputsdepoptlist = new List<string>();
                        var valuesdepoptlist = new List<string>();
                        for (var ind = 0; ind < selectedRows.Count; ind++)
                        {
                            var lastChild = selectedRows[ind].ChildElements.ToList().Count() - 1;
                            var input =
                                        selectedRows[ind].ChildElements.ToList()[lastChild].ChildElements.ToList()[1].GetAttribute(
                                            "name");
                            var value = selectedRows[ind].ChildElements.ToList()[lastChild].ChildElements.ToList()[1].GetAttribute(
                                    "value");
                            inputsdepoptlist.Add(input);
                            valuesdepoptlist.Add(value);
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
                        var htmlPrice = (CQ)searchResAgent0.Content;
                        returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                        var tableBreakdown = htmlPrice[".farebreakdown"];
                        var er= tableBreakdown[0].ChildElements.ToList()[0].
                            ChildElements.ToList()[1].InnerText.Split(' ');
                        exchangeRate = Convert.ToDecimal(er[er.Length - 1]);

                        if (exchangeRate != null)
                        {
                            currencyGot = true;
                        }
                    }

                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);
                    return exchangeRate;
                }
                catch //(Exception e)
                {
                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);
                    return 0;
                    

                    //return new BookFlightResult
                    //{
                    //    IsSuccess = false,
                    //    Errors = new List<FlightError> {FlightError.TechnicalError},
                    //    ErrorMessages = new List<string> {"Web Layout Changed! " + returnPath + successLogin + searchResAgent0.Content},
                    //    Status = new BookingStatusInfo
                    //    {
                    //        BookingStatus = BookingStatus.Failed
                    //    }
                    //};
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

