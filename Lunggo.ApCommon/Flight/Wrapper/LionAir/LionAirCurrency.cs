using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using RestSharp;
using HttpUtility = RestSharp.Extensions.MonoHttp.HttpUtility;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir

{
    internal partial class LionAirWrapper
    {
        internal override Currency CurrencyGetter(string currency)
        {
            return Client.CurrencyGetter(currency);
        }

        private partial class LionAirClientHandler
        {
            internal Currency CurrencyGetter(string currencyName)
            {
                
                string originAirport;
                var destinationAirport = "CGK";

                Currency currclass;
                var currencyList = Currency.GetAllCurrencies(Payment.Constant.Supplier.LionAir);
                if (!currencyList.TryGetValue(currencyName, out currclass))
                {
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);
                }

                switch (currencyName)
                {
                    case "SGD":
                        originAirport = "SIN";
                        break;
                    case "MYR":
                        originAirport = "KUL";
                        break;
                    default:
                        originAirport = "CGK";
                        break;
                }
                
                DateTime depdate = DateTime.Now.AddMonths(6);
                var client = CreateCustomerClient();
                
                // Calling The Zeroth Page
                client.BaseUrl = new Uri("http://www.lionair.co.id");
                const string url0 = @"Default.aspx";
                var searchRequest0 = new RestRequest(url0, Method.GET);
                searchRequest0.AddHeader("Referer", "http://www.lionair.co.id");

                var searchResponse0 = client.Execute(searchRequest0);

                if (searchResponse0.ResponseUri.AbsolutePath != "/Default.aspx" &&
                    (searchResponse0.StatusCode == HttpStatusCode.OK ||
                        searchResponse0.StatusCode == HttpStatusCode.Redirect))
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);

                // Calling The First Page
                client.BaseUrl = new Uri("https://secure2.lionair.co.id");
                const string url = @"lionairibe2/OnlineBooking.aspx";
                var searchRequest = new RestRequest(url, Method.GET);
                searchRequest.AddHeader("Referer", "http://www.lionair.co.id");
                searchRequest.AddQueryParameter("trip_type", "one way");
                searchRequest.AddQueryParameter("date_flexibility", "fixed");
                searchRequest.AddQueryParameter("depart", originAirport);
                searchRequest.AddQueryParameter("dest.1", destinationAirport);
                searchRequest.AddQueryParameter("date.0", depdate.ToString("ddMMM"));
                searchRequest.AddQueryParameter("date.1", depdate.ToString("ddMMM"));
                searchRequest.AddQueryParameter("persons.0", "1");
                searchRequest.AddQueryParameter("persons.1", "0");
                searchRequest.AddQueryParameter("persons.2", "0");

                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                        SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                var searchResponse = client.Execute(searchRequest);

                if (searchResponse.ResponseUri.AbsolutePath != "/lionairibe2/OnlineBooking.aspx" &&
                    (searchResponse.StatusCode == HttpStatusCode.OK ||
                     searchResponse.StatusCode == HttpStatusCode.Redirect))
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);
                
                //Calling The Second Page
                const string url2 = @"lionairibe2/OnlineBooking.aspx";
                var searchRequest2 = new RestRequest(url2, Method.GET);
                searchRequest2.AddHeader("Referer", "https://secure2.lionair.co.id");

                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                        SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                var searchResponse2 = client.Execute(searchRequest2);
                if (searchResponse2.ResponseUri.AbsolutePath != "/lionairibe2/OnlineBooking.aspx" &&
                    (searchResponse2.StatusCode == HttpStatusCode.OK ||
                        searchResponse2.StatusCode == HttpStatusCode.Redirect))
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);

                var html2 = (CQ)searchResponse2.Content;
                var table = html2[".flight-matrix"];
                var rows = table[0].ChildElements.ToList()[0].ChildElements.ToList();
               
                var listflight = new List<string>();

                var currencyGot = false;
                var i = 1;
                var j = 1;
                decimal rateInCurr = 1;
                decimal agentprice = 0;

                try
                {
                    while (!currencyGot && i < rows.Count)
                    {
                        if (rows.ElementAt(i).ChildElements.ToList().Count == 1)
                        {
                            i++;
                        }
                        else
                        {

                            var columns = rows[i].ChildElements.ToList();

                            while (j < columns.Count)
                            {
                                if (rows[i].ChildElements.ToList()[j].InnerText == "N/A" ||
                                    rows[i].ChildElements.ToList()[j].InnerText
                                    == "Sold Out")
                                {
                                    j++;
                                }
                                else
                                {
                                    var data =
                                        rows[i].ChildElements.ToList()[j].ChildElements.ToList()[0].ChildElements.ToList
                                            ()[1].InnerText;
                                    var curr =
                                        rows[i].ChildElements.ToList()[j].ChildElements.ToList()[0].ChildElements.ToList
                                            ()[1].
                                            ChildElements.ToList()[0].InnerText;
                                    //if (curr == currencyName)
                                    //{
                                    //    currencyName = curr;
                                    //}
                                    //else if (curr == "IDR")
                                    //{
                                    //    return 0;
                                    //}
                                    
                                    //var data1 = "0";
                                    var success = decimal.TryParse(data, out rateInCurr);
                                    if (success)
                                    {
                                        //currencyGot = true;
                                        j = columns.Count;
                                        var flight = rows.ElementAt(i).ChildElements.ToList()[0].
                                            ChildElements.ToList()[0].ChildElements.ToList()[2].InnerText;
                                        flight = flight.TrimEnd(' ');
                                        listflight.Add(flight);
                                        var k = i + 1;
                                        var flightGot = false;
                                        var currRowClass = rows.ElementAt(i).GetAttribute("id");
                                        while (k < rows.Count && !flightGot)
                                        {
                                            var nextRowClass = rows.ElementAt(k).GetAttribute("id");

                                            if (nextRowClass.SubstringBetween(0, nextRowClass.Length - 2) ==
                                                currRowClass.SubstringBetween(0, currRowClass.Length - 2))
                                            {
                                                var flightnext = rows.ElementAt(k).ChildElements.ToList()[0].
                                                    ChildElements.ToList()[0].ChildElements.ToList()[2].InnerText;
                                                flightnext = flightnext.TrimEnd(' ');
                                                listflight.Add(flightnext);
                                                k++;
                                            }
                                            else
                                            {
                                                flightGot = true;
                                                currencyGot = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);
                }

                string userName;
                string userId;

                var agentClient = CreateAgentClient();
                var succeedLogin = Login(client, out userName, out userId);
                if (!succeedLogin)
                {
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);
                }

                //GET PAGE CONST ID
                var startind = userId.IndexOf("consID");
                var cid = userId.SubstringBetween(startind, userId.Length);
                var urlB = @"/LionAirAgentsIBE/OnlineBooking.aspx?" + cid;
                var searchRequestB = new RestRequest(urlB, Method.GET);
                searchRequestB.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequestB.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequestB.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx");
                Thread.Sleep(3000);
                var searchResponseB = agentClient.Execute(searchRequestB);

                //GET PAGE ONLINE BOOKING (PAGE MILIH PESAWAT)
                const string url3 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                var searchRequest3 = new RestRequest(url3, Method.GET);
                searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest3.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest3.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx");
                Thread.Sleep(3000);
                var searchResponse3 = agentClient.Execute(searchRequest3);
                //var vs = new CQ();
                var html3 = searchResponse3.Content;
                var vs = (CQ)html3;
                var vs4 = HttpUtility.UrlEncode(vs["#__VIEWSTATE"].Attr("value"));

                //POST FOR PAGE AVAILABLE FLIGHTS AND PRICE 

                const string url4 = @"LionAirAgentsIBE/Step1.aspx";
                var searchRequest4 = new RestRequest(url4, Method.POST);
                searchRequest4.AddHeader("Accept-Encoding", "gzip, deflate");
                searchRequest4.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest4.AddHeader("Referer", "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                searchRequest4.AddHeader("Origin", "https://agent.lionair.co.id");

                var cityairport = new GetCityAirportPair();
                string cityDep;
                string cityArr;

                var check1 = cityairport.GetCity(originAirport, out cityDep);
                var check2 = cityairport.GetCity(destinationAirport, out cityArr);
                var postData4 =
                    @"__EVENTTARGET=UcFlightSelection%24lbSearch" + @"&__EVENTARGUMENT=" + @"&__VIEWSTATE=" + vs4 +
                    @"&UcFlightSelection%24TripType=rbOneWay" +
                    @"&UcFlightSelection%24DateFlexibility=rbMustTravel" +
                    @"&UcFlightSelection%24txtSelOri=" + originAirport +
                    @"&UcFlightSelection%24txtOri=" + cityDep + "+%28" + originAirport + "%29" +
                    @"&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "+" + depdate.Year +
                    @"&UcFlightSelection%24ddlDepDay=" + depdate.Day +
                    @"&UcFlightSelection%24ddlADTCount=" + 1 +
                    @"&UcFlightSelection%24txtSelDes=" + destinationAirport +
                    @"&UcFlightSelection%24txtDes=" + cityArr + "+%28" + destinationAirport + "%29" +
                    @"&UcFlightSelection%24ddlCNNCount=" + 0 +
                    @"&UcFlightSelection%24ddlINFCount=" + 0 +
                    @"&UcFlightSelection%24txtDepartureDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" +
                    depdate.Year +
                    @"&UcFlightSelection%24txtReturnDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" +
                    depdate.Year;
                Thread.Sleep(3000);
                searchRequest4.AddParameter("application/x-www-form-urlencoded", postData4, ParameterType.RequestBody);
                agentClient.FollowRedirects = false;
                var searchResponse4 = agentClient.Execute(searchRequest4);

                // GET THE PAGE OF FLIGHTS (ONLINE BOOKING)

                const string url5 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                var searchRequest5 = new RestRequest(url5, Method.GET);
                searchRequest5.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest5.AddHeader("Content-Encoding", "gzip");
                searchRequest5.AddHeader("Host", "agent.lionair.co.id");
                searchRequest5.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest5.AddHeader("Referer", "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                Thread.Sleep(3000);
                var searchResponse5 = agentClient.Execute(searchRequest5);
                var html5 = searchResponse5.Content;

                var pageFlight = (CQ)html5;
                try
                {
                    var rows1 = pageFlight["#tblOutFlightBlocks > tbody"].Children();
                    var selectedRows = new List<IDomObject>();
                    var v = 2;

                    while (v < rows1.Count())
                    {

                        if (selectedRows.Count == listflight.Count)
                        {
                            break;
                        }
                        var plane = rows1[v].ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                        var w = 0;
                        if (plane == listflight.ElementAt(w))
                        {
                            selectedRows.Add(rows1[v]);
                            var z = v + 1;
                            w += 1;
                            while (z < rows1.Count() && z < v + listflight.Count())
                            {
                                plane = rows1[z].ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                                if (plane == listflight.ElementAt(w))
                                {
                                    selectedRows.Add(rows1[z]);
                                }
                                else
                                {
                                    selectedRows.Clear();
                                    break;
                                }
                                z += 1;
                                w += 1;
                            }
                            v = z;
                        }
                        else
                        {
                            v = v + 1;
                        }
                    }

                    var cabinClass = CabinClass.Economy;
                    if (j == 3)
                    {
                        cabinClass = CabinClass.Business;
                    }
                    var txt_OBNNRowID = selectedRows.Last().Id;
                    var colCollection = new List<List<String>>();
                    var seatCollection = new List<List<String>>();
                    switch (cabinClass)
                    {
                        case CabinClass.Economy:
                        {
                            for (var x = 0; x < selectedRows.Count; x++)
                            {
                                colCollection.Add(new List<String>());
                                seatCollection.Add(new List<String>());
                                var selectedColumns =
                                    selectedRows[x].ChildElements.ToList().GetRange(9, 18).ToList();
                                foreach (IDomElement t in selectedColumns)
                                {
                                    if (t.GetAttribute("class") != "step2_soldcell fareInfo_middle_tconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_middle_bconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_middle"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_middle_mconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right_tconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right_bconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right_mconx")
                                    {
                                        if (t.InnerText != "No Fares")
                                        {
                                            if (t.ChildElements.ToList()[1].ChildElements.ToList().Count() == 1
                                                && Convert.ToInt32(
                                                    t.ChildElements.ToList()[0].ChildElements.ToList()[1].InnerText) >=
                                                1)
                                            {
                                                colCollection[x].Add(t.GetAttribute("id").SubstringBetween(0,
                                                    t.GetAttribute("id").Length - 3));
                                                seatCollection[x].Add(t.ChildElements.ToList()[0].GetAttribute("title"));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        case CabinClass.Business:
                        {
                            for (var x = 0; x < selectedRows.Count; x++)
                            {
                                colCollection.Add(new List<string>());
                                seatCollection.Add(new List<string>());
                                var selectedColumns = selectedRows[x].ChildElements.ToList().GetRange(4, 5).ToList();
                                foreach (IDomElement t in selectedColumns)
                                {
                                    if (t.GetAttribute("class") != "step2_soldcell fareInfo_middle_tconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_middle_bconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_middle"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_middle_mconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right_tconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right_bconx"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right"
                                        && t.GetAttribute("class") != "step2_soldcell fareInfo_right_mconx")
                                    {
                                        if (t.InnerText != "No Fares")
                                        {
                                            if (
                                                t.ChildElements.ToList()[1].ChildElements.ToList().Count() == 1
                                                && Convert.ToInt32(
                                                    t.ChildElements.ToList()[0].ChildElements.ToList()[1].InnerText) >=
                                                1)
                                            {
                                                colCollection[x].Add(t.GetAttribute("id")
                                                    .SubstringBetween(0, t.GetAttribute("id").Length - 3));
                                                seatCollection[x].Add(t.ChildElements.ToList()[0].GetAttribute("title"));
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }

                    //priceCollections = list of string, isinya kolom paling kanan/terakhir dari tiap segmen
                    var priceCollections = colCollection.Select(seg => seg[seg.Count - 1]).ToList();
                    var seat = string.Join("|", seatCollection.Select(seg => seg[seg.Count - 1]).ToArray());


                    if (priceCollections.Count != 0) // Kalau casenya cabin business kdg2 suka habis
                    {
                        var postdata = new CreatePostData();
                        var colpost = postdata.Create(rows1, priceCollections);
                        const string garbled =
                            "ScriptManager1=upnlTotalTripCost%7CbtnPriceSelection&__EVENTTARGET=btnPriceSelection&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=";
                        //if (originCountry == "ID")
                        //{
                        var data =
                            "&txtUpdateInsurance=no" +
                            "&Insurance%24rblInsurance=No" +
                            "&Insurance%24txtInsPostbackRequired=no" +
                            "&txtPricingResponse=OK" + "" +
                            "&txtOutFBCsUsed=" + //seat +
                            "&txtInFBCsUsed=" +
                            "&txtTaxBreakdown=" +
                            "&UcFlightSelection%24TripType=rbOneWay" + "" +
                            "&UcFlightSelection%24DateFlexibility=rbMustTravel" +
                            "&UcFlightSelection%24txtSelOri=" + originAirport + //CHANGE
                            "&UcFlightSelection%24txtOri=" + cityDep + "%20(" + originAirport + ")" + //CHANGE
                            "&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year +
                            "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
                            "&UcFlightSelection%24ddlADTCount=" + 1 + //CHANGE
                            "&UcFlightSelection%24txtSelDes=" + destinationAirport + //CHANGE
                            "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + destinationAirport + ")" + //CHANGE
                            "&UcFlightSelection%24ddlCNNCount=" + 0 + //CHANGE
                            "&UcFlightSelection%24ddlINFCount=" + 0 + //CHANGE
                            "&UcFlightSelection%24txtDepartureDate=" + depdate.ToString("dd") + "%20" +
                            depdate.ToString("MMM") +
                            "%20" + depdate.Year + //CHANGE
                            "&UcFlightSelection%24txtReturnDate=" + depdate.ToString("dd") + "%20" +
                            depdate.ToString("MMM") +
                            "%20" + depdate.Year + //CHANGE
                            "&txtOBNNCellID=" + string.Join("|", priceCollections.ToArray()) +
                            "&txtIBNNCellID=oneway" +
                            "&txtOBNNRowID=" + txt_OBNNRowID +
                            "&txtIBNNRowID=" +
                            "&txtUserSelectedOneway=" +
                            "&__ASYNCPOST=true&";

                        var b =
                            "&txtUpdateInsurance=no" +
                            "&Insurance%24rblInsurance=No" +
                            "&Insurance%24txtInsPostbackRequired=no" +
                            "&txtPricingResponse=OK" + "" +
                            "&txtOutFBCsUsed=" + seat +
                            "&txtInFBCsUsed=" +
                            "&txtTaxBreakdown=" +
                            "&lbContinue.x=39&lbContinue.y=11" +
                            "&UcFlightSelection%24TripType=rbOneWay" + "" +
                            "&UcFlightSelection%24DateFlexibility=rbMustTravel" +
                            "&UcFlightSelection%24txtSelOri=" + originAirport + //CHANGE
                            "&UcFlightSelection%24txtOri=" + cityDep + "%20(" + originAirport + ")" + //CHANGE
                            "&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year +
                            "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
                            "&UcFlightSelection%24ddlADTCount=" + 1 + //CHANGE
                            "&UcFlightSelection%24txtSelDes=" + destinationAirport + //CHANGE
                            "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + destinationAirport + ")" + //CHANGE
                            "&UcFlightSelection%24ddlCNNCount=" + 0 + //CHANGE
                            "&UcFlightSelection%24ddlINFCount=" + 0 + //CHANGE
                            "&UcFlightSelection%24txtDepartureDate=" + depdate.ToString("dd") + "%20" +
                            depdate.ToString("MMM") +
                            "%20" + depdate.Year + //CHANGE
                            "&UcFlightSelection%24txtReturnDate=" + depdate.ToString("dd") + "%20" +
                            depdate.ToString("MMM") +
                            "%20" + depdate.Year + //CHANGE
                            "&txtOBNNCellID=" + string.Join("|", priceCollections.ToArray()) +
                            "&txtIBNNCellID=oneway" +
                            "&txtOBNNRowID=" + txt_OBNNRowID +
                            "&txtIBNNRowID=" +
                            "&txtUserSelectedOneway=";

                        // POST BUAT DAPETIN HARGA
                        const string url6 = @"LionAirAgentsIBE/Step2Availability.aspx";
                        var searchRequest6 = new RestRequest(url6, Method.POST);
                        searchRequest6.AddHeader("Accept-Encoding", "gzip, deflate");
                        searchRequest6.AddHeader("Accept", "*///*");
                        searchRequest6.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                        searchRequest6.AddHeader("Origin", "https://agent.lionair.co.id");
                        searchRequest6.AddHeader("Cache-Control", "no-cache");
                        searchRequest6.AddHeader("X-Requested-With", "XMLHttpRequest");
                        searchRequest6.AddHeader("X-MicrosoftAjax", "Delta=true");
                        var vs5 = HttpUtility.UrlEncode(pageFlight["#__VIEWSTATE"].Attr("value"));
                        var postData6 = garbled + vs5 + colpost + data;
                        searchRequest6.AddParameter("application/x-www-form-urlencoded", postData6,
                            ParameterType.RequestBody);
                        //Thread.Sleep(1000);
                        var searchResponse6 = agentClient.Execute(searchRequest6);
                        var html6 = searchResponse6.Content;
                        var pagePrice = (CQ) html6;
                        var revalidateFare = pagePrice["#tdAmtTotal"].Text();
                        var priceText = pagePrice.Text();
                        var startvs = priceText.IndexOf("__VIEWSTATE");
                        var xyz = priceText.SubstringBetween(startvs + 12, priceText.Length);
                        var myvs = HttpUtility.UrlEncode(xyz.Split('|')[0]);
                        agentprice = decimal.Parse(revalidateFare.Replace(",", ""));
                        var agentcurr = pagePrice["#tdCurrTotal"].Text();
                    }

                }
                catch
                {
                    TurnInUsername(userName);
                    LogOut(cid, agentClient);
                    return new Currency(currencyName, Payment.Constant.Supplier.LionAir);
                }

                TurnInUsername(userName);
                LogOut(cid, agentClient);
                var exchangeRate = agentprice / rateInCurr;
                Currency.SetRate(currencyName, exchangeRate, Payment.Constant.Supplier.LionAir);
                var currs = new Currency(currencyName, exchangeRate) { Supplier = Payment.Constant.Supplier.LionAir };
                Console.WriteLine("The Rate for " + currencyName + " is: " + exchangeRate);
                return currs;                
            }
        }
    }
 }

