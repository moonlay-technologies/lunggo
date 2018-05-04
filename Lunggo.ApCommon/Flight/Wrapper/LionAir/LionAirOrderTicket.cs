using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Log;
using RestSharp;
using Lunggo.ApCommon.Log;
using Lunggo.Framework.Environment;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            var env = EnvVariables.Get("general", "environment");
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
                GlobalLog TableLog = new GlobalLog();
                var client = CreateAgentClient();

                TableLog.PartitionKey = "FLIGHT ISSUE LOG";

                client.FollowRedirects = false;
                CQ searchedHtml;
                //string currentDeposit;

                string userName;
                string userId;
                string errorMessage;
                var log = LogService.GetInstance();
                TableLog.Log = "[Lion Air]  Proses Login.aspx. Booking ID: " + bookingId;
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

                var succeedLogin = Login(client, out userName, out userId, out errorMessage, 24*3600);
                if (!succeedLogin)
                {
                    return new IssueTicketResult
                    {
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { errorMessage }
                    };
                }

                // Page Welcome
                TableLog.Log = "[Lion Air] Welcome Page. Booking ID: " + bookingId;
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();
                var startind = userId.IndexOf("consID");
                var cid = userId.SubstringBetween(startind, userId.Length);
                cid = cid.SubstringBetween(7, cid.Length);

                //Page Masukin Booking Id
                TableLog.Log = "[Lion Air] Inserting Booking Id. Url : /LionAgentsOPS/TicketingQueue.aspx?consID=" + cid;
                log.Post(TableLog.Log + " Booking ID: " + bookingId, "#logging-issueflight");
                TableLog.Logging();

                var url3 = @"/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid;
                var searchRequest3 = new RestRequest(url3, Method.GET);
                searchRequest3.AddHeader("Referer",
                   "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?consID=" + cid);
                searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest3.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var searchResponse3 = client.Execute(searchRequest3);
                Thread.Sleep(1000);
                var html3 = searchResponse3.Content;
                searchedHtml = html3;
                var vs = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));

                //Post data booking id
                TableLog.Log = "[Lion Air] Post Booking Id. Url : " + url3 + " Booking ID: " + bookingId;
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

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
                var searchResponse4 = client.Execute(searchRequest4);

                //Page Tampilin Reservasi
                TableLog.Log = "[Lion Air] Page Show Reservation. Url : /LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId + " Booking ID: " + bookingId;
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

                var url5 = @"/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId;
                var searchRequest5 = new RestRequest(url5, Method.GET);
                searchRequest5.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid);
                searchRequest5.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest5.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var searchResponse5 = client.Execute(searchRequest5);
                Thread.Sleep(3000);
                var html8 = searchResponse5.Content;
                searchedHtml = (CQ) html8;
                var vsPostToPay = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));

                try
                {
                    //GO TO PAY, POST
                    TableLog.Log = "[Lion Air] [POST] Go to Pay. Url : /LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId + " Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
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
                    var searchResponse9 = client.Execute(searchRequest9);
                    Thread.Sleep(3000);
                    if (searchResponse9.ResponseUri.AbsolutePath != "/LionAgentsOPS/TicketBooking.aspx"
                        &&
                        (searchResponse9.StatusCode == HttpStatusCode.OK ||
                         searchResponse9.StatusCode == HttpStatusCode.Redirect))
                    {
                        TableLog.Log = "[Lion Air][GET] Response Uri is not /LionAgentsOPS/TicketBooking.aspx or Status is not (OK Or Redirected). Booking ID: " + bookingId;
                        log.Post(TableLog.Log, "#logging-issueflight");
                        TableLog.Logging();
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[Lion Air]  || " + searchResponse9.Content }
                        };
                    }

                    //CashPayment
                    TableLog.Log = "[Lion Air] [POST] Page CashPayment. Url : /LionAgentsOPS/CashPayment.aspx. Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    const string url10 = @"/LionAgentsOPS/CashPayment.aspx";
                    var searchRequest10 = new RestRequest(url10, Method.GET);
                    searchRequest10.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest10.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest10.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse10 = client.Execute(searchRequest10);
                    Thread.Sleep(3000);
                    if (searchResponse10.ResponseUri.AbsolutePath != "/LionAgentsOPS/CashPayment.aspx"
                        &&
                        (searchResponse10.StatusCode == HttpStatusCode.OK ||
                         searchResponse10.StatusCode == HttpStatusCode.Redirect))
                    {
                        TableLog.Log = "[Lion Air][GET] Response Uri is not /LionAgentsOPS/CashPayment.aspx or Status is not (OK Or Redirected). Booking ID: " + bookingId;
                        log.Post(TableLog.Log, "#logging-issueflight");
                        TableLog.Logging();
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[Lion Air]  || " + searchResponse10.Content }
                        };
                    }

                    //Confirmation
                    TableLog.Log = "[Lion Air] [POST] Confirmation Page. Url : LionAgentsOPS/Confirmation.aspx. Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    const string url11 = @"/LionAgentsOPS/Confirmation.aspx";
                    var searchRequest11 = new RestRequest(url11, Method.GET);
                    searchRequest11.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest11.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest11.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse11 = client.Execute(searchRequest11);
                    if (searchResponse11.ResponseUri.AbsolutePath != "/LionAgentsOPS/Confirmation.aspx"
                        &&
                        (searchResponse11.StatusCode == HttpStatusCode.OK ||
                         searchResponse11.StatusCode == HttpStatusCode.Redirect))
                    {
                        TableLog.Log = "[Lion Air] Response Uri is not /LionAgentsOPS/Confirmation.aspx or Status is not (OK Or Redirected).  \n*RESPONSE :*\n " + searchResponse11.Content + " Booking ID: " + bookingId;
                        log.Post(TableLog.Log, "#logging-issueflight");
                        TableLog.Logging();
                        return new IssueTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData },
                            ErrorMessages = new List<string> { "[Lion Air]  || " + searchResponse11.Content }
                        };
                    }
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
                    
                    LogOut(cid, client);
                    TurnInUsername(userName);

                    TableLog.Log = "[Lion Air] Done Issue. Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();

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
                    LogOut(cid, client);
                    TurnInUsername(userName);

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
