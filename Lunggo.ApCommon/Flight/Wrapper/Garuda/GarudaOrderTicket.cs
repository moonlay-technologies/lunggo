using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
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
                
                // [GET] Search Flight
                clientx.BaseUrl = new Uri("https://gosga.garuda-indonesia.com");
                var urlweb = @"";
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
                var successLogin = false;
                var counter = 0;
                var returnPath = "";

                //successLogin = Login(clientx, "SA3ALEU1", "Standar123", out returnpath);
                while (!successLogin && counter < 31 && returnPath != "/web/dashboard/welcome")
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
                    successLogin = Login(clientx, userName, password, out returnPath);
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
                returnPath = searchResAgent0.ResponseUri.AbsolutePath;

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
                    "&btnQuery=&PageInput%5Bpage%5D=1" +
                    "&PageInput%5BgoToPage%5D=" +
                    "&PageInput%5Blimit%5D=10" +
                    "&PageInput%5Bsortby%5D=";

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

                //LogOut(returnPath, clientx);
                //TurnInUsername(clienty, userName);

                try
                {
                    //First, GET
                    var splitted = linktoTicket.Split('/');
                    urlweb = @"/web/order/purchasedtuinit/" + splitted[3];
                    searchReqAgent0 = new RestRequest(urlweb, Method.GET);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/ticketDetail/"+ splitted[3] + "/live");
                    //var a = "https://gosga.garuda-indonesia.com/web/order/ticketDetail/7TL2M8160609/live";
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, sdch, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchResAgent0 = clientx.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;
                    var infopage = (CQ) searchResAgent0.Content;

                    var info = infopage["#myForm"];

                    var infodata = info[0].ChildElements.ToList();
                    var invoiceno = infodata[0].GetAttribute("value");
                    var amount = infodata[1].GetAttribute("value");
                    var tourcode = infodata[2].GetAttribute("value");
                    var info1 = infodata[3].GetAttribute("value");
                    var words = infodata[4].GetAttribute("value");
                    //HttpUtility.UrlEncode(

                    //Second, POST

                    urlweb = @"GarudaAgentTrans/Payment";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/purchasedtuinit/" + splitted[3]);
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "deposit.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://deposit.garuda-indonesia.com");
                    postdata =
                        "INVOICENO=" + HttpUtility.UrlEncode(invoiceno)+
                        "&AMOUNT=" + HttpUtility.UrlEncode(amount) +
                        "&TOURCODE=" + HttpUtility.UrlEncode(tourcode) +
                        "&INFO=" + HttpUtility.UrlEncode(info1) +
                        "&WORDS=" + HttpUtility.UrlEncode(words);

                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = clientx.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    //Third, POST
                    urlweb = @"GarudaAgentTrans/Payment";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://deposit.garuda-indonesia.com/GarudaAgentTrans/Payment");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "deposit.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://deposit.garuda-indonesia.com");
                    postdata ="tpin=123456&GosPay=Next";

                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = clientx.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;
                    var refPage = (CQ) searchResAgent0.Content;

                    var refr = refPage[".wrapper-input"];
                    var refcode = refr[0].ChildElements.ToList()[3].InnerText;

                    //Fourth, Post
                    urlweb = @"web/order/ticketDetail/" + splitted[3] + "/live";
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", 
                        "https://gosga.garuda-indonesia.com/web/order/ticketDetail/" + splitted[3] + "/live");
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                    postdata = "Inputs%5Bdturefno%5D=" + refcode +
                               "&btnDtuSubmit=++++++++++++++++++++++++Submit++++++++++++++++++++++++";

                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = clientx.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    //Fifth, post

                    urlweb = @"web/order/ticketDetail/" + splitted[3] ;
                    searchReqAgent0 = new RestRequest(urlweb, Method.POST);
                    searchReqAgent0.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/order/ticketDetail/" + splitted[3]);
                    searchReqAgent0.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchReqAgent0.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    searchReqAgent0.AddHeader("Host", "gosga.garuda-indonesia.com");
                    searchReqAgent0.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                    postdata = "btnGetTix=Get+Ticket";

                    searchReqAgent0.AddParameter("application/x-www-form-urlencoded", postdata, ParameterType.RequestBody);
                    searchResAgent0 = clientx.Execute(searchReqAgent0);
                    returnPath = searchResAgent0.ResponseUri.AbsolutePath;

                    var htmlConfirmation = (CQ) searchResAgent0.Content;
                    var tableConf = htmlConfirmation[".inline"];

                    var confirmed = tableConf[0].ChildElements.ToList()[7].InnerText;

                    var isIssued = confirmed == "TKT";
                    var bookingRef = tableConf[0].ChildElements.ToList()[3].ChildElements.ToList()[0].InnerText;

                    LogOut(returnPath, clientx);
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
                    LogOut(returnPath, clientx);
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
