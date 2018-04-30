using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.Framework.Config;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Constant;
using RestSharp;
using HttpUtility = RestSharp.Extensions.MonoHttp.HttpUtility;

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        internal override Currency CurrencyGetter(string currency)
        {
            return Client.CurrencyGetter(currency);
        }

        private partial class GarudaClientHandler
        {
            internal Payment.Model.Currency CurrencyGetter(string currencyName)
            {
               
                string origin;
                var dest= "CGK";
                Payment.Model.Currency currclass;
                var currencyList = Payment.Model.Currency.GetAllCurrencies(Supplier.Garuda);
                if (!currencyList.TryGetValue(currencyName, out currclass))
                {
                    return new Payment.Model.Currency(currencyName, Supplier.Garuda);
                }

                switch (currencyName)
                {
                    case "SGD":
                        origin = "SIN";
                        break;
                    case "MYR":
                        origin = "KUL";
                        break;
                    case "KRW":
                        origin = "ICN";
                        break;
                    case "JPY":
                        origin = "NRT";
                        break;
                    case "THB":
                        origin = "BKK";
                        break;
                    case "AUD":
                        origin = "SYD";
                        break;
                    case "GBP":
                        origin = "LHR";
                        break;
                    case "EUR":
                        origin = "AMS";
                        break;
                    case "USD":
                        origin = "PNH";
                        break;
                    case "CNY":
                        origin = "PEK";
                        break;
                    case "HKD":
                        origin = "HKG";
                        break;
                    case "SAR":
                        origin = "JED";
                        break;
                    case "AED":
                        origin = "AUH";
                        break;
                    default:
                        origin = "CGK";
                        break;
                }

                //var depdate = new DateTime(2016, 12, 14);
                DateTime depdate = DateTime.Now.AddMonths(6);
                var returnPath = "";
                var client = CreateAgentClient();
                var cloudAppUrl = EnvVariables.Get("general", "cloudAppUrl");
                var clientx = new RestClient(cloudAppUrl);
                var userName = "";
                var successLogin = false;
                
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

                    //successLogin = Login(client, "SA3ALEU5", "Standar123", out returnPath);
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
                            return new Payment.Model.Currency(currencyName, Supplier.Garuda);
                        }

                        const string password = "Standar123";
                        counter++;
                        successLogin = Login(client, userName, password, out returnPath);
                    }

                    if (counter >= 31)
                    {
                        TurnInUsername(clientx, userName);
                        return new Payment.Model.Currency(currencyName, Supplier.Garuda);
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
                    var scheduledNotFound = true;
                    var loopday = 0;
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
                        return new Payment.Model.Currency(currencyName, Supplier.Garuda);
                    }

                    string postdata;
                    var htmlFlight = searchResAgent0.Content;
                    while (scheduledNotFound && loopday < 7)
                    {
                        postdata =
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
                        htmlFlight = searchResAgent0.Content;
                        scheduledNotFound = htmlFlight.Contains("Tidak memperoleh daftar outbound flight dari sistem reservasi.");
                        returnPath = searchResAgent0.ResponseUri.AbsolutePath;
                        if (scheduledNotFound)
                        {
                            searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                            searchReqAgent0.AddHeader("Accept",
                                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                            searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                            searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                            searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                            searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/e-retail");
                            depdate = depdate.AddDays(8);
                        }
                        loopday++;
                    }

                    if (loopday == 7)
                    {
                        LogOut(returnPath, client);
                        TurnInUsername(clientx, userName);
                        return new Payment.Model.Currency(currencyName, Supplier.Garuda);
                    }
                    
                    var htmlFlightList = (CQ)htmlFlight;
                    var tableFlight = htmlFlightList[".gtable"];
                    var rows = tableFlight[0].ChildElements.ToList()[1].ChildElements.ToList();
                    var selectedRows = new List<IDomObject>();

                    var currencyGot = false;
                    
                    var v = 2;

                    loopday = 0;
                    while (!currencyGot && loopday < 7)
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

                        if (v <= rows.Count)
                        {
                            var inputsdepoptlist = new List<string>();
                            var valuesdepoptlist = new List<string>();
                            for (var ind = 0; ind < selectedRows.Count; ind++)
                            {
                                var totalChildren = selectedRows[ind].ChildElements.ToList().Count();
                                var lastChild = 0;
                                for (var b = 0; b < totalChildren; b++)
                                {
                                    if (selectedRows[ind].ChildElements.ToList()[b].ChildElements.Count() == 2)
                                    {
                                        lastChild = b;
                                    }
                                }

                                var input =
                                    selectedRows[ind].ChildElements.ToList()[lastChild].ChildElements.ToList()[1]
                                        .GetAttribute(
                                            "name");
                                var value = selectedRows[ind].ChildElements.ToList()[lastChild].ChildElements.ToList()[1
                                    ].GetAttribute(
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
                            searchReqAgent0.AddHeader("Referer",
                                "https://gosga.garuda-indonesia.com/web/order/selectFlight");
                            searchReqAgent0.AddHeader("Accept",
                                "text/html, */*; q=0.01");
                            searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                            searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                            searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                            searchReqAgent0.AddHeader("X-Requested-With", "XMLHttpRequest");
                            searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata,
                                ParameterType.RequestBody);
                            searchResAgent0 = client.Execute(searchReqAgent0);
                            htmlFlight = searchResAgent0.Content;
                            returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                            urlweb = @"web/order/checkFare";
                            searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                            searchReqAgent0.AddHeader("Referer",
                                "https://gosga.garuda-indonesia.com/web/order/selectFlight");
                            searchReqAgent0.AddHeader("Accept",
                                "text/html, */*; q=0.01");
                            searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                            searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                            searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                            searchReqAgent0.AddHeader("X-Requested-With", "XMLHttpRequest");
                            postdata += "&btnSubmit=1";
                            searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata,
                                ParameterType.RequestBody);
                            searchResAgent0 = client.Execute(searchReqAgent0);
                            var htmlPrice = (CQ) searchResAgent0.Content;
                            returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                            var tableBreakdown = htmlPrice[".farebreakdown"];
                            if (tableBreakdown.Length != 0)
                            {
                                var er = tableBreakdown[0].ChildElements.ToList()[0].
                                    ChildElements.ToList()[0].ChildElements.ToList()[1].InnerText.Split(' ');

                                var b = decimal.TryParse(er[er.Length - 1], out exchangeRate);
                                if (b)
                                {
                                    currencyGot = true;
                                }
                            }
                        }
                        else
                        {
                            if (loopday < 7)
                            {
                                urlweb = @"web/order/selectFlight";
                                searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                                searchReqAgent0.AddHeader("Referer",
                                    "https://gosga.garuda-indonesia.com/web/order/selectFlight");
                                searchReqAgent0.AddHeader("Accept",
                                    "text/html, */*; q=0.01");
                                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                                searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                                searchReqAgent0.AddHeader("X-Requested-With", "XMLHttpRequest");
                                postdata = "Inputs%5BdepOptKey%5D=&btnBack=++++++Sebelumnya++++++";
                                searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata,
                                    ParameterType.RequestBody);
                                searchResAgent0 = client.Execute(searchReqAgent0);
                                var htmlSelectFlight = (CQ)searchResAgent0.Content;
                                returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                                depdate = depdate.AddDays(8);
                                postdata =
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

                                urlweb = @"web/order/e-retail";
                                searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/dashboard/welcome");
                                searchReqAgent0.AddHeader("Accept",
                                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                                searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                                searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                                searchResAgent0 = client.Execute(searchReqAgent0);
                                htmlFlight = searchResAgent0.Content;
                                htmlFlightList = (CQ)htmlFlight;
                                tableFlight = htmlFlightList[".gtable"];
                                rows = tableFlight[0].ChildElements.ToList()[1].ChildElements.ToList();
                                v = 2;
                            }
                            loopday++;
                        }
                    }

                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);
                    if (loopday == 7)
                    {
                        return new Payment.Model.Currency(currencyName, Supplier.Garuda);
                    }
                    Payment.Model.Currency.SetRate(currencyName, exchangeRate, Supplier.Garuda);
                    var currs = new Payment.Model.Currency(currencyName, exchangeRate) { Supplier = Supplier.Garuda };
                    Console.WriteLine("The Rate for " + currencyName + " is: " + exchangeRate);
                    return currs;  
                    
                }
                catch //(Exception e)
                {
                    LogOut(returnPath, client);
                    TurnInUsername(clientx, userName);
                    return new Payment.Model.Currency(currencyName, Supplier.Garuda);
                }
            }
        }
     }
 }

