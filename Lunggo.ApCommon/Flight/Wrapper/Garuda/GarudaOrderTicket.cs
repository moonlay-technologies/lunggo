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

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId, bool canHold)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            //if (env == "production")
                return Client.OrderTicket(bookingId);
            //else
            //    return new OrderTicketResult
            //    {
            //        IsSuccess = true,
            //        BookingId = bookingId,
            //        IsInstantIssuance = true
            //    };
        }

        private partial class GarudaClientHandler
        {
            internal OrderTicketResult OrderTicket(string bookingId)
            {
                var clientx = CreateAgentClient();
                CQ searchedHtml;
                var userId = "";
                
                // [GET] Search Flight
                clientx.BaseUrl = new Uri("https://gosga.garuda-indonesia.com");
                string urlweb = @"";
                var searchReqAgent = new RestRequest(urlweb, Method.GET);
                searchReqAgent.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent.AddHeader("Host", "gosga.garuda-indonesia.com");
                var searchResAgent = clientx.Execute(searchReqAgent);

                urlweb = @"web/user/login/id";
                var searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                var searchResAgent0 = clientx.Execute(searchReqAgent0);

                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clienty = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/GarudaAccount/ChooseUserId", Method.GET);
                var userName = "";
                var reqTime = DateTime.UtcNow;
                var newitin = new FlightItinerary();
                var successLogin = false;
                var counter = 0;
                string returnpath = "";

                //successLogin = Login(clientx, "SA3ALEU1", "Standar123", out returnpath);
                while (!successLogin && counter < 31 && returnpath != "/web/dashboard/welcome")
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && (userName.Length == 0))
                    //|| returnpath != "/web/dashboard/welcome")
                    {
                        
                        var accRs = (RestResponse)clienty.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }

                    if (userName.Length == 0)
                    {
                        return new OrderTicketResult
                        {
                            Errors = new List<FlightError> { FlightError.TechnicalError },
                            ErrorMessages = new List<string> { "All usernames are used" }
                        };
                    }

                    var password = userName == "SA3ALEU1" ? "Standar123" : "Travorama1234";
                    counter++;
                    successLogin = Login(clientx, userName, password, out returnpath);
                }

                if (counter >= 31)
                {
                    TurnInUsername(clientx, userName);
                    return new OrderTicketResult
                    {

                        Errors = new List<FlightError> { FlightError.InvalidInputData },
                        ErrorMessages = new List<string> { "Can't get id" }
                    };
                }

                urlweb = @"web/order/ticket";
                searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/dashboard/welcome");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchResAgent0 = clientx.Execute(searchReqAgent0);
                string returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/ticket");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");

                var postdata =
                    "selectTransactionType=noneretail" +
                    "&selectTransactionConnector=live" +
                    "&PageInput%5Bstatus%5D=ALL" +
                    "&PageInput%5Btrx_date_start%5D=" +
                    "&PageInput%5Btrx_date_end%5D=" +
                    "&PageInput%5Bkeyword%5D=" + bookingId +
                    "&btnQuery=&PageInput%5Bpage%5D=1&PageInput%5BgoToPage%5D=&PageInput%5Blimit%5D=10&PageInput%5Bsortby%5D=";

                searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                searchResAgent0 = clientx.Execute(searchReqAgent0);
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;
                var htmlListTickets = (CQ) searchResAgent0.Content;

                var tableToTicket = htmlListTickets["#table2"];
                var linktoTicket = tableToTicket[0].ChildElements.ToList()[1].
                    ChildElements.ToList()[0].ChildElements.ToList()[3].ChildElements.ToList()[0].GetAttribute("href");

                linktoTicket = linktoTicket.SubstringBetween(linktoTicket.Length - 40, linktoTicket.Length);

                urlweb = @"" + linktoTicket;
                searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/ticket/live");
                searchReqAgent0.AddHeader("Accept",
                    "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                searchResAgent0 = clientx.Execute(searchReqAgent0);
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;


                LogOut(returnPath, clientx);
                TurnInUsername(clienty, userName);

                var url5 = "";
                var vsPostToPay = "";
                var cid = "";

                try
                {
                    //GO TO PAY, POST

                    var searchRequest9 = new RestRequest(url5, Method.POST);
                    searchRequest9.AddHeader("Referer",
                        "https://agent.Garuda.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
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
                        throw new Exception();

                    //CashPayment

                    const string url10 = @"/LionAgentsOPS/CashPayment.aspx";
                    var searchRequest10 = new RestRequest(url10, Method.GET);
                    searchRequest10.AddHeader("Referer",
                        "https://agent.Garuda.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest10.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest10.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse10 = clientx.Execute(searchRequest10);
                    Thread.Sleep(3000);
                    if (searchResponse10.ResponseUri.AbsolutePath != "/LionAgentsOPS/CashPayment.aspx"
                        && (searchResponse10.StatusCode == HttpStatusCode.OK || searchResponse10.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();

                    //Confirmation

                    const string url11 = @"/LionAgentsOPS/Confirmation.aspx";
                    var searchRequest11 = new RestRequest(url11, Method.GET);
                    searchRequest11.AddHeader("Referer",
                        "https://agent.Garuda.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest11.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest11.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    var searchResponse11 = clientx.Execute(searchRequest11);
                    if (searchResponse11.ResponseUri.AbsolutePath != "/LionAgentsOPS/Confirmation.aspx"
                        && (searchResponse11.StatusCode == HttpStatusCode.OK || searchResponse11.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();
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
                    return new OrderTicketResult
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
                            return new OrderTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId,
                                IsInstantIssuance = true
                            };
                        case IssueEnum.NotIssued:
                            return new OrderTicketResult
                            {
                                IsSuccess = false,
                                CurrentBalance = GetCurrentBalance()
                            };
                        case IssueEnum.CheckingError:
                            return new OrderTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                                
                            };
                        default:
                            return new OrderTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
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
