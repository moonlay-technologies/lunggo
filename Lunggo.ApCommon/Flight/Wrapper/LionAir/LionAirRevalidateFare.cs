using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return Client.RevalidateFare(conditions);
        }

        private partial class LionAirClientHandler
        {
            internal RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
            {
                //conditions.FareId = "NTX+PKU+14 JUN 2016+7+0+0+Y+404700+FR00_C0_SLOT0+2+IW 1271|JT 235+10:35|13:30";
                if (conditions.Itinerary.FareId == null)
                {
                    //throw new Exception("revalidate 1");
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> { "Input data is invalid" }
                    };
                }

                List<string> listflight;
                List<string> listdephr;
                string origin, dest;
                var flights = new List<string>();
                var dephrs = new List<string>();
                int segmentCount;
                DateTime depdate;
                int adultCount, childCount, infantCount;
                decimal price;
                CabinClass cabinClass;
                try
                {
                    var splittedFareId = conditions.Itinerary.FareId.Split('+');
                    origin = splittedFareId[0];
                    dest = splittedFareId[1];
                    depdate = Convert.ToDateTime(splittedFareId[2]);
                    adultCount = Convert.ToInt32(splittedFareId[3]);
                    childCount = Convert.ToInt32(splittedFareId[4]);
                    infantCount = Convert.ToInt32(splittedFareId[5]);
                    cabinClass = FlightService.GetInstance().ParseCabinClass(splittedFareId[6]);
                    price = Convert.ToDecimal((splittedFareId[7]));
                    segmentCount = Convert.ToInt32(splittedFareId[9]);
                    listflight = splittedFareId[10].Split('|').ToList();
                    listdephr = splittedFareId[11].Split('|').ToList();
                }
                catch
                {
                    //throw new Exception("revalidate data parsing error");
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> {FlightError.FareIdNoLongerValid},
                        ErrorMessages = new List<string> { "FareId is no longer valid" }
                    };
                }


                if (adultCount == 0)
                {
                    //throw new Exception("revalidate adult = 0");
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> {"There Must be one adult"}
                    };
                }
                if (adultCount + childCount > 7)
                {
                    //throw new Exception("revalidate adult+children > 7");
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Total adult and children passenger must be not more than seven"}
                    };
                }
                if (adultCount < infantCount)
                {
                    //throw new Exception("revalidate adult < children");
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages =
                            new List<string> {"Every infant must be accompanied by one adult"}
                    };
                }
                if (depdate > DateTime.Now.AddDays(331).Date)
                {
                    //throw new Exception("revalidate > 331");
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> {FlightError.InvalidInputData},
                        ErrorMessages = new List<string> {"Time of departure exceeds"}
                    };
                }

                // [GET] Search Flight

                var client = CreateAgentClient();string userId = "";
                var msgLogin = "Your login name is inuse";
                int counter = 0;

                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clientx = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/LionAirAccount/ChooseUserId", Method.GET);
                var userName = "";
                var currentDeposit = "";
                RestResponse accRs;
                var reqTime = DateTime.UtcNow;
                while (msgLogin == "Your login name is inuse" || msgLogin == "There was an error logging you in")
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && userName.Length == 0)
                    {
                        accRs = (RestResponse) clientx.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }

                    if (userName.Length == 0)
                    {
                        return new RevalidateFareResult
                        {
                            Errors = new List<FlightError> {FlightError.TechnicalError},
                            ErrorMessages = new List<string> {"userName is full"}
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
                            accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + userName, Method.GET);
                            accRs = (RestResponse)clientx.Execute(accReq);
                            return new RevalidateFareResult
                            {
                                Errors = new List<FlightError> {FlightError.InvalidInputData},
                                ErrorMessages = new List<string> { "can't enter page default" }
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
                    } while (!successLogin && counter < 21 && (msgLogin != "Your login name is inuse"
                        && msgLogin != "There was an error logging you in"));
                }
            

                if (counter >= 21)
                {
                    accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + userName, Method.GET);
                    accRs = (RestResponse)clientx.Execute(accReq);
                    return new RevalidateFareResult
                    {
                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages = new List<string> { "Captcha is invalid" }
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
                Thread.Sleep(1000);
                var searchResponse2 = client.Execute(searchRequest2);
                //var deposit = currentDeposit.Split(' ')[1];
                
                //GET PAGE ONLINE BOOKING (PAGE MILIH PESAWAT)
                const string url3 = @"/LionAirAgentsIBE/OnlineBooking.aspx";
                var searchRequest3 = new RestRequest(url3, Method.GET);
                searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest3.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchRequest3.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx");
                Thread.Sleep(1000);
                var searchResponse3 = client.Execute(searchRequest3);
                
                var html3 = searchResponse3.Content;
                CQ vs = html3;
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
                    @"&UcFlightSelection%24txtSelOri=" + origin + // => should be variabel origin (ex: CGK)
                    @"&UcFlightSelection%24txtOri=" + cityDep + "+%28" + origin + "%29" +
                    @"&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "+" + depdate.Year +
                    @"&UcFlightSelection%24ddlDepDay=" + depdate.Day + // => should be taken from variabel depdate
                    @"&UcFlightSelection%24ddlADTCount=" + adultCount + //adultCount
                    @"&UcFlightSelection%24txtSelDes=" + dest + //=> should be variabel DEST (ex: CGK)
                    @"&UcFlightSelection%24txtDes=" + cityArr + "+%28" + dest + "%29" +
                    @"&UcFlightSelection%24ddlCNNCount=" + childCount + //childCount
                    @"&UcFlightSelection%24ddlINFCount=" + infantCount + //infantCount
                    @"&UcFlightSelection%24txtDepartureDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" + depdate.Year +
                    @"&UcFlightSelection%24txtReturnDate=" + depdate.Day + "+" + depdate.ToString("MMM") + "+" + depdate.Year; 
                searchRequest4.AddParameter("application/x-www-form-urlencoded", postData4, ParameterType.RequestBody);
                client.FollowRedirects = false;
                Thread.Sleep(5000);
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
                var searchResponse5 = client.Execute(searchRequest5);
                Thread.Sleep(1000);
                var html5 = searchResponse5.Content;

                var pageFlight = (CQ) html5;

                try
                {
                    var rows = pageFlight["#tblOutFlightBlocks > tbody"].Children();
                    var selectedRows = new List<IDomObject>();
                    
                    // PILIH ROW DAN SEGMENNYA
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
		                    int z = v + 1;
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

                    var segments = new List<FlightSegment>();
                    var arrDate = new DateTime(depdate.Year, depdate.Month, depdate.Day, 0, 0, 0);
                    var txt_OBNNRowID = selectedRows.Last().Id;
                    foreach (var row in selectedRows)
                    {
                        //Column 0
                        var flightIdty = row.ChildElements.ToList()[0];
                        var flightNo = "JT 34";
                        var aircraftNo = "737-900ER";
                        var airplaneName = "Lion Air";
                        switch (flightIdty.ChildElements.ToList().Count)
                        {
                            case 2:
                                aircraftNo = flightIdty.ChildElements.ToList()[1].InnerText;
                                break;
                            case 1:
                                aircraftNo = null;
                                break;
                        }

                        flightNo = flightIdty.ChildElements.ToList()[0].InnerText;
                        flights.Add(flightNo);
                        switch (flightNo.Split(' ')[0])
                        {
                            case "JT":
                                airplaneName = "Lion Air";
                                break;
                            case "IW":
                                airplaneName = "Wings Air";
                                break;
                            case "ID":
                                airplaneName = "Batik Air";
                                break;
                            case "OD":
                                airplaneName = "Malindo Air";
                                break;
                            case "SL":
                                airplaneName = "Thai Lion Air";
                                break;
                        }

                        //Column 1

                        var colhidden = row.ChildElements.ToList()[1];
                        var hidtext = colhidden.InnerText.Split('|')[6];
                        var airportdeparture = hidtext.SubstringBetween(0, 3);
                        var airportarrival = hidtext.SubstringBetween(3, hidtext.Length);

                        //Column 2

                        var departure = row.ChildElements.ToList()[2];
                        var x = departure.InnerText.Split(' ');
                        var timeDeparture = x[x.Length - 1];
                        dephrs.Add(timeDeparture);
                        DateTime depDate;
                        var jamberangkat = Convert.ToInt32(timeDeparture.Split(':')[0]);
                        var jamdatang = arrDate.Hour;
                        var changeDay = jamdatang > jamberangkat;

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
                        }

                        //Column 3
                        var arrival = row.ChildElements.ToList()[3];
                        var k = arrival.InnerText.Split(' ');
                        var timeArrival = k[k.Length - 1];

                        jamdatang = Convert.ToInt32(timeArrival.Split(':')[0]);
                        jamberangkat = Convert.ToInt32(timeDeparture.Split(':')[0]);
                        changeDay = jamdatang < jamberangkat;

                        if (changeDay)
                        {
                            var newDay = depDate.AddDays(1);
                            arrDate =  DateTime.SpecifyKind(new DateTime(newDay.Year, newDay.Month, newDay.Day,
                                Convert.ToInt32(timeArrival.Split(':')[0]),
                                Convert.ToInt32(timeArrival.Split(':')[1]), 0), DateTimeKind.Utc);
                        }
                        else
                        {
                            arrDate = DateTime.SpecifyKind(new DateTime(depDate.Year, depDate.Month, depDate.Day,
                                Convert.ToInt32(timeArrival.Split(':')[0]),
                                Convert.ToInt32(timeArrival.Split(':')[1]), 0), DateTimeKind.Utc);
                        }

                        FlightService.GetInstance().GetAirportTimeZone(airportarrival);
                        FlightService.GetInstance().GetAirportTimeZone(airportdeparture);

                        var dur = arrDate.AddHours(-FlightService.GetInstance().GetAirportTimeZone(airportarrival)) - 
                            depDate.AddHours(-FlightService.GetInstance().GetAirportTimeZone(airportdeparture));

                        segments.Add(new FlightSegment
                        {
                            AirlineCode = flightNo.Split()[0],
                            FlightNumber = flightNo.Split()[1],
                            CabinClass = cabinClass,
                            AirlineType = AirlineType.Lcc,
                            DepartureTime = depDate,
                            ArrivalTime = arrDate,
                            OperatingAirlineCode = flightNo.Split()[0],
                            AircraftCode = aircraftNo,
                            AirlineName = airplaneName,
                            OperatingAirlineName = airplaneName,
                            ArrivalAirport = airportarrival,
                            DepartureAirport = airportdeparture,
                            Duration = dur
                        });
                    }

                    var isSegmentEqual = segmentCount == segments.Count();
                    var isFlightSame = true;
                    var isDepHrSame = true;
                    for (var ind = 0; ind < segments.Count; ind++)
                    {
                        if (flights[ind] != listflight[ind])
                            isFlightSame = false;
                        if (listdephr[ind] != dephrs[ind])
                            isDepHrSame = false;
                    }
                    int passengers = adultCount + childCount; //jumlah adult + children
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
                                foreach (var col in selectedColumns)
                                {
                                    if (col.GetAttribute("class") != "step2_soldcell fareInfo_middle_tconx"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_middle_bconx"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_middle"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_middle_mconx"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_right_tconx"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_right_bconx"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_right"
                                        && col.GetAttribute("class") != "step2_soldcell fareInfo_right_mconx")
                                    {
                                        if (col.InnerText != "No Fares")
                                        {
                                            if (
                                                col.ChildElements.ToList()[1].ChildElements.ToList()
                                                    .Count() == 1
                                                &&
                                                Convert.ToInt32(
                                                    col.ChildElements.ToList()[0].ChildElements
                                                        .ToList()[1].InnerText) >= passengers)
                                            {
                                                colCollection[i].Add(col.GetAttribute("id")
                                                        .SubstringBetween(0, col.GetAttribute("id").Length - 3));
                                                seatCollection[i].Add(col.ChildElements.ToList()[0].GetAttribute("title"));
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
                                                && Convert.ToInt32(t.ChildElements.ToList()[0].ChildElements
                                                    .ToList()[1].InnerText) >= passengers)
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
                    }

                    //priceCollections = list of string, isinya kolom paling kanan/terakhir dari tiap segmen
                    var priceCollections = colCollection.Select(seg => seg[seg.Count - 1]).ToList();
                    //string seat = string.Join("|", seatCollection.Select(seg => seg[seg.Count - 1]).ToArray());

                    var agentprice = "";
                    if (priceCollections.Count != 0) // Kalau casenya cabin business kdg2 suka habis
                    {
                        var postdata = new CreatePostData();
                        var colpost = postdata.Create(rows, priceCollections);
                        const string garbled =
                            "ScriptManager1=upnlTotalTripCost%7CbtnPriceSelection&__EVENTTARGET=btnPriceSelection&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=";
                        
                        //if (originCountry == "ID")
                        //{
                            var b = "&txtUpdateInsurance=no" +
                                       "&Insurance%24rblInsurance=No" +
                                       "&Insurance%24txtInsPostbackRequired=no" +
                                       "&txtPricingResponse=OK" + "" +
                                       "&txtOutFBCsUsed=" + //seat +
                                       "&txtInFBCsUsed=" +
                                       "&txtTaxBreakdown=" +
                                       //"&lbContinue.x=39&lbContinue.y=11" +
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
                        //}
                        //else
                        //{
                        //    b =
                        //    "&txtUpdateInsurance="+
                        //    "&Insurance%24txtInsPostbackRequired=no"+
                        //    "&txtPricingResponse=OK" + "" +
                        //    "&txtOutFBCsUsed=" + //seat +
                        //    "&txtInFBCsUsed=" +
                        //    "&txtTaxBreakdown=" +
                        //    "&lbContinue.x=39&lbContinue.y=11" +
                        //    "&UcFlightSelection%24TripType=rbOneWay" + "" +
                        //    "&UcFlightSelection%24DateFlexibility=rbMustTravel" +
                        //    "&UcFlightSelection%24txtSelOri=" + origin + //CHANGE
                        //    "&UcFlightSelection%24txtOri=" + cityDep + "%20(" + origin + ")" + //CHANGE
                        //    "&UcFlightSelection%24ddlDepMonth=" + depdate.ToString("MMM") + "%20" + depdate.Year +
                        //    "&UcFlightSelection%24ddlDepDay=" + depdate.Day + //CHANGE
                        //    "&UcFlightSelection%24ddlADTCount=" + adultCount + //CHANGE
                        //    "&UcFlightSelection%24txtSelDes=" + dest + //CHANGE
                        //    "&UcFlightSelection%24txtDes=" + cityArr + "%20(" + dest + ")" + //CHANGE
                        //    "&UcFlightSelection%24ddlCNNCount=" + childCount + //CHANGE
                        //    "&UcFlightSelection%24ddlINFCount=" + infantCount + //CHANGE
                        //    "&UcFlightSelection%24txtDepartureDate=" + depdate.ToString("dd") + "%20" + depdate.ToString("MMM") +
                        //    "%20" + depdate.Year + //CHANGE
                        //    "&UcFlightSelection%24txtReturnDate=" + depdate.ToString("dd") + "%20" + depdate.ToString("MMM") +
                        //    "%20" + depdate.Year + //CHANGE
                        //    "&txtOBNNCellID=" + string.Join("|", priceCollections.ToArray()) +
                        //    "&txtIBNNCellID=oneway" +
                        //    "&txtOBNNRowID=" + txt_OBNNRowID +
                        //    "&txtIBNNRowID=" +
                        //    "&txtUserSelectedOneway=";
                        //}

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
                        var postData6 = garbled + vs5 + colpost + b;
                        searchRequest6.AddParameter("application/x-www-form-urlencoded", postData6,
                            ParameterType.RequestBody);
                        Thread.Sleep(1000);
                        var searchResponse6 = client.Execute(searchRequest6);
                        var html6 = searchResponse6.Content;

                        var pagePrice = (CQ) html6;

                        var revalidateFare = pagePrice["#tdAmtTotal"].Text();
                        agentprice = revalidateFare.Replace(",", "");
                        
                        //GET PAGE LOGOUT

                        const string url15 = @"/LionAirAgentsPortal/Logout.aspx";
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

                        const string url16 = @"/LionAirAgentsPortal/Default.aspx";
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

                        accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + userName, Method.GET);
                        accRs = (RestResponse)clientx.Execute(accReq);
                    }

                    var itin = new FlightItinerary
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
                        RequestedCabinClass = CabinClass.Economy,
                        TripType = TripType.OneWay,
                        Supplier = Supplier.LionAir,
                        Price = new Price(),
                        FareId = conditions.Itinerary.FareId,
                        Trips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                OriginAirport = origin,
                                DestinationAirport = dest,
                                DepartureDate = DateTime.SpecifyKind(depdate, DateTimeKind.Utc),
                                Segments = segments
                            }
                        }
                    };
                    itin.Price.SetSupplier(Convert.ToDecimal(agentprice), new Currency("IDR"));

                    var newPrice = Convert.ToDecimal(agentprice);
                    var result = new RevalidateFareResult
                    {
                        IsSuccess = true,
                        IsValid = true,
                        IsItineraryChanged = !(isDepHrSame && isFlightSame && isSegmentEqual),
                        IsPriceChanged = price != newPrice,
                        NewItinerary = itin,
                    };
                    if (result.IsPriceChanged)
                        result.NewPrice = newPrice;
                    return result;  

                }

                catch //(Exception e)
                {
                    //throw e;
                    //GET PAGE LOGOUT

                    const string url15 = @"/LionAirAgentsPortal/Logout.aspx";
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

                    const string url16 = @"/LionAirAgentsPortal/Default.aspx";
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

                    accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + userName, Method.GET);
                    accRs = (RestResponse)clientx.Execute(accReq);
                    return new RevalidateFareResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> {FlightError.TechnicalError},
                        ErrorMessages = new List<string> {"Web Layout Changed!"}
                    };
                }
            }

            private class GetCityAirportPair
            {
                private readonly Dictionary<string, string> CityAirportPair = new Dictionary<string, string>();

                public GetCityAirportPair()
                {
                    CityAirportPair.Add("AMD", "Ahmedabad");
                    CityAirportPair.Add("ARD", "Alor");
                    CityAirportPair.Add("AOR", "Alor+Setar");
                    CityAirportPair.Add("AMQ", "Ambon");
                    CityAirportPair.Add("VPM", "Ampana");
                    CityAirportPair.Add("ATQ", "Aritsar");
                    CityAirportPair.Add("ABU", "Atambua");
                    CityAirportPair.Add("BJW", "Bajawa");
                    CityAirportPair.Add("BPN", "Balikpapan");
                    CityAirportPair.Add("BTJ", "Banda Aceh");
                    CityAirportPair.Add("TKG", "Bandar+Lampung");
                    CityAirportPair.Add("BDO", "Bandung");
                    CityAirportPair.Add("DMK", "Bangkok+Donmueang");
                    CityAirportPair.Add("BKK", "Bangkok+Donmueang");
                    CityAirportPair.Add("BWX", "Banjarmasin");
                    CityAirportPair.Add("BTH", "Batam");
                    CityAirportPair.Add("BTW", "Batu+Licin");
                    CityAirportPair.Add("BUW", "Baubau");
                    CityAirportPair.Add("BKS", "Bengkulu");
                    CityAirportPair.Add("BEJ", "Berau");
                    CityAirportPair.Add("BIK", "Biak");
                    CityAirportPair.Add("BMU", "Bima");
                    CityAirportPair.Add("WUB", "Buli");
                    CityAirportPair.Add("UOL", "Buol");
                    CityAirportPair.Add("CNX", "Chiang+Mai");
                    CityAirportPair.Add("CEI", "Chiang+Rai");
                    CityAirportPair.Add("CGP", "Chittagong");
                    CityAirportPair.Add("VED", "Dekai%2FYahukimo");
                    CityAirportPair.Add("DEL", "Delhi");
                    CityAirportPair.Add("DPS", "Denpasar+%28Bali%29");
                    CityAirportPair.Add("DAC", "Dhaka");
                    CityAirportPair.Add("DUM", "Dumai");
                    CityAirportPair.Add("ENE", "Ende");
                    CityAirportPair.Add("FKQ", "Fak+Fak");
                    CityAirportPair.Add("GLX", "Galela");
                    CityAirportPair.Add("GTO", "Gorontalo");
                    CityAirportPair.Add("GNS", "Gunung+Sitoli");
                    CityAirportPair.Add("HDY", "Hat+Yai");
                    CityAirportPair.Add("SGN", "Ho+Chi+Minh+City");
                    CityAirportPair.Add("HHQ", "Huahin");
                    CityAirportPair.Add("IPH", "Ipoh");
                    CityAirportPair.Add("CGK", "Jakarta");
                    CityAirportPair.Add("HLP", "Jakarta+Halim+Perdanakusuma");
                    CityAirportPair.Add("DJB", "Jambi");
                    CityAirportPair.Add("DJJ", "Jayapura");
                    CityAirportPair.Add("JED", "Jeddah");
                    CityAirportPair.Add("JOG", "Jogjakarta");
                    CityAirportPair.Add("JHB", "Johor+Baru");
                    CityAirportPair.Add("KNG", "Kaimana");
                    CityAirportPair.Add("KTM", "Kathmandu");
                    CityAirportPair.Add("KDI", "Kendari");
                    CityAirportPair.Add("KTE", "Kerteh");
                    CityAirportPair.Add("KTG", "Ketapang");
                    CityAirportPair.Add("COK", "Kochi");
                    CityAirportPair.Add("KBR", "Kota+Bahru");
                    CityAirportPair.Add("BKI", "Kota+Kinabalu");
                    CityAirportPair.Add("KBU", "Kotabaru");
                    CityAirportPair.Add("KBV", "Krabi");
                    CityAirportPair.Add("KUL", "Kuala+Lumpur+%28KLIA2%29");
                    CityAirportPair.Add("TGG", "Kuala+Terengganu");
                    CityAirportPair.Add("KUA", "Kuantan");
                    CityAirportPair.Add("KCH", "Kuching");
                    CityAirportPair.Add("KOE", "Kupang");
                    CityAirportPair.Add("LBJ", "Labuan+Bajo");
                    CityAirportPair.Add("LAH", "Labuha");
                    CityAirportPair.Add("LUV", "Langgur%2FTual");
                    CityAirportPair.Add("LGK", "Langkawi");
                    CityAirportPair.Add("LKA", "Larantuka");
                    CityAirportPair.Add("LSW", "Lhokseumawe");
                    CityAirportPair.Add("LOP", "Lombok");
                    CityAirportPair.Add("LUW", "Luwuk");
                    CityAirportPair.Add("MKZ", "Malacca");
                    CityAirportPair.Add("MLG", "Malang");
                    CityAirportPair.Add("MJU", "Mamuju");
                    CityAirportPair.Add("MDC", "Manado");
                    CityAirportPair.Add("MKW", "Manokwari");
                    CityAirportPair.Add("MOF", "Maumere");
                    CityAirportPair.Add("KNO", "Medan+Kuala+Namu");
                    CityAirportPair.Add("MES", "Medan+Kuala+Namu");
                    CityAirportPair.Add("MNA", "Melanguane");
                    CityAirportPair.Add("MKQ", "Merauke");
                    CityAirportPair.Add("MEQ", "Meulaboh");
                    CityAirportPair.Add("MYY", "Miri");
                    CityAirportPair.Add("OTI", "Morotai");
                    CityAirportPair.Add("BOM", "Mumbai");
                    CityAirportPair.Add("NBX", "Nabire");
                    CityAirportPair.Add("NST", "Nakhon+Si+Thammarat");
                    CityAirportPair.Add("NTX", "Natuna+Ranai");
                    CityAirportPair.Add("PDG", "Padang");
                    CityAirportPair.Add("PKY", "Palangkaraya");
                    CityAirportPair.Add("PLM", "Palembang");
                    CityAirportPair.Add("UPU", "Palopo");
                    CityAirportPair.Add("PLW", "Palu");
                    CityAirportPair.Add("PGK", "Pangkal+Pinang");
                    CityAirportPair.Add("PKN", "Pangkalan+Bun");
                    CityAirportPair.Add("PKU", "Pekan+Baru");
                    CityAirportPair.Add("PEN", "Penang");
                    CityAirportPair.Add("PER", "Perth");
                    CityAirportPair.Add("HKT", "Phuket");
                    CityAirportPair.Add("PUM", "Pomalaa");
                    CityAirportPair.Add("PNK", "Pontianak");
                    CityAirportPair.Add("PSJ", "Poso");
                    CityAirportPair.Add("PSU", "Putus+Sibau");
                    CityAirportPair.Add("RTI", "Rote");
                    CityAirportPair.Add("SMQ", "Sampit");
                    CityAirportPair.Add("SQN", "Sanana");
                    CityAirportPair.Add("SXK", "Saumlaki");
                    CityAirportPair.Add("YKR", "Selayar");
                    CityAirportPair.Add("SRG", "Semarang");
                    CityAirportPair.Add("FLZ", "Sibolga");
                    CityAirportPair.Add("SBW", "Sibu");
                    CityAirportPair.Add("DTB", "Silangit");
                    CityAirportPair.Add("SMG", "Simeuleu");
                    CityAirportPair.Add("SIN", "Singapore");
                    CityAirportPair.Add("SQG", "Sintang");
                    CityAirportPair.Add("SOC", "Solo");
                    CityAirportPair.Add("SOQ", "Sorong");
                    CityAirportPair.Add("SZB", "Subang");
                    CityAirportPair.Add("SWQ", "Sumbawa");
                    CityAirportPair.Add("SUB", "Surabaya");
                    CityAirportPair.Add("URT", "Surat+Thani");
                    CityAirportPair.Add("NAH", "Tahuna");
                    CityAirportPair.Add("TMC", "Tambolaka");
                    CityAirportPair.Add("TJQ", "Tanjung+Pandan");
                    CityAirportPair.Add("TNJ", "Tanjung+Pinang");
                    CityAirportPair.Add("TJG", "Tanjung+Warukin");
                    CityAirportPair.Add("TRK", "Tarakan");
                    CityAirportPair.Add("TWU", "Tawau");
                    CityAirportPair.Add("TTE", "Ternate");
                    CityAirportPair.Add("TIM", "Timika");
                    CityAirportPair.Add("TRZ", "Tiruchirapally");
                    CityAirportPair.Add("KAZ", "Tobelo");
                    CityAirportPair.Add("TLI", "Toli+Toli");
                    CityAirportPair.Add("TRV", "Trivandum");
                    CityAirportPair.Add("UBP", "Ubon+Ratchathani");
                    CityAirportPair.Add("UTH", "Udon+Thani");
                    CityAirportPair.Add("UPG", "Ujung+Pandang");
                    CityAirportPair.Add("VTZ", "Visakhapatnam");
                    CityAirportPair.Add("WGP", "Waingapu");
                    CityAirportPair.Add("WNI", "Wakatobi");
                    CityAirportPair.Add("WMX", "Wamena");
                }

                public bool GetCity(string airport, out string city)
                {
                    return CityAirportPair.TryGetValue(airport, out city);
                }
            }

            private class GetLionAirPriceAgent
            {
                private readonly Dictionary<string, string> ColumnLetterPair = new Dictionary<string, string>();

                public GetLionAirPriceAgent()
                {
                    ColumnLetterPair.Add("0", "C");
                    ColumnLetterPair.Add("1", "J");
                    ColumnLetterPair.Add("2", "D");
                    ColumnLetterPair.Add("3", "I");
                    ColumnLetterPair.Add("4", "Z");
                    ColumnLetterPair.Add("5", "Y");
                    ColumnLetterPair.Add("6", "A");
                    ColumnLetterPair.Add("7", "G");
                    ColumnLetterPair.Add("8", "W");
                    ColumnLetterPair.Add("9", "S");
                    ColumnLetterPair.Add("10", "B");
                    ColumnLetterPair.Add("11", "H");
                    ColumnLetterPair.Add("12", "K");
                    ColumnLetterPair.Add("13", "L");
                    ColumnLetterPair.Add("14", "M");
                    ColumnLetterPair.Add("15", "N");
                    ColumnLetterPair.Add("16", "Q");
                    ColumnLetterPair.Add("17", "T");
                    ColumnLetterPair.Add("18", "V");
                    ColumnLetterPair.Add("19", "X");
                    ColumnLetterPair.Add("20", "R");
                    ColumnLetterPair.Add("21", "O");
                    ColumnLetterPair.Add("22", "U");
                }

                public bool GetLetter(string number, out string letter)
                {
                    return ColumnLetterPair.TryGetValue(number, out letter);
                }
            }

            private class CreatePostData
            {
                public string Create(CQ rows, List<string> idcols)
                {
                    var postdata = "";
                    var kamus = new GetLionAirPriceAgent();
                    var c = 0;
                    for (var i = 2; i < rows.Count() - 1; i++)
                    {
                        var cols = rows[i].ChildElements.ToList();
                        for (var j = 5; j < cols.Count; j++)
                        {
                            if (cols[j].GetAttribute("class") != "step2_soldcell fareInfo_middle_tconx"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_middle_bconx"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_middle"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_middle_mconx"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_right_tconx"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_right_bconx"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_right"
                                && cols[j].GetAttribute("class") != "step2_soldcell fareInfo_right_mconx")
                            {

                                if (cols[j].InnerText != "No Fares")
                                {

                                    var idc = cols[j].GetAttribute("id")
                                        .SubstringBetween(0, cols[j].GetAttribute("id").Length - 3);
                                    var splitted = idc.Split('_')[3];
                                    string let;
                                    var b = kamus.GetLetter(splitted.SubstringBetween(1, splitted.Length), out let);
                                    
                                    if (idcols.Any(idc.Contains))
                                    {
                                        postdata += "&RGM0_" + c + "=" + idc;
                                        c += 1;
                                    }
                                    
                                    if (cols[j].ChildElements.ToList()[1].ChildElements.ToList().Count() != 1)
                                    {

                                        postdata += "&" + idc + "_na=";
                                        postdata += "&" + idc + "_bc=" + let + "%2FHL";
                                    }
                                    else
                                    {
                                        postdata += "&" + idc + "_bc=" + let + "%2FSS";
                                    }
                                }
                            }
                        }
                    }
                    return postdata;
                }
            }

            //public static String GetDeposit(CQ pages)
            //{
            //    return pages["#ctl00_ContentPlaceHolder1_lblCreditAvailable"].Text();
            //}

        }
    }
  }

