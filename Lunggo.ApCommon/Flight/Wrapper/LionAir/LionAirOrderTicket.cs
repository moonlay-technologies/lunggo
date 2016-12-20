using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env == "production")
                return Client.OrderTicket(bookingId);
            else
                return new IssueTicketResult
                {
                    IsSuccess = true,
                    BookingId = bookingId,
                    IsInstantIssuance = true
                };
        }

        private partial class LionAirClientHandler
        {
            internal IssueTicketResult OrderTicket(string bookingId)
            {
                var clientx = CreateAgentClient();
                clientx.FollowRedirects = false;
                CQ searchedHtml;
                var userId = "";
                //string currentDeposit;

                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clienty = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/LionAirAccount/ChooseUserId", Method.GET);
                var userName = "";
                var currentDeposit = "";
                RestResponse accRs;
                var reqTime = DateTime.UtcNow;
                var msgLogin = "Your login name is inuse";
                
                while (msgLogin == "Your login name is inuse" || msgLogin == "There was an error logging you in")
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && userName.Length == 0)
                    {
                        accRs = (RestResponse) clienty.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }

                    if (userName.Length == 0)
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> {FlightError.TechnicalError},
                            ErrorMessages = new List<string> { "[Lion Air] userName is full" }
                        };

                    bool successLogin;
                    do
                    {
                        clientx.BaseUrl = new Uri("https://agent.lionair.co.id");
                        const string url0 = @"/lionairagentsportal/default.aspx";
                        var searchRequest0 = new RestRequest(url0, Method.GET);
                        var searchResponse0 = clientx.Execute(searchRequest0);
                        var html0 = searchResponse0.Content;
                        searchedHtml = html0;
                        var viewstate = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));
                        var eventval = HttpUtility.UrlEncode(searchedHtml["#__EVENTVALIDATION"].Attr("value"));
                        FlightService.GetInstance().ParseCabinClass(CabinClass.Economy);
                        if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                            (searchResponse0.StatusCode == HttpStatusCode.OK ||
                             searchResponse0.StatusCode == HttpStatusCode.Redirect))
                            return new IssueTicketResult
                            {
                                Errors = new List<FlightError> {FlightError.InvalidInputData},
                                ErrorMessages = new List<string> { "[Lion Air] Error while requesting at /lionairagentsportal/default.aspx. Unexpected response path or response status code || " + searchResponse0.Content }
                            };
                        const string url1 = @"/lionairagentsportal/CaptchaGenerator.aspx";
                        var searchRequest1 = new RestRequest(url1, Method.GET);
                        var searchResponse1 = clientx.Execute(searchRequest1);
                        successLogin = Login(clientx, searchResponse1.RawBytes, viewstate, eventval, out userId,
                            userName, out msgLogin, out currentDeposit);
                    } while (!successLogin && (msgLogin != "Your login name is inuse"
                        && msgLogin != "There was an error logging you in"));
                }
                // Page Welcome
                var startind = userId.IndexOf("consID");
                var cid = userId.SubstringBetween(startind, userId.Length);
                cid = cid.SubstringBetween(7, cid.Length);

                //Page Masukin Booking Id
                var url3 = @"/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid;
                var searchRequest3 = new RestRequest(url3, Method.GET);
                searchRequest3.AddHeader("Referer",
                   "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?consID=" + cid);
                searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest3.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var searchResponse3 = clientx.Execute(searchRequest3);
                Thread.Sleep(1000);
                var html3 = searchResponse3.Content;
                searchedHtml = html3;
                var vs = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));

                //Post data booking id
                var searchRequest4 = new RestRequest(url3, Method.POST);
                searchRequest4.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid);
                searchRequest4.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest4.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                const string beginning = "__EVENTTARGET=lbSearch&__EVENTARGUMENT=&__VIEWSTATE=";
                var ending = "&txtBookingReloc=" + bookingId + "&txtPassengerName=&ddlDateSelection=";
                var postData4 = beginning + vs + ending;
                searchRequest4.AddParameter("application/x-www-form-urlencoded", postData4, ParameterType.RequestBody);
                Thread.Sleep(3000);
                var searchResponse4 = clientx.Execute(searchRequest4);
                
                //Page Tampilin Reservasi
                var url5 = @"/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId;
                var searchRequest5 = new RestRequest(url5, Method.GET);
                searchRequest5.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid);
                searchRequest5.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest5.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var searchResponse5 = clientx.Execute(searchRequest5);
                Thread.Sleep(3000);
                var html8 = searchResponse5.Content;
                searchedHtml = (CQ) html8;
                var vsPostToPay = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));

                try
                {
                    //GO TO PAY, POST

                    var searchRequest9 = new RestRequest(url5, Method.POST);
                    searchRequest9.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest9.AddHeader("Accept-Encoding", "gzip, deflate");
                    searchRequest9.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var beginningToPay = "__EVENTTARGET=lbContinue&__EVENTARGUMENT=&__VIEWSTATE=" + vsPostToPay;
                    const string endingToPay = "&payDet=rbPay_CA&CreditCardDisplay1%24CreditCardType=VI&CreditCardDisplay1%24txtCardHolderName=&CreditCardDisplay1%24CreditCardNumber=&CreditCardDisplay1%24CreditCardExpiryMonth=MM&CreditCardDisplay1%24CreditCardExpiryYear=YY&CreditCardDisplay1%24CVVNumber=&FlightInfo=&AXTotal=&DCTotal=&OtherTotal=&nameMismatch=";
                    var postData9 = beginningToPay + endingToPay;
                    searchRequest9.AddParameter("application/x-www-form-urlencoded", postData9, ParameterType.RequestBody);
                    var searchResponse9 = clientx.Execute(searchRequest9);
                    Thread.Sleep(3000);
                    if (searchResponse9.ResponseUri.AbsolutePath != "/LionAgentsOPS/TicketBooking.aspx"
                        && (searchResponse9.StatusCode == HttpStatusCode.OK || searchResponse9.StatusCode == HttpStatusCode.Redirect))
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[Lion Air]  || " + searchResponse9.Content }
                        };

                    //CashPayment

                    const string url10 = @"/LionAgentsOPS/CashPayment.aspx";
                    var searchRequest10 = new RestRequest(url10, Method.GET);
                    searchRequest10.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest10.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest10.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse10 = clientx.Execute(searchRequest10);
                    Thread.Sleep(3000);
                    if (searchResponse10.ResponseUri.AbsolutePath != "/LionAgentsOPS/CashPayment.aspx"
                        && (searchResponse10.StatusCode == HttpStatusCode.OK || searchResponse10.StatusCode == HttpStatusCode.Redirect))
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[Lion Air]  || " + searchResponse10.Content }
                        };

                    //Confirmation

                    const string url11 = @"/LionAgentsOPS/Confirmation.aspx";
                    var searchRequest11 = new RestRequest(url11, Method.GET);
                    searchRequest11.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest11.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest11.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse11 = clientx.Execute(searchRequest11);
                    if (searchResponse11.ResponseUri.AbsolutePath != "/LionAgentsOPS/Confirmation.aspx"
                        && (searchResponse11.StatusCode == HttpStatusCode.OK || searchResponse11.StatusCode == HttpStatusCode.Redirect))
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[Lion Air]  || " + searchResponse11.Content }
                        };
                    Thread.Sleep(3000);
                    var confirmationContent = searchResponse11.Content;
                    var html11 = (CQ)confirmationContent;
                    var booking = html11["#RelocHighlight"];
                    var bookingRef = booking.Children().ToList()[0].GetAttribute("id");

                    var theTable = html11[".Step4ItinRow"];
                    var isIssued = false;
                    if (!theTable.IsNullOrEmpty())
                    {
                        isIssued = theTable.Children().ToList()[7].InnerText == "Confirmed";
                    }
                    
                    LogOut(cid, clientx);
                    TurnInUsername(clienty, userName);

                    var abc = IsIssued(bookingId);
                    var isIssuedByFunction = abc == IssueEnum.IssueSuccess;
                    var isIssuedx = isIssued && isIssuedByFunction;
                    return new IssueTicketResult
                    {
                        IsSuccess = isIssuedx,
                        BookingId = isIssuedx ? bookingRef : null,
                        IsInstantIssuance = isIssuedx
                    };
                }
                catch
                {
                    LogOut(cid, clientx);
                    TurnInUsername(clienty, userName);

                    var isIssued = IsIssued(bookingId);
                    switch (isIssued)
                    {
                        case IssueEnum.IssueSuccess:
                            return new IssueTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId,
                                IsInstantIssuance = true
                            };
                        case IssueEnum.NotIssued:
                            return new IssueTicketResult
                            {
                                IsSuccess = false,
                                CurrentBalance = GetCurrentBalance()
                            };
                        case IssueEnum.CheckingError:
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "[Lion Air] Failed to check whether deposit cut or not! Manual checking advised!" }
                                
                            };
                        default:
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "[Lion Air] Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                    }
                }
            }
        }
    }
}
