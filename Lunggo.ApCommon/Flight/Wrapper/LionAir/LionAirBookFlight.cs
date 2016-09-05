using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using RestSharp;
using System.Globalization;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return Client.BookFlight(bookInfo);
        }

        private partial class LionAirClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                if (bookInfo.Itinerary.FareId == null)
                {
                    //throw new Exception("haloooo 1");
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        ErrorMessages = new List<string> { "FareId is null" }
                    };
                }
                
                var provider = CultureInfo.InvariantCulture;
                   
                string origin, dest, flightId;
                int segmentCount;
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
                    depdate = Convert.ToDateTime(splittedFareId[2]);
                    adultCount = Convert.ToInt32(splittedFareId[3]);
                    childCount = Convert.ToInt32(splittedFareId[4]);
                    infantCount = Convert.ToInt32(splittedFareId[5]);
                    cabinClass = FlightService.GetInstance().ParseCabinClass(splittedFareId[6]);
                    flightId = splittedFareId[8];
                    segmentCount = Convert.ToInt32(splittedFareId[9]);
                    listflight = splittedFareId[10].Split('|').ToList();
                 }
                catch
                {
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "FareId is no longer valid" }
                    };
                }

                var infants = bookInfo.Passengers.Where(pax => pax.Type == PaxType.Infant);
                var children = bookInfo.Passengers.Where(pax => pax.Type == PaxType.Child);
                bool isInfantValid = true;
                bool isChildValid = true;
                foreach (var inft in infants)
                {
                    if (inft.DateOfBirth.Value.AddYears(2) <= depdate)
                    {
                        isInfantValid = false;
                    }
                }

                foreach (var child in children)
                {
                    if (child.DateOfBirth.Value.AddYears(2) > depdate || child.DateOfBirth.Value.AddYears(12) <= depdate)
                    {
                        isChildValid = false;
                    }
                }

                if (adultCount == 0)
                {
                    //throw new Exception("haloooo 3");
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages = new List<string> { "There Must be one adult" },
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
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages =
                            new List<string> { "Total adult and children passenger must be not more than seven" },
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
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages =
                            new List<string> { "Each infant must be accompanied by one adult" },
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
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages = new List<string> { "Time of Departure Exceeds" },
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
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages = new List<string> { "Age of infant when traveling must be less than or equal 2 years old" },
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
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages = new List<string> { "Age of chil when traveling must be between 2 and 12 years old" },
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false
                    };
                }
                // [GET] Search Flight

                var client = CreateAgentClient();
                var flight = FlightService.GetInstance();
                var destinationCountry = flight.GetAirportCountryCode(dest);
                var userId = "";
                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clientx = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/LionAirAccount/ChooseUserId", Method.GET);
                var userName = "";
                var currentDeposit = "";
                var reqTime = DateTime.UtcNow;
                var itin = new FlightItinerary();
                var msgLogin = "Your login name is inuse";
                var counter = 0;

                var listDepHr = new List<string>();
                var listFlightNo = new List<string>();

                while (msgLogin == "Your login name is inuse" || msgLogin == "There was an error logging you in")
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && userName.Length == 0)
                    {
                        var accRs = (RestResponse)clientx.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }

                    if (userName.Length == 0)
                    {
                        return new BookFlightResult
                        {
                            Errors = new List<FlightError> { FlightError.TechnicalError },
                            ErrorMessages = new List<string> { "userName is full" }
                        };
                    }
                    bool successLogin;
                    
                    
                    do
                    {
                        client.BaseUrl = new Uri("https://agent.lionair.co.id");
                        const string url0 = @"/lionairagentsportal/default.aspx";
                        var searchRequest0 = new RestRequest(url0, Method.GET);
                        var searchResponse0 = client.Execute(searchRequest0);
                        var html0 = searchResponse0.Content;
                        CQ searchedHtml = html0;
                        var viewstate = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));
                        var eventval = HttpUtility.UrlEncode(searchedHtml["#__EVENTVALIDATION"].Attr("value"));
                        FlightService.GetInstance().ParseCabinClass(CabinClass.Economy);
                        if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                            (searchResponse0.StatusCode == HttpStatusCode.OK ||
                             searchResponse0.StatusCode == HttpStatusCode.Redirect))
                        {
                            TurnInUsername(clientx, userName);
                            return new BookFlightResult
                            {
                                Errors = new List<FlightError> { FlightError.InvalidInputData },
                                Status = new BookingStatusInfo
                                {
                                    BookingStatus = BookingStatus.Failed
                                },
                                ErrorMessages = new List<string> { "error entering default page" },
                                IsSuccess = false
                            };
                        }

                        Thread.Sleep(1000);
                        const string url1 = @"/lionairagentsportal/CaptchaGenerator.aspx";
                        var searchRequest1 = new RestRequest(url1, Method.GET);
                        var searchResponse1 = client.Execute(searchRequest1);
                        Thread.Sleep(1000);
                        successLogin = Login(client, searchResponse1.RawBytes, viewstate, eventval, out userId, userName,
                            out msgLogin, out currentDeposit);
                        Thread.Sleep(1000);
                        counter++;
                    } while (!successLogin && counter < 31 && (msgLogin != "Your login name is inuse"
                        && msgLogin != "There was an error logging you in"));
                }

                if (counter >= 31)
                {
                    //throw new Exception("haloooo 23");
                    TurnInUsername(clientx, userName);
                    return new BookFlightResult
                    {

                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Error captcha" }
                    };
                }
                
                //GET PAGE CONST ID
                var startind = userId.IndexOf("consID");
                var cid = userId.SubstringBetween(startind, userId.Length);
                var url2 = @"/LionAirAgentsIBE/OnlineBooking.aspx?" + cid;
                var searchRequest2 = new RestRequest(url2, Method.GET);
                searchRequest2.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest2.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest2.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx");
                Thread.Sleep(3000);
                var searchResponse2 = client.Execute(searchRequest2);

                //GET PAGE ONLINE BOOKING (PAGE MILIH PESAWAT)
                const string url3 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                var searchRequest3 = new RestRequest(url3, Method.GET);
                searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest3.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest3.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx");
                Thread.Sleep(3000);
                var searchResponse3 = client.Execute(searchRequest3);
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

                var check1 = cityairport.GetCity(origin, out cityDep);
                var check2 = cityairport.GetCity(dest, out cityArr);
                var postData4 =
                    @"__EVENTTARGET=UcFlightSelection%24lbSearch" + @"&__EVENTARGUMENT=" + @"&__VIEWSTATE=" + vs4 +
                    @"&UcFlightSelection%24TripType=rbOneWay" +
                    @"&UcFlightSelection%24DateFlexibility=rbMustTravel" +
                    @"&UcFlightSelection%24txtSelOri=" + origin +
                    @"&UcFlightSelection%24txtOri=" + cityDep + "+%28" + origin + "%29" +
                    @"&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "+" + depdate.Year +
                    @"&UcFlightSelection%24ddlDepDay=" + depdate.Day +
                    @"&UcFlightSelection%24ddlADTCount=" + adultCount + 
                    @"&UcFlightSelection%24txtSelDes=" + dest + 
                    @"&UcFlightSelection%24txtDes=" + cityArr + "+%28" + dest + "%29" +
                    @"&UcFlightSelection%24ddlCNNCount=" + childCount + 
                    @"&UcFlightSelection%24ddlINFCount=" + infantCount +
                    @"&UcFlightSelection%24txtDepartureDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" +
                    depdate.Year +
                    @"&UcFlightSelection%24txtReturnDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" +
                    depdate.Year; 
                Thread.Sleep(3000);
                searchRequest4.AddParameter("application/x-www-form-urlencoded", postData4, ParameterType.RequestBody);
                client.FollowRedirects = false;
                var searchResponse4 = client.Execute(searchRequest4);

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
                var searchResponse5 = client.Execute(searchRequest5);
                var html5 = searchResponse5.Content;

                var pageFlight = (CQ)html5;

                try
                {
                    var rows = pageFlight["#tblOutFlightBlocks > tbody"].Children();
                    var selectedRows = new List<IDomObject>();
                    var v = 2;
                    
                    while (v < rows.Count())
                    {
                        
                        if (selectedRows.Count == segmentCount)
                        {
                            break;
                        }
                        var plane = rows[v].ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                        var w = 0;
                        if (plane == listflight.ElementAt(w))
                        {
                            selectedRows.Add(rows[v]);
                            var z = v + 1;
                            w += 1;
                            while (z < rows.Count() && z < v + listflight.Count())
                            {
                                plane = rows[z].ChildElements.ToList()[0].ChildElements.ToList()[0].InnerText;
                                if (plane == listflight.ElementAt(w))
                                {
                                    selectedRows.Add(rows[z]);
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

                    var txt_OBNNRowID = selectedRows.Last().Id;
                    var passengers = adultCount + childCount; //jumlah adult + children
                    var colCollection = new List<List<String>>();
                    var seatCollection = new List<List<String>>();
                    
                    //PILIH COLUMN YG MASIH AVAILABLE 
                    switch (cabinClass)
                    {
                        case CabinClass.Economy:
                            {
                                for (var i = 0; i < selectedRows.Count; i++)
                                {
                                    colCollection.Add(new List<String>());
                                    seatCollection.Add(new List<String>());
                                    var selectedColumns =
                                        selectedRows[i].ChildElements.ToList().GetRange(9, 18).ToList();
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
                                                        t.ChildElements.ToList()[0].ChildElements.ToList()[1].InnerText) >= passengers)
                                                {
                                                    colCollection[i].Add(t.GetAttribute("id").SubstringBetween(0,
                                                                t.GetAttribute("id").Length - 3));
                                                    seatCollection[i].Add(t.ChildElements.ToList()[0].GetAttribute("title"));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case CabinClass.Business:
                            {
                                for (var i = 0; i < selectedRows.Count; i++)
                                {
                                    colCollection.Add(new List<string>());
                                    seatCollection.Add(new List<string>());
                                    var selectedColumns = selectedRows[i].ChildElements.ToList().GetRange(4, 5).ToList();
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
                                                        t.ChildElements.ToList()[0].ChildElements.ToList()[1].InnerText) >= passengers)
                                                {
                                                    colCollection[i].Add(t.GetAttribute("id")
                                                            .SubstringBetween(0, t.GetAttribute("id").Length - 3));
                                                    seatCollection[i].Add(t.ChildElements.ToList()[0].GetAttribute("title"));
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
                        var colpost = postdata.Create(rows, priceCollections);
                        const string garbled = "ScriptManager1=upnlTotalTripCost%7CbtnPriceSelection&__EVENTTARGET=btnPriceSelection&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=";
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
                            "&UcFlightSelection%24txtSelOri=" + origin + //CHANGE
                            "&UcFlightSelection%24txtOri=" + cityDep + "%20(" + origin + ")" + //CHANGE
                            "&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year +
                            "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
                            "&UcFlightSelection%24ddlADTCount=" + adultCount + //CHANGE
                            "&UcFlightSelection%24txtSelDes=" + dest + //CHANGE
                            "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + dest + ")" + //CHANGE
                            "&UcFlightSelection%24ddlCNNCount=" + childCount + //CHANGE
                            "&UcFlightSelection%24ddlINFCount=" + infantCount + //CHANGE
                            "&UcFlightSelection%24txtDepartureDate=" + depdate.ToString("dd") + "%20" + depdate.ToString("MMM") +
                            "%20" + depdate.Year + //CHANGE
                            "&UcFlightSelection%24txtReturnDate=" + depdate.ToString("dd") + "%20" + depdate.ToString("MMM") +
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
                                "&UcFlightSelection%24txtSelOri=" + origin + //CHANGE
                                "&UcFlightSelection%24txtOri=" + cityDep + "%20(" + origin + ")" + //CHANGE
                                "&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year +
                                "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
                                "&UcFlightSelection%24ddlADTCount=" + adultCount + //CHANGE
                                "&UcFlightSelection%24txtSelDes=" + dest + //CHANGE
                                "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + dest + ")" + //CHANGE
                                "&UcFlightSelection%24ddlCNNCount=" + childCount + //CHANGE
                                "&UcFlightSelection%24ddlINFCount=" + infantCount + //CHANGE
                                "&UcFlightSelection%24txtDepartureDate=" + depdate.ToString("dd") + "%20" + depdate.ToString("MMM") +
                                "%20" + depdate.Year + //CHANGE
                                "&UcFlightSelection%24txtReturnDate=" + depdate.ToString("dd") + "%20" + depdate.ToString("MMM") +
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
                        var searchResponse6 = client.Execute(searchRequest6);
                        var html6 = searchResponse6.Content;
                        var pagePrice = (CQ)html6;
                        var revalidateFare = pagePrice["#tdAmtTotal"].Text();
                        var priceText = pagePrice.Text();
                        var startvs = priceText.IndexOf("__VIEWSTATE");
                        var xyz = priceText.SubstringBetween(startvs + 12, priceText.Length);
                        var myvs = HttpUtility.UrlEncode(xyz.Split('|')[0]);
                        var agentprice = decimal.Parse(revalidateFare.Replace(",", ""));
                        var agentcurr = pagePrice["#tdCurrTotal"].Text();
                        
                        // POST SEBELUM PAGE PASSENGER
                        const string url7 = @"LionAirAgentsIBE/Step2Availability.aspx";
                        var searchRequest7 = new RestRequest(url7, Method.POST);
                        searchRequest7.AddHeader("Accept-Encoding", "gzip, deflate");
                        searchRequest7.AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*///*;q=0.8");
                        searchRequest7.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                        searchRequest7.AddHeader("Origin", "https://agent.lionair.co.id");
                        searchRequest7.AddHeader("Cache-Control", "max-age=0");
                        var postData7 = colpost.SubstringBetween(1, colpost.Length) + b +
                                        "&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" + myvs;
                        searchRequest7.AddParameter("application/x-www-form-urlencoded", postData7,
                            ParameterType.RequestBody);
                        //Thread.Sleep(1000);
                        var searchResponse7 = client.Execute(searchRequest7);

                        // GET PAGE DATA Passenger
                        const string url8 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                        var searchRequest8 = new RestRequest(url8, Method.GET);
                        searchRequest8.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                        searchRequest8.AddHeader("Content-Encoding", "gzip");
                        searchRequest8.AddHeader("Host", "agent.lionair.co.id");
                        searchRequest8.AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        searchRequest8.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                        //Thread.Sleep(1000);
                        var searchResponse8 = client.Execute(searchRequest8);
                        var html8 = searchResponse8.Content;

                        var pageBooking = (CQ)html8;
                        var vs9 = HttpUtility.UrlEncode(pageBooking["#__VIEWSTATE"].Attr("value"));
                        var beginning = "__EVENTTARGET=lbContinue&__EVENTARGUMENT=&__VIEWSTATE=" + vs9;// + "&__VIEWSTATEGENERATOR=B80F8107";
                        const string ending =
                            "&txtRemark=&payDet=rbPay_HOLD&CreditCardDisplay1%24CreditCardType=VI&CreditCardDisplay1%24txtCardHolderName=&CreditCardDisplay1%24CreditCardNumber=&CreditCardDisplay1%24CreditCardExpiryMonth=MM&CreditCardDisplay1%24CreditCardExpiryYear=YY&CreditCardDisplay1%24CVVNumber=&AcceptFareConditions=on&FlightInfo=&AXTotal=&DCTotal=&OtherTotal=&nameMismatch=";

                        //NEW ITINERARY TAKEN FROM PASSENGER DATA PAGE
                        
                        var flightTableRows = pageBooking["#FlightTable > tbody"].Children().ToList();
                        var itinRows = flightTableRows.GetRange(1, flightTableRows.Count - 2);
                        var newSegments = new List<FlightSegment>();
                        foreach (var row in itinRows)
                        {
                            //Column 0
                            var column0 = row.ChildElements.ToList()[0];
                            var flightNo = column0.ChildElements.ToList()[0].InnerText;
                            string airplaneCode = column0.ChildElements.ToList().Count == 1 ? null : column0.ChildElements.ToList()[1].InnerText;
                            
                            const string format1 = "dd MMM";
                            
                            //Column 1
                            var column1 = row.ChildElements.ToList()[1];
                            var insideText1 = column1.InnerText.Split(')');
                            var departureAirport =
                                insideText1[insideText1.Length - 2].
                                    SubstringBetween(insideText1[insideText1.Length - 2].Length - 3,
                                        insideText1[insideText1.Length - 2].Length);
                            var departureHour = insideText1.Last().SubstringBetween(0, 5);
                            var departureDate = column1.ChildElements.ToList()[1].InnerText;
                            var dateDeparture = DateTime.SpecifyKind(DateTime.ParseExact(departureDate, format1, provider), DateTimeKind.Utc);
                            if (dateDeparture < DateTime.Now)
                            {
                                dateDeparture = DateTime.SpecifyKind(new DateTime(DateTime.Now.AddYears(1).Year, dateDeparture.Month, dateDeparture.Day,
                                    Convert.ToInt32(departureHour.Split(':')[0]), Convert.ToInt32(departureHour.Split(':')[1]), 0), DateTimeKind.Utc);
                            }
                            else
                            {
                                dateDeparture = DateTime.SpecifyKind(new DateTime(DateTime.Now.Year, dateDeparture.Month, dateDeparture.Day,
                                     Convert.ToInt32(departureHour.Split(':')[0]), Convert.ToInt32(departureHour.Split(':')[1]), 0), DateTimeKind.Utc);
                            }

                            //Column 2
                            var column2 = row.ChildElements.ToList()[2];
                            var insideText2 = column2.InnerText.Split(')');
                             var arrivalAirport = insideText2[insideText2.Length - 2].
                                SubstringBetween(insideText2[insideText2.Length - 2].Length - 3,
                                insideText2[insideText2.Length - 2].Length);
                            var arrivalHour = insideText2.Last().SubstringBetween(0, 5);
                            var arrivalDate = column2.ChildElements.ToList()[1].InnerText;
                            var dateArrival = DateTime.SpecifyKind(DateTime.ParseExact(arrivalDate, format1, provider), DateTimeKind.Utc);
                            if (dateArrival < DateTime.Now)
                            {
                                dateArrival = DateTime.SpecifyKind(new DateTime(DateTime.Now.AddYears(1).Year, dateArrival.Month, dateArrival.Day,
                                    Convert.ToInt32(arrivalHour.Split(':')[0]), Convert.ToInt32(arrivalHour.Split(':')[1]), 0), DateTimeKind.Utc);
                            }
                            else
                            {
                                dateArrival = DateTime.SpecifyKind(new DateTime(DateTime.Now.Year, dateArrival.Month, dateArrival.Day,
                                     Convert.ToInt32(arrivalHour.Split(':')[0]), Convert.ToInt32(arrivalHour.Split(':')[1]), 0), DateTimeKind.Utc);
                            }

                            //Column 3

                            var column3 = row.ChildElements.ToList()[3];
                            var column3Text = column3.InnerText.Split(new string[] { "stops", "stop" }, StringSplitOptions.RemoveEmptyEntries);
                            var stopsno = column3Text[0];
                            var dur = column3Text[1].Split(' ');
                            TimeSpan duration;
                            if (dur.Length != 1)
                            {
                                duration = new TimeSpan(
                                Convert.ToInt32(dur[0].SubstringBetween(0, dur[0].Length - 1)),
                                Convert.ToInt32(dur[1].SubstringBetween(0, dur[1].Length - 1)), 0);
                            }
                            else
                            {
                                duration = new TimeSpan(0, 
                                Convert.ToInt32(dur[0].SubstringBetween(0, dur[0].Length - 1)), 0);
                            }

                            listFlightNo.Add(flightNo);
                            listDepHr.Add(departureHour);

                            newSegments.Add(new FlightSegment
                            {
                                AircraftCode = airplaneCode,
                                AirlineCode = flightNo.Split()[0],
                                ArrivalAirport = arrivalAirport,
                                ArrivalTime = dateArrival,
                                CabinClass = cabinClass,
                                DepartureAirport = departureAirport,
                                DepartureTime = dateDeparture,
                                Duration = duration,
                                FlightNumber = flightNo.Split()[1],
                                OperatingAirlineCode = flightNo.Split()[0],
                                StopQuantity = Convert.ToInt32(stopsno),
                                IsMealIncluded = flightNo.Split()[0] == "ID",
                                IsPscIncluded = true
                            });

                        }

                        var depHrJoin = String.Join("|", listDepHr.ToArray());
                        var flightNoJoin = String.Join("|", listFlightNo.ToArray());
                        var isInternational = CheckInternationality(newSegments);
                        
                        itin = new FlightItinerary
                        {
                            AdultCount = adultCount,
                            ChildCount = childCount,
                            InfantCount = infantCount,
                            CanHold = true,
                            FareType = FareType.Published,
                            RequireBirthDate = isInternational,
                            RequirePassport = isInternational,
                            RequireSameCheckIn = false,
                            RequireNationality = isInternational,
                            RequestedCabinClass = CabinClass.Economy,
                            RequestedTripType = bookInfo.Itinerary.RequestedTripType,
                            TripType = TripType.OneWay,
                            Supplier = Supplier.LionAir,
                            Price = new Price(),
                            FareId = bookInfo.Itinerary.FareId,
                            Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = origin,
                                    DestinationAirport = dest,
                                    DepartureDate = depdate.Date,
                                    Segments = newSegments
                                }
                            }
                        };

                        Currency currclass;
                        var currencyList = Currency.GetAllCurrencies(Payment.Constant.Supplier.LionAir);
                        if (!currencyList.TryGetValue(agentcurr, out currclass))
                        {
                            return new BookFlightResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                Status = null,
                                ErrorMessages = new List<string> { "Currency is not available" },
                                IsValid = true,
                                NewItinerary = itin,

                            };
                        }
                        itin.Price.SetSupplier(agentprice, new Currency(agentcurr, Payment.Constant.Supplier.LionAir));

                        var isItinChanged = !itin.Identical(bookInfo.Itinerary);

                        //Cek Jika Itinerary berubah
                        if (isItinChanged)
                        {
                            LogOut(cid, client);
                            TurnInUsername(clientx, userName);

                            return new BookFlightResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                Status = null,
                                ErrorMessages = new List<string> { "Itinerary is Changed!" },
                                NewPrice = agentprice,
                                IsValid = true,
                                IsPriceChanged = agentprice != bookInfo.Itinerary.Price.Supplier,
                                NewItinerary = itin,
                                IsItineraryChanged = isItinChanged

                            };
                        }

                        var harga2 = agentprice;
                        var orderedPassengers = bookInfo.Passengers.OrderBy(x => x.Type).ToList();
                        var dataPassenger = "";
                        
                        if (destinationCountry == "ID")
                        {
                            var infpax = 1;
                            for (var i = 0; i < orderedPassengers.Count; i++)
                            {
                                var title = "";
                                string mealrequest;
                                switch (orderedPassengers[i].Title)
                                {
                                    case Title.Miss:
                                        title = "Miss";
                                        break;
                                    case Title.Mister:
                                        title = orderedPassengers.ElementAt(i).Type == PaxType.Adult ? "Mr" : "Mstr";
                                        break;
                                    case Title.Mistress:
                                        title = "Mrs";
                                        break;
                                }

                                
                                switch (orderedPassengers[i].Type)
                                {
                                    case PaxType.Adult:
                                        mealrequest = "No+Preference";
                                        dataPassenger +=
                                            "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                            "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                            String.Join("+", orderedPassengers[i].FirstName.Split(' ')) +
                                            "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                            orderedPassengers[i].LastName +
                                            "&NameBlock" + (i + 1) + "%24ddlAirline=JT" + //codeFlight + //NOT YET
                                            "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                            "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                            "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest;
                                        break;
                                    case PaxType.Child:
                                        mealrequest = "No+Preference";
                                        dataPassenger +=
                                            "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                            "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                            String.Join("+", orderedPassengers[i].FirstName.Split(' ')) +
                                            "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                            orderedPassengers[i].LastName +
                                            "&NameBlock" + (i + 1) + "%24ddlAirline=JT" +// codeFlight + //NOT YET
                                            "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                            "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                            "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                            orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("dd") +
                                            "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                            orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("MMM") +
                                            "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                            orderedPassengers[i].DateOfBirth.GetValueOrDefault().Year +
                                            "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest;
                                        break;
                                    case PaxType.Infant:
                                        mealrequest = "BBML";
                                        dataPassenger +=
                                            "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                            "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                            String.Join("+", orderedPassengers[i].FirstName.Split(' ')) +
                                            "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                            orderedPassengers[i].LastName +
                                            "&NameBlock" + (i + 1) + "%24ddlINFPaxAssoc=" + infpax +
                                            "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                            "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                            "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                            orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("dd") +
                                            "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                            orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("MMM") +
                                            "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                            orderedPassengers[i].DateOfBirth.GetValueOrDefault().Year +
                                            "&NameBlock" + (i + 1) + "$ddlMealRequest=" + mealrequest;
                                        infpax += 1;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            var infpax = 1;
                            for (var i = 0; i < orderedPassengers.Count; i++)
                            {
                                var gender = "";
                                var title = "";
                                string mealrequest;
                                switch (orderedPassengers[i].Gender)
                                {
                                    case Gender.Male:
                                        gender = "M";
                                        break;
                                    case Gender.Female:
                                        gender = "F";
                                        break;
                                }

                                switch (orderedPassengers[i].Title)
                                {
                                    case Title.Miss:
                                        title = "Miss";
                                        break;
                                    case Title.Mister:
                                        title = orderedPassengers.ElementAt(i).Type == PaxType.Adult ? "Mr" : "Mstr";
                                        break;
                                    case Title.Mistress:
                                        title = "Mrs";
                                        break;
                                }

                                if (orderedPassengers.ElementAt(i).Type == PaxType.Infant)
                                {
                                    mealrequest = "BBML";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers[i].FirstName.Split(' ')) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers[i].LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlGender=" + gender +
                                        "&NameBlock" + (i + 1) + "%24ddlINFPaxAssoc=" + infpax +
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                        orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                        orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                        orderedPassengers[i].DateOfBirth.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest +
                                        "&NameBlock" + (i + 1) + "%24txtPassportNumber=" +
                                        orderedPassengers[i].PassportNumber +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpDay=" +
                                        orderedPassengers[i].PassportExpiryDate.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpMon=" +
                                        orderedPassengers[i].PassportExpiryDate.GetValueOrDefault().ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpYear=" +
                                        orderedPassengers[i].PassportExpiryDate.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlDocCountry=" +
                                        orderedPassengers[i].PassportCountry +
                                        "&NameBlock" + (i + 1) + "%24ddlPaxCountry=" +
                                        orderedPassengers[i].PassportCountry;
                                    infpax += 1;
                                }
                                else
                                {
                                    mealrequest = "No+Preference";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers[i].FirstName) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers[i].LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlGender=" + gender +
                                        "&NameBlock" + (i + 1) + "%24ddlAirline=JT" + //codeFlight +
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                        orderedPassengers[i].DateOfBirth.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                        orderedPassengers[i]
                                            .DateOfBirth.GetValueOrDefault()
                                            .ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                        orderedPassengers[i].DateOfBirth.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest +
                                        "&NameBlock" + (i + 1) + "%24txtPassportNumber=" +
                                        orderedPassengers[i].PassportNumber +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpDay=" +
                                        orderedPassengers[i]
                                            .PassportExpiryDate.GetValueOrDefault()
                                            .ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpMon=" +
                                        orderedPassengers[i]
                                            .PassportExpiryDate.GetValueOrDefault()
                                            .ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpYear=" +
                                        orderedPassengers[i].PassportExpiryDate.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlDocCountry=" +
                                        orderedPassengers[i].PassportCountry +
                                        "&NameBlock" + (i + 1) + "%24ddlPaxCountry=" +
                                        orderedPassengers[i].PassportCountry;
                                }
                            }
                        }

                        var middle = "";
                        for (var y = 0; y < adultCount + childCount; y++)
                        {
                            for (var x = 0; x < newSegments.Count; x++)
                            {
                                middle += "&Bag_" + newSegments.ElementAt(x).DepartureAirport + "%24" 
                                    + newSegments.ElementAt(x).ArrivalAirport + "%24" + x + "%24" + y + "=";
                            }
                        }

                        var middle1 = "&hdnBagSelection=";
                        for (var x = 0; x < adultCount + childCount; x++)
                        {
                            middle1 += "%2C%7C";
                        }
                        middle1 += "&hdnDepPorts=&hdnArrPorts=";

                        var c = bookInfo.Contact.Name.Trim();
                        string contactFirstname, contactLastname;
                        var contactTitle = "";
                        if (c.Split(' ').Count() == 1)
                        {
                            contactFirstname = c;
                            contactLastname = c;
                        }
                        else
                        {
                            contactLastname = c.Split(' ').Last();
                            contactFirstname = String.Join("+", c.Split(' ').ToList().Take(c.Split(' ').Length - 1));
                        }

                        var cntct =
                            "&ContactTitle=Mr" +
                            "&ContactFirstName=" + contactFirstname +
                            "&ContactLastName=" + contactLastname +
                            "&txtAddress1=" +
                            "&txtAddress2=" +
                            "&ddlCountry=ID" + //bookInfo.ContactData.CountryCode + ///change?///
                            "&txtCity=" +
                            "&txtPostCode=" +
                            "&txtCountryCode1=" + bookInfo.Contact.CountryCallingCode +
                            "&txtAreaCode1=" +
                            "&txtPhoneNumber1=" + bookInfo.Contact.Phone +
                            "&ddlOriNumber=M" +
                            "&txtCountryCode3=" +
                            "&txtPhoneNumber3=" +
                            "&txtEmailAddress1=" + HttpUtility.UrlEncode("dwi.agustina@travelmadezy.com") + //email agent
                            "&txtEmailAddress2=" + HttpUtility.UrlEncode("dwi.agustina@travelmadezy.com");

                        Thread.Sleep(1000);
                        var postDataBooking = beginning + dataPassenger + middle + middle1 + cntct + ending;
                        client.FollowRedirects = false;

                        const string urlBooking = @"LionAirAgentsIBE/Step3.aspx";
                        var searchRequestBooking = new RestRequest(urlBooking, Method.POST);
                        searchRequestBooking.AddHeader("Accept-Encoding", "gzip, deflate");
                        searchRequestBooking.AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        searchRequestBooking.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                        searchRequestBooking.AddHeader("Origin", "https://agent.lionair.co.id");
                        searchRequestBooking.AddHeader("Cache-Control", "max-age=0");
                        searchRequestBooking.AddParameter("application/x-www-form-urlencoded", postDataBooking,
                            ParameterType.RequestBody);
                        Thread.Sleep(3000);
                        var searchResponseBooking = client.Execute(searchRequestBooking);
                        string newPrice;

                        //HARGA BARU DI UJUNG
                        try
                        {
                            var htmlBookingResult = (CQ)searchResponseBooking.Content;
                            newPrice = htmlBookingResult["#lblTotalFares"].Text().Replace(",", "");
                            var vsPriceAcceptance =
                                HttpUtility.UrlEncode(htmlBookingResult["#__VIEWSTATE"].Attr("value"));
                            
                            //Kondisi dimana ada perubahan harga namun udah sama dengan harga kita
                            if (newPrice.Length != 0)
                            {
                                if (decimal.Parse(newPrice) != bookInfo.Itinerary.Price.Supplier)
                                {
                                    LogOut(cid, client);
                                    TurnInUsername(clientx, userName);
                                    
                                    var newFareId = origin + "+" + dest + "+" + splittedFareId[2] + "+" + adultCount +
                                                    "+" +
                                                    childCount + "+" + infantCount + "+" + splittedFareId[6] + "+" +
                                                    newPrice + "+" +
                                                    flightId + "+" + newSegments.Count + "+" + flightNoJoin + "+" +
                                                    depHrJoin;

                                    itin.Price.SetSupplier(decimal.Parse(newPrice), new Currency("IDR"
                                        , Payment.Constant.Supplier.LionAir));
                                    itin.FareId = newFareId;

                                    return new BookFlightResult
                                    {
                                        IsSuccess = false,
                                        Errors = new List<FlightError> { FlightError.TechnicalError },
                                        Status = null,
                                        ErrorMessages = new List<string> { "Price is changed!" },
                                        NewPrice = decimal.Parse(newPrice),
                                        IsValid = true,
                                        IsPriceChanged = true,
                                        NewItinerary = itin,
                                        IsItineraryChanged = isItinChanged

                                    };
                                }
                                if (decimal.Parse(newPrice) == bookInfo.Itinerary.Price.Supplier)
                                {
                                    var postDataPriceAcceptance = "__EVENTTARGET=lbContinue&__EVENTARGUMENT=&__VIEWSTATE="
                                                                  + vsPriceAcceptance;
                                    const string urlPriceAcceptance = @"LionAirAgentsIBE/PriceAcceptance.aspx";
                                    var searchRequestPriceAcceptance = new RestRequest(urlPriceAcceptance, Method.POST);
                                    searchRequestPriceAcceptance.AddHeader("Accept-Encoding", "gzip, deflate");
                                    searchRequestPriceAcceptance.AddHeader("Accept",
                                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                                    searchRequestPriceAcceptance.AddHeader("Referer",
                                        "https://agent.lionair.co.id/LionAirAgentsIBE/Step3.aspx");
                                    searchRequestPriceAcceptance.AddHeader("Origin", "https://agent.lionair.co.id");
                                    searchRequestPriceAcceptance.AddHeader("Cache-Control", "max-age=0");
                                    searchRequestPriceAcceptance.AddParameter
                                        ("application/x-www-form-urlencoded", postDataPriceAcceptance,
                                            ParameterType.RequestBody);
                                    Thread.Sleep(3000);
                                    var searchResponsPriceAcceptance = client.Execute(searchRequestPriceAcceptance);
                                }
                            }
                            // kalau harga setelah dipilih di page yg banyak radio button beda dengan yg pertama
                                // kali di search

                            else if (bookInfo.Itinerary.Price.Supplier != harga2)
                            {
                                var newFareId = origin + "+" + dest + "+" + splittedFareId[2] + "+" + adultCount 
                                    + "+" + childCount + "+" + infantCount + "+" + splittedFareId[6] + "+" + harga2 
                                    + "+" + flightId + "+" + newSegments.Count + "+" + flightNoJoin + "+" + depHrJoin;

                                itin.Price.SetSupplier(harga2, new Currency("IDR",
                                    Payment.Constant.Supplier.LionAir));
                                itin.FareId = newFareId;

                                LogOut(cid, client);
                                TurnInUsername(clientx, userName);

                                //newPrice itu harga nya null, padahal dicoba untuk parse
                                return new BookFlightResult
                                {
                                    IsSuccess = false,
                                    Errors = new List<FlightError> { FlightError.TechnicalError },
                                    Status = null,
                                    ErrorMessages = new List<string> { "Price is changed!2" },
                                    NewPrice = harga2,
                                    IsValid = true,
                                    IsPriceChanged = true,
                                    NewItinerary = itin,
                                    IsItineraryChanged = isItinChanged

                                };
                            }
                        }
                        catch 
                        {
                            LogOut(cid, client);
                            TurnInUsername(clientx, userName);

                            return new BookFlightResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Error di bagian validasi harga di halaman akhir booking" },
                                Status = new BookingStatusInfo
                                {
                                    BookingStatus = BookingStatus.Failed
                                }
                            };
                        }

                        if (newPrice.Length == 0)
                        {
                            // Kalau normal2 aja

                            var newFareId = origin + "+" + dest + "+" + splittedFareId[2] + "+" + adultCount +
                                                   "+" + childCount + "+" + infantCount + "+" + splittedFareId[6] +
                                                   "+" + harga2 + "+" + flightId + "+" + newSegments.Count +
                                                   "+" + flightNoJoin + "+" + depHrJoin;

                            itin.Price.SetSupplier(harga2, new Currency("IDR",
                                Payment.Constant.Supplier.LionAir));
                            itin.FareId = newFareId;

                            const string url9 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                            var searchRequest9 = new RestRequest(url9, Method.GET);
                            searchRequest9.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                            searchRequest9.AddHeader("Content-Encoding", "gzip");
                            searchRequest9.AddHeader("Host", "agent.lionair.co.id");
                            searchRequest9.AddHeader("Accept",
                                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                            searchRequest9.AddHeader("Referer",
                                "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                            Thread.Sleep(3000);
                            var searchResponse9 = client.Execute(searchRequest9);

                            const string url10 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                            var searchRequest10 = new RestRequest(url10, Method.GET);
                            searchRequest10.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                            searchRequest10.AddHeader("Content-Encoding", "gzip");
                            searchRequest10.AddHeader("Host", "agent.lionair.co.id");
                            searchRequest10.AddHeader("Accept",
                                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                            searchRequest10.AddHeader("Referer",
                                "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                            Thread.Sleep(3000);
                            var searchResponse10 = client.Execute(searchRequest10);
                            var html10 = searchResponse10.Content;
                            var bookingData = (CQ)html10;

                            bookingReference = bookingData["#lblRefNumber"].Text();
                            bookingTimeLimit = bookingData["#lblPayByDate"].Text();
                        }
                        else
                        {
                            var newFareId = origin + "+" + dest + "+" + splittedFareId[2] + "+" + adultCount +
                                                   "+" + childCount + "+" + infantCount + "+" + splittedFareId[6] + 
                                                   "+" + newPrice + "+" + flightId + "+" + newSegments.Count + 
                                                   "+" + flightNoJoin + "+" + depHrJoin;
                            itin.Price.SetSupplier(decimal.Parse(newPrice), new Currency("IDR",
                                Payment.Constant.Supplier.LionAir));
                            itin.FareId = newFareId;

                            // kalau ini booking ke 2 (setelah booking 1, yang harga naik di ujung)
                            const string url9 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                            var searchRequest9 = new RestRequest(url9, Method.GET);
                            searchRequest9.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                            searchRequest9.AddHeader("Content-Encoding", "gzip");
                            searchRequest9.AddHeader("Host", "agent.lionair.co.id");
                            searchRequest9.AddHeader("Accept",
                                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                            searchRequest9.AddHeader("Referer",
                                "https://agent.lionair.co.id/LionAirAgentsIBE/Step3.aspx");
                            Thread.Sleep(3000);
                            var searchResponse9 = client.Execute(searchRequest9);

                            const string url10 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                            var searchRequest10 = new RestRequest(url10, Method.GET);
                            searchRequest10.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                            searchRequest10.AddHeader("Content-Encoding", "gzip");
                            searchRequest10.AddHeader("Host", "agent.lionair.co.id");
                            searchRequest10.AddHeader("Accept",
                                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                            searchRequest10.AddHeader("Referer",
                                "https://agent.lionair.co.id/LionAirAgentsIBE/Step3.aspx");
                            Thread.Sleep(3000);
                            var searchResponse10 = client.Execute(searchRequest10);
                            var html10 = searchResponse10.Content;
                            var bookingData = (CQ)html10;

                            bookingReference = bookingData["#lblRefNumber"].Text();
                            bookingTimeLimit = bookingData["#lblPayByDate"].Text();
                        }

                        LogOut(cid, client);
                        TurnInUsername(clientx, userName);
                    }

                    const string format = "dd MMMM', 'yyyy', 'HH:mm";
                    if (bookingReference.Length != 0)
                    {
                        return new BookFlightResult
                        {
                            IsSuccess = true,
                            Status = new BookingStatusInfo
                            {
                                BookingId = bookingReference,
                                TimeLimit =
                                    DateTime.SpecifyKind(
                                        DateTime.ParseExact(
                                            bookingTimeLimit.SubstringBetween(0, bookingTimeLimit.Length - 3), format,
                                            provider), DateTimeKind.Utc).AddHours(-7),
                                BookingStatus = BookingStatus.Booked
                            },
                            IsValid = true,
                            IsItineraryChanged = false,
                            IsPriceChanged = false,
                            NewItinerary = itin
                        };
                    }
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        ErrorMessages = new List<string> { "Web Layout Changed! Failed to get Booking Reference" }
                    };
                }
                catch (Exception e)
                {
                    LogOut(cid, client);
                    TurnInUsername(clientx, userName);
                     
                    return new BookFlightResult
                     {
                         IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.TechnicalError},
                        ErrorMessages = new List<string> { e.Message },
                         Status = new BookingStatusInfo
                         {
                             BookingStatus = BookingStatus.Failed
                         }
                     };
                 }
             }

            private void LogOut(string accountId, RestClient client)
            {
                const string url15 = @"/LionAirAgentsPortal/Logout.aspx";
                var searchRequest15 = new RestRequest(url15, Method.GET);
                searchRequest15.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest15.AddHeader("Content-Encoding", "gzip");
                searchRequest15.AddHeader("Host", "agent.lionair.co.id");
                searchRequest15.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest15.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?" + accountId);
                Thread.Sleep(1000);
                var searchResponse15 = client.Execute(searchRequest15);

                //GET PAGE DEFAULT(HOME)

                const string url16 = @"/LionAirAgentsPortal/Default.aspx";
                var searchRequest16 = new RestRequest(url16, Method.GET);
                searchRequest16.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest16.AddHeader("Content-Encoding", "gzip");
                searchRequest16.AddHeader("Host", "agent.lionair.co.id");
                searchRequest16.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest16.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?" + accountId);
                Thread.Sleep(1000);
                var searchResponse16 = client.Execute(searchRequest16);
            }

            private void TurnInUsername(RestClient client, string username)
            {
                var accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + username, Method.GET);
                var accRs = (RestResponse)client.Execute(accReq);
            }
         }
     }
 }

