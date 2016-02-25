using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId, bool canHold)
        {
            return Client.OrderTicket(bookingId);
        }

        private partial class LionAirClientHandler
        {
            internal OrderTicketResult OrderTicket(string bookingId)
            {
                var clientx = CreateAgentClient();
                clientx.FollowRedirects = false;
                var searchedHtml = new CQ();
                bool successLogin = false;
                string userId;
                string currentDeposit;
                do
                {
                    clientx.BaseUrl = new Uri("https://agent.lionair.co.id");
                    var url0 = @"/lionairagentsportal/default.aspx";
                    var searchRequest0 = new RestRequest(url0, Method.GET);
                    var searchResponse0 = clientx.Execute(searchRequest0);
                    var html0 = searchResponse0.Content;
                    searchedHtml = (CQ)html0;
                    var viewstate = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));
                    var eventval = HttpUtility.UrlEncode(searchedHtml["#__EVENTVALIDATION"].Attr("value"));
                    FlightService.ParseCabinClass(CabinClass.Economy);
                    if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                        (searchResponse0.StatusCode == HttpStatusCode.OK ||
                            searchResponse0.StatusCode == HttpStatusCode.Redirect))
                        return new OrderTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.InvalidInputData }
                        };
                    var url1 = @"/lionairagentsportal/CaptchaGenerator.aspx";
                    var searchRequest1 = new RestRequest(url1, Method.GET);
                    var searchResponse1 = clientx.Execute(searchRequest1);
                    successLogin = Login(clientx, searchResponse1.RawBytes, viewstate, eventval, out userId);// out currentDeposit);
                } while (!successLogin);

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
                searchedHtml = (CQ)html3;
                var vs = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));

                //Post data booking id
                var searchRequest4 = new RestRequest(url3, Method.POST);
                searchRequest4.AddHeader("Referer",
                    "https://agent.lionair.co.id/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid);
                searchRequest4.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                searchRequest4.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                var beginning = "__EVENTTARGET=lbSearch&__EVENTARGUMENT=&__VIEWSTATE=";
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
                searchedHtml = (CQ)html8;
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
                    var endingToPay =
                        "&payDet=rbPay_CA&CreditCardDisplay1%24CreditCardType=VI&CreditCardDisplay1%24txtCardHolderName=&CreditCardDisplay1%24CreditCardNumber=&CreditCardDisplay1%24CreditCardExpiryMonth=MM&CreditCardDisplay1%24CreditCardExpiryYear=YY&CreditCardDisplay1%24CVVNumber=&FlightInfo=&AXTotal=&DCTotal=&OtherTotal=&nameMismatch=";
                    var postData9 = beginningToPay + endingToPay;
                    searchRequest9.AddParameter("application/x-www-form-urlencoded", postData9, ParameterType.RequestBody);
                    var searchResponse9 = clientx.Execute(searchRequest9);
                    //var Y = searchResponse9.ResponseUri.AbsolutePath;
                    Thread.Sleep(3000);
                    if (searchResponse9.ResponseUri.AbsolutePath != "/LionAgentsOPS/TicketBooking.aspx"
                        && (searchResponse9.StatusCode == HttpStatusCode.OK || searchResponse9.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();

                    //CashPayment

                    var url10 = @"/LionAgentsOPS/CashPayment.aspx";
                    var searchRequest10 = new RestRequest(url10, Method.GET);
                    searchRequest10.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest10.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest10.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse10 = clientx.Execute(searchRequest10);
                   // var z = searchResponse10.ResponseUri.AbsolutePath;
                    Thread.Sleep(3000);
                    if (searchResponse10.ResponseUri.AbsolutePath != "/LionAgentsOPS/CashPayment.aspx"
                        && (searchResponse10.StatusCode == HttpStatusCode.OK || searchResponse10.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();

                    //Confirmation

                    var url11 = @"/LionAgentsOPS/Confirmation.aspx";
                    var searchRequest11 = new RestRequest(url11, Method.GET);
                    searchRequest11.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest11.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest11.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse11 = clientx.Execute(searchRequest11);
                    //var abc = searchResponse11.ResponseUri.AbsolutePath;
                    if (searchResponse11.ResponseUri.AbsolutePath != "/LionAgentsOPS/Confirmation.aspx"
                        && (searchResponse11.StatusCode == HttpStatusCode.OK || searchResponse11.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();
                    Thread.Sleep(3000);
                    var confirmationContent = searchResponse11.Content;
                    var html11 = (CQ) confirmationContent;
                    var bookingRef = html11["#lblRefNumber"].Text();

                    //Logout
                    var url15 = @"/LionAirAgentsPortal/Logout.aspx";
                    var searchRequest15 = new RestRequest(url15, Method.GET);
                    searchRequest15.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest15.AddHeader("Content-Encoding", "gzip");
                    searchRequest15.AddHeader("Host", "agent.lionair.co.id");
                    searchRequest15.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchRequest15.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?" + cid);
                    Thread.Sleep(3000);
                    var searchResponse15 = clientx.Execute(searchRequest15);

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
                    Thread.Sleep(3000);
                    var searchResponse16 = clientx.Execute(searchRequest16);
                    
                    return new OrderTicketResult
                    {
                        IsSuccess = true,
                        BookingId = bookingRef
                    };
                }
                catch
                {
                    var isIssued = IsIssued(bookingId);
                    switch (isIssued)
                    {
                        case IssueEnum.IssueSuccess:
                            return new OrderTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId,
                                IsInstantIssuance = true
                            };
                        case IssueEnum.NotIssued:
                            return new OrderTicketResult
                            {
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            return new OrderTicketResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            return new OrderTicketResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                    }
                }
            }
        }
    }
}
