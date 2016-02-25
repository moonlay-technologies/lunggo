using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
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
                if (bookInfo.FareId == null)
                {
                    return new BookFlightResult {Errors = new List<FlightError> {FlightError.InvalidInputData}};
                }

                string origin, dest, flightId;
                var flights = new List<string>();
                var dephrs = new List<string>();
                int segmentCount;
                DateTime depdate;
                int adultCount, childCount, infantCount;
                decimal price;
                string bookingTimeLimit = "";
                string bookingReference = "";
                CabinClass cabinClass;
                try
                {
                    var splittedFareId = bookInfo.FareId.Split('+');
                    origin = splittedFareId[0]; //CGK
                    dest = splittedFareId[1]; // SIN
                    depdate = Convert.ToDateTime(splittedFareId[2]);
                    adultCount = Convert.ToInt32(splittedFareId[3]);
                    childCount = Convert.ToInt32(splittedFareId[4]);
                    infantCount = Convert.ToInt32(splittedFareId[5]);
                    cabinClass = FlightService.ParseCabinClass(splittedFareId[6]);
                    price = Convert.ToDecimal((splittedFareId[7]));
                    flightId = splittedFareId[8];
                    segmentCount = Convert.ToInt32(splittedFareId[9]);
                 }
                catch
                {
                    return new BookFlightResult {Errors = new List<FlightError> {FlightError.FareIdNoLongerValid}};
                }


                if (adultCount == 0)
                {
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> {"There Must be one adult"}
                    };
                }
                if (adultCount + childCount > 7)
                {
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Total adult and children passenger must be not more than seven"}
                    };
                }
                if (adultCount < infantCount)
                {
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Each infant must be accompanied by one adult"}
                    };
                }
                if (depdate > DateTime.Now.AddMonths(12).Date)
                {
                    return new BookFlightResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> {"Time of Departure Exceeds"}
                    };
                }

                // [GET] Search Flight

                var client = CreateAgentClient();
                string currentDeposit;
                var dict = DictionaryService.GetInstance();
                var originCountry = dict.GetAirportCountryCode(origin);
                var destinationCountry = dict.GetAirportCountryCode(dest);
                var searchedHtml = new CQ();
                string userID;
                if (originCountry == "ID")
                {
                    bool successLogin;
                    do
                    {
                        client.BaseUrl = new Uri("https://agent.lionair.co.id");
                        var url0 = @"/lionairagentsportal/default.aspx";
                        var searchRequest0 = new RestRequest(url0, Method.GET);
                        var searchResponse0 = client.Execute(searchRequest0);
                        var html0 = searchResponse0.Content;
                        searchedHtml = (CQ) html0;
                        var viewstate = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));
                        var eventval = HttpUtility.UrlEncode(searchedHtml["#__EVENTVALIDATION"].Attr("value"));
                        FlightService.ParseCabinClass(CabinClass.Economy);
                        if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                            (searchResponse0.StatusCode == HttpStatusCode.OK ||
                             searchResponse0.StatusCode == HttpStatusCode.Redirect))
                            return new BookFlightResult
                            {
                                Errors = new List<FlightError> {FlightError.InvalidInputData}
                            };
                        Thread.Sleep(1000);
                        var url1 = @"/lionairagentsportal/CaptchaGenerator.aspx";
                        var searchRequest1 = new RestRequest(url1, Method.GET);
                        var searchResponse1 = client.Execute(searchRequest1);
                        Thread.Sleep(1000);
                        successLogin = Login(client, searchResponse1.RawBytes, viewstate, eventval, out userID); //, out currentDeposit);
                        Thread.Sleep(1000);
                    } while (!successLogin);
                }
                else
                {
                    return new BookFlightResult
                    {
                        IsSuccess = true,
                        //IsValid = false,
                        //Itinerary = null
                    };
                }

                //GET PAGE CONST ID
                var startind = userID.IndexOf("consID");
                var cid = userID.SubstringBetween(startind, userID.Length);
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
                var url3 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                var searchRequest3 = new RestRequest(url3, Method.GET);
                searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest3.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest3.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx");
                Thread.Sleep(3000);
                var searchResponse3 = client.Execute(searchRequest3);
                var vs = new CQ();
                var html3 = searchResponse3.Content;
                vs = (CQ) html3;
                var vs4 = HttpUtility.UrlEncode(vs["#__VIEWSTATE"].Attr("value"));

                //POST FOR PAGE AVAILABLE FLIGHTS AND PRICE 

                var url4 = @"LionAirAgentsIBE/Step1.aspx";
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
                    @"&UcFlightSelection%24txtSelOri=" + origin + // => should be variabel origin (ex: CGK)
                    @"&UcFlightSelection%24txtOri=" + cityDep + "+%28" + origin + "%29" +
                    // => should be return of City-Airport method whatsoever
                    @"&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "+" + depdate.Year +
                    // => should be taken from variabel depdate
                    @"&UcFlightSelection%24ddlDepDay=" + depdate.Day + // => should be taken from variabel depdate
                    @"&UcFlightSelection%24ddlADTCount=" + adultCount + //adultCount
                    @"&UcFlightSelection%24txtSelDes=" + dest + //=> should be variabel DEST (ex: CGK)
                    @"&UcFlightSelection%24txtDes=" + cityArr + "+%28" + dest + "%29" +
                    // => should be return of City-Airport method whatsoever
                    @"&UcFlightSelection%24ddlCNNCount=" + childCount + //childCount
                    @"&UcFlightSelection%24ddlINFCount=" + infantCount + //infantCount
                    @"&UcFlightSelection%24txtDepartureDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" +
                    depdate.Year + // => should be taken from variabel depdate
                    @"&UcFlightSelection%24txtReturnDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" +
                    depdate.Year; // => should be taken from variabel depdate
                Thread.Sleep(3000);
                searchRequest4.AddParameter("application/x-www-form-urlencoded", postData4, ParameterType.RequestBody);
                client.FollowRedirects = false;
                var searchResponse4 = client.Execute(searchRequest4);

                // GET THE PAGE OF FLIGHTS (ONLINE BOOKING)

                var url5 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
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

                var pageFlight = (CQ) html5;

                try
                {
                    CQ Rows;
                    Rows = pageFlight["#tblOutFlightBlocks > tbody"].Children();

                    var selectedRows = new List<IDomObject>();

                    // PILIH ROW DAN SEGMENNYA
                    //var j = 0;
                    for (int v = 2; v < Rows.Count() - 1; v++)
                    {
                        var rowId = Rows[v].GetAttribute("id");
                        var flightselection = rowId.Split('_')[1].SubstringBetween(1, rowId.Split('_')[1].Length);
                        //var liat = flightID.Split('_')[1].SubstringBetween(1, flightID.Split('_')[1].Length);
                        if (rowId[2] == flightId[3] &&
                            (flightselection ==
                             flightId.Split('_')[1].SubstringBetween(1, flightId.Split('_')[1].Length)))
                        {
                            selectedRows.Add(Rows[v]);
                        }
                    }
                    //var jlh = selectedRows.Count();
                    //Crawling data penerbangan: Jam, airport, flight number
                    string b;
                    string colpost;
                    string garbled;
                    string vs5;

                    var segments = new List<FlightSegment>();
                    bool changeDay;
                    var arrDate = new DateTime(depdate.Year, depdate.Month, depdate.Day, 0, 0, 0);
                    int jamdatang;
                    int jamberangkat;
                    string txt_OBNNRowID = selectedRows.Last().Id;
                    foreach (var row in selectedRows)
                    {
                        //Column 0
                        var flightIdty = row.ChildElements.ToList()[0];
                        var flightNo = "JT 34";
                        var aircraftNo = "737-900ER";
                        var airplaneName = "Lion Air";
                        if (flightIdty.ChildElements.ToList().Count == 2)
                        {
                            aircraftNo = flightIdty.ChildElements.ToList()[1].InnerText;
                        }
                        else if (flightIdty.ChildElements.ToList().Count == 1)
                        {
                            aircraftNo = "Unknown";
                        }

                        flightNo = flightIdty.ChildElements.ToList()[0].InnerText;
                        flights.Add(flightNo);
                        if (flightNo.Split(' ')[0] == "JT")
                            airplaneName = "Lion Air";
                        else if (flightNo.Split(' ')[0] == "IW")
                        {
                            airplaneName = "Wings Air";
                        }
                        else if (flightNo.Split(' ')[0] == "ID")
                        {
                            airplaneName = "Batik Air";
                        }
                        else if (flightNo.Split(' ')[0] == "OD")
                        {
                            airplaneName = "Malindo Air";
                        }
                        else if (flightNo.Split(' ')[0] == "SL")
                        {
                            airplaneName = "Thai Lion Air";
                        }

                        //Column 1

                        var colhidden = row.ChildElements.ToList()[1];
                        var hidtext = colhidden.InnerText.Split('|')[6];
                        var airportdeparture = hidtext.SubstringBetween(0, 3);
                        var airportarrival = hidtext.SubstringBetween(3, hidtext.Length);

                        //Column 2

                        var departure = row.ChildElements.ToList()[2];
                        var x = departure.InnerText.Split(' ');
                        //var y = string.Join(" ", x.Take(x.Length - 1));
                        //var cityDeparture = y.SubstringBetween(0, y.Length - 3);
                        var timeDeparture = x[x.Length - 1];
                        dephrs.Add(timeDeparture);
                        DateTime depDate;
                        jamberangkat = Convert.ToInt32(timeDeparture.Split(':')[0]);
                        jamdatang = arrDate.Hour;
                        if (jamdatang > jamberangkat)
                            changeDay = true;
                        else
                        {
                            changeDay = false;
                        }

                        if (changeDay == false)
                        {
                            depDate = DateTime.SpecifyKind(new DateTime(arrDate.Year, arrDate.Month, arrDate.Day,
                                Convert.ToInt32(timeDeparture.Split(':')[0]),
                                Convert.ToInt32(timeDeparture.Split(':')[1]), 0), DateTimeKind.Utc);
                        }

                        else
                        {
                            var newDay = arrDate.AddDays(1);
                            depDate = DateTime.SpecifyKind(new DateTime(newDay.Year, newDay.Month, newDay.Day,
                                Convert.ToInt32(timeDeparture.Split(':')[0]),
                                Convert.ToInt32(timeDeparture.Split(':')[1]), 0), DateTimeKind.Utc);
                            //depDate = depdate.Day +1 + " " + depdate.ToString("MMM") + " " + depdate.Year + " " + timeDeparture;
                        }


                        //Column 3

                        var arrival = row.ChildElements.ToList()[3];
                        var k = arrival.InnerText.Split(' ');
                        var timeArrival = k[k.Length - 1];

                        //var newday = 6;
                        jamdatang = Convert.ToInt32(timeArrival.Split(':')[0]);
                        jamberangkat = Convert.ToInt32(timeDeparture.Split(':')[0]);
                        if (jamdatang < jamberangkat)
                        {
                            changeDay = true;
                        }
                        else
                        {
                            changeDay = false;
                        }

                        if (changeDay)
                        {
                            var newDay = depDate.AddDays(1);
                            arrDate = DateTime.SpecifyKind(new DateTime(newDay.Year, newDay.Month, newDay.Day,
                                Convert.ToInt32(timeArrival.Split(':')[0]),
                                Convert.ToInt32(timeArrival.Split(':')[1]), 0), DateTimeKind.Utc);
                        }
                        else
                        {
                            arrDate = DateTime.SpecifyKind(new DateTime(depDate.Year, depDate.Month, depDate.Day,
                                Convert.ToInt32(timeArrival.Split(':')[0]),
                                Convert.ToInt32(timeArrival.Split(':')[1]), 0), DateTimeKind.Utc);
                        }

                        segments.Add(new FlightSegment
                        {
                            AirlineCode = flightNo.Split()[0],
                            FlightNumber = flightNo.Split()[1],
                            CabinClass = cabinClass,
                            DepartureTime = depDate,
                            ArrivalTime = arrDate,
                            OperatingAirlineCode = flightNo.Split()[0],
                            AircraftCode = aircraftNo,
                            AirlineName = airplaneName,
                            OperatingAirlineName = airplaneName,
                            ArrivalAirport = airportarrival,
                            DepartureAirport = airportdeparture
                        });
                    }

                    int passengers = adultCount + childCount; //jumlah adult + children
                    var colCollection = new List<List<String>>();
                    var seatCollection = new List<List<String>>();
                    //PILIH COLUMN YG MASIH AVAILABLE 
                    switch (cabinClass)
                    {
                        case CabinClass.Economy:
                            {
                                for (int i = 0; i < selectedRows.Count; i++)
                                {
                                    colCollection.Add(new List<String>());
                                    seatCollection.Add(new List<String>());
                                    var selectedColumns =
                                        selectedRows[i].ChildElements.ToList().GetRange(9, 18).ToList();
                                    for (int col = 0; col < selectedColumns.Count; col++)
                                    {
                                        if (selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle_tconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle_bconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle_mconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right_tconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right_bconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right_mconx")
                                        {
                                            if (selectedColumns[col].InnerText != "No Fares")
                                            {
                                                if (
                                                    selectedColumns[col].ChildElements.ToList()[1].ChildElements.ToList()
                                                        .Count() == 1
                                                    &&
                                                    Convert.ToInt32(
                                                        selectedColumns[col].ChildElements.ToList()[0].ChildElements
                                                            .ToList()[1].InnerText) >= passengers)
                                                {
                                                    colCollection[i].Add(
                                                        selectedColumns[col].GetAttribute("id")
                                                            .SubstringBetween(0,
                                                                selectedColumns[col].GetAttribute("id").Length - 3));
                                                    seatCollection[i].Add(selectedColumns[col].ChildElements.ToList()[0].GetAttribute("title"));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case CabinClass.Business:
                            {
                                for (int i = 0; i < selectedRows.Count; i++)
                                {
                                    colCollection.Add(new List<string>());
                                    seatCollection.Add(new List<string>());
                                    var selectedColumns = selectedRows[i].ChildElements.ToList().GetRange(4, 5).ToList();
                                    for (int col = 0; col < selectedColumns.Count; col++)
                                    {
                                        if (selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle_tconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle_bconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_middle_mconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right_tconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right_bconx"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right"
                                            &&
                                            selectedColumns[col].GetAttribute("class") !=
                                            "step2_soldcell fareInfo_right_mconx"
                                            )
                                        {
                                            if (selectedColumns[col].InnerText != "No Fares")
                                            {
                                                if (
                                                    selectedColumns[col].ChildElements.ToList()[1].ChildElements.ToList()
                                                        .Count() == 1
                                                    &&
                                                    Convert.ToInt32(
                                                        selectedColumns[col].ChildElements.ToList()[0].ChildElements
                                                            .ToList()[1].InnerText) >= passengers)
                                                {
                                                    colCollection[i].Add(
                                                        selectedColumns[col].GetAttribute("id")
                                                            .SubstringBetween(0,
                                                                selectedColumns[col].GetAttribute("id").Length - 3));
                                                    seatCollection[i].Add(selectedColumns[col].ChildElements.ToList()[0].GetAttribute("title"));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                    }

                    //priceCollections = list of string, isinya kolom paling kanan/terakhir dari tiap segmen
                    var priceCollections = new List<string>();
                    var seats = new List<string>();
                    foreach (var seg in colCollection)
                    {
                        priceCollections.Add(seg[seg.Count - 1]);
                    }

                    string seat;
                    foreach (var seg in seatCollection)
                    {
                        seats.Add(seg[seg.Count - 1]);
                    }

                    seat = string.Join("|", seats.ToArray());

                    var agentprice = "";
                    if (priceCollections.Count != 0) // Kalau casenya cabin business kdg2 suka habis
                    {
                        var postdata = new CreatePostData();
                        colpost = postdata.Create(Rows, priceCollections);
                        garbled =
                            "ScriptManager1=upnlTotalTripCost%7CbtnPriceSelection&__EVENTTARGET=btnPriceSelection&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=";
                         b =
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
                            //CHANGE
                            "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
                            "&UcFlightSelection%24ddlADTCount=" + adultCount + //CHANGE
                            "&UcFlightSelection%24txtSelDes=" + dest + //CHANGE
                            "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + dest + ")" + //CHANGE
                            //"&UcFlightSelection%24ddlRetMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year + //CHANGE
                            //"&UcFlightSelection%24ddlRetDay=" + depdate.Day + //CHANGE
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
                        var url6 = @"LionAirAgentsIBE/Step2Availability.aspx";
                        var searchRequest6 = new RestRequest(url6, Method.POST);
                        searchRequest6.AddHeader("Accept-Encoding", "gzip, deflate");
                        searchRequest6.AddHeader("Accept", "*///*");
                        searchRequest6.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                        searchRequest6.AddHeader("Origin", "https://agent.lionair.co.id");
                        searchRequest6.AddHeader("Cache-Control", "no-cache");
                        searchRequest6.AddHeader("X-Requested-With", "XMLHttpRequest");
                        searchRequest6.AddHeader("X-MicrosoftAjax", "Delta=true");
                        vs5 = HttpUtility.UrlEncode(pageFlight["#__VIEWSTATE"].Attr("value"));
                        var postData6 = garbled + vs5 + colpost + b +
                                        "&__ASYNCPOST=true&";
                        searchRequest6.AddParameter("application/x-www-form-urlencoded", postData6,
                            ParameterType.RequestBody);
                        //Thread.Sleep(1000);
                        var searchResponse6 = client.Execute(searchRequest6);
                        var html6 = searchResponse6.Content;
                        var pagePrice = (CQ)html6;
                        var revalidateFare = pagePrice["#tdAmtTotal"].Text();
                        var sampah = pagePrice.Text();
                        var startvs = sampah.IndexOf("__VIEWSTATE");
                        var xyz = sampah.SubstringBetween(startvs + 12, sampah.Length);
                        var myvs = HttpUtility.UrlEncode(xyz.Split('|')[0]);
                        agentprice = revalidateFare.Replace(",", "");

                        // POST SEBELUM SEARCH LAGI
                        var url7 = @"LionAirAgentsIBE/Step2Availability.aspx";
                        var searchRequest7 = new RestRequest(url7, Method.POST);
                        searchRequest7.AddHeader("Accept-Encoding", "gzip, deflate");
                        searchRequest7.AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*///*;q=0.8");
                        searchRequest7.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
                        searchRequest7.AddHeader("Origin", "https://agent.lionair.co.id");
                        searchRequest7.AddHeader("Cache-Control", "max-age=0");
                        var postData7 = colpost.SubstringBetween(1, colpost.Length) + b + "&__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=" +
                                        myvs; //UBAH KE lbGoBack kalau udah bener
                        searchRequest7.AddParameter("application/x-www-form-urlencoded", postData7,
                            ParameterType.RequestBody);
                        //Thread.Sleep(1000);
                        var searchResponse7 = client.Execute(searchRequest7);

                        // GET PAGE NEW SEARCH
                        var url8 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
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

                        var pageBooking = (CQ) html8;
                        var vs9 = HttpUtility.UrlEncode(pageBooking["#__VIEWSTATE"].Attr("value"));
                        var beginning = "__EVENTTARGET=lbContinue&__EVENTARGUMENT=&__VIEWSTATE=" + vs9;
                        string ending =
                            "&txtRemark=&payDet=rbPay_HOLD&CreditCardDisplay1%24CreditCardType=VI&CreditCardDisplay1%24txtCardHolderName=&CreditCardDisplay1%24CreditCardNumber=&CreditCardDisplay1%24CreditCardExpiryMonth=MM&CreditCardDisplay1%24CreditCardExpiryYear=YY&CreditCardDisplay1%24CVVNumber=&AcceptFareConditions=on&FlightInfo=&AXTotal=&DCTotal=&OtherTotal=&nameMismatch=";

                        var orderedPassengers = bookInfo.Passengers.OrderBy(x => x.Type);
                        string dataPassenger = "";
                        string postDataBooking;
                        if (destinationCountry == "ID")
                        {
                            var infpax = 1;
                            for (var i = 0; i < orderedPassengers.Count(); i++)
                            {
                                var title = "";
                                string mealrequest;
                                if (orderedPassengers.ElementAt(i).Title == Title.Miss)
                                {
                                    title = "Miss";
                                }
                                else if (orderedPassengers.ElementAt(i).Title == Title.Mister)
                                {
                                    if (orderedPassengers.ElementAt(i).Type == PassengerType.Adult)
                                        title = "Mr";
                                    else
                                        title = "Mstr";
                                }
                                else if (orderedPassengers.ElementAt(i).Title == Title.Mistress)
                                {
                                    title = "Mrs";
                                }

                                
                                if (orderedPassengers.ElementAt(i).Type == PassengerType.Adult)
                                {
                                    mealrequest = "No+Preference";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers.ElementAt(i).FirstName.Split(' ')) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers.ElementAt(i).LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlAirline=JT" + //codeFlight + //NOT YET
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                        "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest;
                                }
                                else if (orderedPassengers.ElementAt(i).Type == PassengerType.Child)
                                {
                                    mealrequest = "No+Preference";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers.ElementAt(i).FirstName.Split(' ')) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers.ElementAt(i).LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlAirline=JT" +// codeFlight + //NOT YET
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest;
                                }
                                else if (orderedPassengers.ElementAt(i).Type == PassengerType.Infant)
                                {
                                    mealrequest = "BBML";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers.ElementAt(i).FirstName.Split(' ')) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers.ElementAt(i).LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlINFPaxAssoc=" + infpax +
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "$ddlMealRequest=" + mealrequest;
                                    infpax += 1;
                                }
                                //infpax += 1;
                            }
                        }
                        else
                        {
                            var infpax = 1;
                            for (var i = 0; i < orderedPassengers.Count(); i++)
                            {
                                var gender = "";
                                var title = "";
                                string mealrequest;
                                if (orderedPassengers.ElementAt(i).Gender == Gender.Male)
                                {
                                    gender = "M";
                                }
                                else if (orderedPassengers.ElementAt(i).Gender == Gender.Female)
                                {
                                    gender = "F";
                                }

                                if (orderedPassengers.ElementAt(i).Title == Title.Miss)
                                {
                                    title = "Miss";
                                }
                                else if (orderedPassengers.ElementAt(i).Title == Title.Mister)
                                {
                                    if (orderedPassengers.ElementAt(i).Type == PassengerType.Adult)
                                        title = "Mr";
                                    else
                                        title = "Mstr";
                                }
                                else if (orderedPassengers.ElementAt(i).Title == Title.Mistress)
                                {
                                    title = "Mrs";
                                }

                                if (orderedPassengers.ElementAt(i).Type == PassengerType.Infant)
                                {
                                    mealrequest = "BBML";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers.ElementAt(i).FirstName.Split(' ')) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers.ElementAt(i).LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlGender=" + gender +
                                        "&NameBlock" + (i + 1) + "%24ddlINFPaxAssoc=" + infpax +
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest +
                                        "&NameBlock" + (i + 1) + "%24txtPassportNumber=" +
                                        orderedPassengers.ElementAt(i).PassportNumber +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpDay=" +
                                        orderedPassengers.ElementAt(i).PassportExpiryDate.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpMon=" +
                                        orderedPassengers.ElementAt(i).PassportExpiryDate.GetValueOrDefault().ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpYear=" +
                                        orderedPassengers.ElementAt(i).PassportExpiryDate.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlDocCountry=" +
                                        orderedPassengers.ElementAt(i).PassportCountry +
                                        "&NameBlock" + (i + 1) + "%24ddlPaxCountry=" +
                                        orderedPassengers.ElementAt(i).PassportCountry;
                                    infpax += 1;
                                }
                                else
                                {
                                    mealrequest = "No+Preference";
                                    dataPassenger +=
                                        "&NameBlock" + (i + 1) + "%24ddlTitle=" + title +
                                        "&NameBlock" + (i + 1) + "%24txtFirstName=" +
                                        String.Join("+", orderedPassengers.ElementAt(i).FirstName) +
                                        "&NameBlock" + (i + 1) + "%24txtLastName=" +
                                        orderedPassengers.ElementAt(i).LastName +
                                        "&NameBlock" + (i + 1) + "%24ddlGender=" + gender +
                                        "&NameBlock" + (i + 1) + "%24ddlAirline=JT" + //codeFlight +
                                        "&NameBlock" + (i + 1) + "%24ddlSpecRequest=NA" +
                                        "&NameBlock" + (i + 1) + "%24txtFFNo=" +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBDay=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBMonth=" +
                                        orderedPassengers.ElementAt(i)
                                            .DateOfBirth.GetValueOrDefault()
                                            .ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlDOBYear=" +
                                        orderedPassengers.ElementAt(i).DateOfBirth.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlMealRequest=" + mealrequest +
                                        "&NameBlock" + (i + 1) + "%24txtPassportNumber=" +
                                        orderedPassengers.ElementAt(i).PassportNumber +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpDay=" +
                                        orderedPassengers.ElementAt(i)
                                            .PassportExpiryDate.GetValueOrDefault()
                                            .ToString("dd") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpMon=" +
                                        orderedPassengers.ElementAt(i)
                                            .PassportExpiryDate.GetValueOrDefault()
                                            .ToString("MMM") +
                                        "&NameBlock" + (i + 1) + "%24ddlPassportExpYear=" +
                                        orderedPassengers.ElementAt(i).PassportExpiryDate.GetValueOrDefault().Year +
                                        "&NameBlock" + (i + 1) + "%24ddlDocCountry=" +
                                        orderedPassengers.ElementAt(i).PassportCountry +
                                        "&NameBlock" + (i + 1) + "%24ddlPaxCountry=" +
                                        orderedPassengers.ElementAt(i).PassportCountry;
                                }
                                //infpax += 1;
                            }
                        }

                        var middle = "";
                        for (var x = 0; x < segmentCount; x++)
                        {
                            for (var y = 0; y < adultCount + childCount; y++)
                            {
                                middle += "&Bag_" + origin + "%24" + dest + "%24" + x + "%24" + y + "=";
                            }
                        }

                        var middle1 = "&hdnBagSelection=";
                        for (var x = 0; x < adultCount + childCount; x++)
                        {
                            middle1 += "%2C%7C";
                        }
                        middle1 += "&hdnDepPorts=&hdnArrPorts=";

                        var c = bookInfo.ContactData.Name;
                        string contactFirstname, contactLastname;
                        string contactTitle = "";
                        if (c.Split(' ').Count() == 1)
                        {
                            contactFirstname = c;
                            contactLastname = c;
                        }
                        else
                        {
                            contactLastname = c.Split(' ').Last();
                            contactFirstname = String.Join("+", c.Split(' ').ToList().Take(c.Split(' ').Length-1));
                        }

                        if (bookInfo.ContactData.Title == Title.Miss)
                        {
                            contactTitle = "Miss";
                        }
                        else if (bookInfo.ContactData.Title == Title.Mister)
                        {
                            contactTitle = "Mr";
                        }
                        else if (bookInfo.ContactData.Title == Title.Mistress)
                        {
                            contactTitle = "Mrs";
                        }

                        var cntct =
                            "&ContactTitle=" + contactTitle +
                            "&ContactFirstName=" + contactFirstname +
                            "&ContactLastName=" + contactLastname +
                            "&txtAddress1=" +
                            "&txtAddress2=" +
                            "&ddlCountry=ID" + //bookInfo.ContactData.CountryCode + ///change?
                            ///
                            "&txtCity=" +
                            "&txtPostCode=" +
                            "&txtCountryCode1=" + bookInfo.ContactData.CountryCode +
                            "&txtAreaCode1=" +
                            "&txtPhoneNumber1=" + bookInfo.ContactData.Phone +
                            "&ddlOriNumber=M" +
                            "&txtCountryCode3=" +
                            "&txtPhoneNumber3=" +
                            "&txtEmailAddress1=" + HttpUtility.UrlEncode("dwi.agustina@travelmadezy.com") + //email agent
                            "&txtEmailAddress2=" + HttpUtility.UrlEncode(bookInfo.ContactData.Email);

                        Thread.Sleep(1000);
                        postDataBooking = beginning + dataPassenger + middle + middle1 + cntct + ending;
                        client.FollowRedirects = false;

                        var urlBooking = @"LionAirAgentsIBE/Step3.aspx";
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

                        var url9 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
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

                        var url10 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
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
                        var bookingData = (CQ) html10;

                        bookingReference = bookingData["#lblRefNumber"].Text();
                        bookingTimeLimit = bookingData["#lblPayByDate"].Text();

                        var url15 = @"/LionAirAgentsPortal/Logout.aspx";
                        var searchRequest15 = new RestRequest(url15, Method.GET);
                        searchRequest15.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                        searchRequest15.AddHeader("Content-Encoding", "gzip");
                        searchRequest15.AddHeader("Host", "agent.lionair.co.id");
                        searchRequest15.AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        searchRequest15.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?" + cid);
                        Thread.Sleep(1000);
                        var searchResponse15 = client.Execute(searchRequest15);

                        //GET PAGE DEFAULT(HOME)

                        var url16 = @"/LionAirAgentsPortal/Default.aspx";
                        var searchRequest16 = new RestRequest(url16, Method.GET);
                        searchRequest16.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                        searchRequest16.AddHeader("Content-Encoding", "gzip");
                        searchRequest16.AddHeader("Host", "agent.lionair.co.id");
                        searchRequest16.AddHeader("Accept",
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                        searchRequest16.AddHeader("Referer",
                            "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?" + cid);
                        Thread.Sleep(1000);
                        var searchResponse16 = client.Execute(searchRequest16);
                    }

                    var format = "dd MMMM', 'yyyy', 'HH:mm";
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    if (bookingReference.Length == 0)
                    {
                        return new BookFlightResult
                        {
                            IsSuccess = false,
                            Errors = new List<FlightError> { FlightError.TechnicalError },
                            //ErrorMessages = new List<string> { "Web Layout Changed!" }
                        };
                    }
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
                        }
                    };
                 }
                 catch
                 {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> {"Web Layout Changed!"}
                    };
                }
            }
        }
    }
    //POST UNTUK BACK

    //var url11 = @"LionAirAgentsIBE/Step4.aspx";
    //var searchRequest11 = new RestRequest(url11, Method.POST);
    //searchRequest11.AddHeader("Accept-Encoding", "gzip, deflate");
    //searchRequest11.AddHeader("Accept",
    //    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
    //searchRequest11.AddHeader("Referer",
    //    "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
    //var vs11 = HttpUtility.UrlEncode(bookingData["#__VIEWSTATE"].Attr("value"));
    //searchRequest11.AddHeader("Origin", "https://agent.lionair.co.id");

    //var postdata11 = "__EVENTTARGET=lbExit&__EVENTARGUMENT=&__VIEWSTATE=%" + vs11;
    //searchRequest11.AddParameter("application/x-www-form-urlencoded", postdata11,
    //    ParameterType.RequestBody);
    //var searchResponse11 = client.Execute(searchRequest11);

    // GET

    //var url12 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
    //var searchRequest12 = new RestRequest(url12, Method.GET);
    //searchRequest12.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
    //searchRequest12.AddHeader("Content-Encoding", "gzip");
    //searchRequest12.AddHeader("Host", "agent.lionair.co.id");
    //searchRequest12.AddHeader("Accept",
    //    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
    //searchRequest12.AddHeader("Referer",
    //    "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
    //var searchResponse12 = client.Execute(searchRequest12);
    //var html12 = searchResponse12.Content;



    //var url13 = @"LionAirAgentsIBE/Step1.aspx";
    //var searchRequest13 = new RestRequest(url13, Method.POST);
    //searchRequest13.AddHeader("Accept-Encoding", "gzip, deflate");
    //searchRequest13.AddHeader("Accept",
    //    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
    //searchRequest13.AddHeader("Referer",
    //    "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
    //searchRequest13.AddHeader("Origin", "https://agent.lionair.co.id");
    //searchRequest13.AddHeader("Cache-Control", "max-age=0");
    //garbled = "__EVENTTARGET=UcFlightSelection%24lbGoBack&__EVENTARGUMENT=&__VIEWSTATE=";
    //var pageBack = (CQ) html12;
    //var vs13 = HttpUtility.UrlEncode(pageBack["#__VIEWSTATE"].Attr("value"));
    //b = "&UcFlightSelection%24TripType=rbOneWay" + "" +
    //    "&UcFlightSelection%24DateFlexibility=rbMustTravel" +
    //    "&UcFlightSelection%24txtSelOri=" + origin + //CHANGE
    //    "&UcFlightSelection%24txtOri=" + cityDep + "%20(" + origin + ")" + //CHANGE
    //    "&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year +
    //    CHANGE
    //    "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
    //    "&UcFlightSelection%24ddlADTCount=" + adultCount + //CHANGE
    //    "&UcFlightSelection%24txtSelDes=" + dest + //CHANGE
    //    "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + dest + ")" + //CHANGE
    //    "&UcFlightSelection%24ddlRetMonth=" + depdate.ToString("MMM") + "%202016" + //CHANGE
    //    "&UcFlightSelection%24ddlRetDay=" + depdate.Day + //CHANGE
    //    "&UcFlightSelection%24ddlCNNCount=" + childCount + //CHANGE
    //    "&UcFlightSelection%24ddlINFCount=" + infantCount + //CHANGE
    //    "&UcFlightSelection%24txtDepartureDate=" + depdate.Day + "%20" + depdate.ToString("MMM") +
    //    "%20" + depdate.Year + //CHANGE
    //    "&UcFlightSelection%24txtReturnDate=" + depdate.Day + "%20" + depdate.ToString("MMM") +
    //    "%20" + depdate.Year;
    //var postData13 = garbled + vs13 + b;
    //searchRequest13.AddParameter("application/x-www-form-urlencoded", postData13,
    //    ParameterType.RequestBody);
    //var searchResponse13 = client.Execute(searchRequest13);

    //GET PAGE WELCOME
    //var url14 = @"/LionAirAgentsPortal/Agents/Welcome.aspx?" + cid;
    //var searchRequest14 = new RestRequest(url14, Method.GET);
    //searchRequest14.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
    //searchRequest14.AddHeader("Content-Encoding", "gzip");
    //searchRequest14.AddHeader("Host", "agent.lionair.co.id");
    //searchRequest14.AddHeader("Accept",
    //    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
    //searchRequest14.AddHeader("Referer",
    //    "https://agent.lionair.co.id/LionAirAgentsIBE/OnlineBooking.aspx");
    //var searchResponse14 = client.Execute(searchRequest14);
}

