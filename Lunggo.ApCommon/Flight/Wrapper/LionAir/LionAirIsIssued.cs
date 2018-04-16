using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using RestSharp;
using Lunggo.ApCommon.Log;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        private partial class LionAirClientHandler
        {
            private IssueEnum IsIssued(string bookingId)
            {
                GlobalLog TableLog = new GlobalLog();
                var log = LogService.GetInstance();


                TableLog.PartitionKey = "FLIGHT ISSUE LOG";
                const int maxRetryCount = 10;
                var counter = 0;
                bool? isIssued = null;
                //string currentDeposit;
                var client = CreateAgentClient();
                client.FollowRedirects = false;
                CQ searchedHtml;
                string userId;
                string userName;
                string errorMessage;
                TableLog.Log = "[Lion Air] IsIssued: Before Login. Booking ID: " + bookingId;
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();
                var succeedLogin = Login(client, out userName, out userId, out errorMessage, 24*3600);
                if (!succeedLogin)
                {
                    return IssueEnum.CheckingError;
                }

                // Page Welcome
                var startind = userId.IndexOf("consID");
                var cid = userId.SubstringBetween(startind, userId.Length);
                cid = cid.SubstringBetween(7, cid.Length);
                
                while (counter++ < maxRetryCount && isIssued == null)
                {
                    //Page Masukin Booking Id
                    TableLog.Log = "[Lion Air] IsIssued: Inserting Booking Id. Url : /LionAgentsOPS/TicketingQueue.aspx?consID=" + cid + " Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    var url3 = @"/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid;
                    var searchRequest3 = new RestRequest(url3, Method.GET);
                    searchRequest3.AddHeader("Referer",
                       "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?consID=" + cid);
                    searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest3.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    Thread.Sleep(2000);
                    var searchResponse3 = client.Execute(searchRequest3);
                    var html3 = searchResponse3.Content;
                    searchedHtml = html3;
                    var vs = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));

                    //Post data booking id
                    TableLog.Log = "[Lion Air] IsIssued: Post Booking Id. Url : " + url3;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
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
                    Thread.Sleep(2000);
                    var searchResponse4 = client.Execute(searchRequest4);

                    //Page Tampilin Reservasi
                    TableLog.Log = "[Lion Air] IsIssued: Page Show Reservation. Url : /LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId + " Booking ID: " + bookingId;
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
                    Thread.Sleep(2000);
                    var confirmationContent = searchResponse5.Content;
                    var bookingListCq = (CQ)confirmationContent;
                    var theTable = bookingListCq[".Step4ItinRow"];
                    var viewstateX = HttpUtility.UrlEncode(bookingListCq["#__VIEWSTATE"].Attr("value"));
                    if (theTable.IsNullOrEmpty())
                        isIssued = false;
                    else if (theTable.Children().ToList()[7].InnerText.Length == 0)
                        isIssued = false;
                    else
                    {
                        isIssued = theTable.Children().ToList()[7].InnerText == "Confirmed";
                    }

                    TableLog.Log = "[Lion Air] IsIssued: Post Page Show Reservation. Url : /LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId + " Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    var url6 = @"/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId;
                    var searchRequest6 = new RestRequest(url6, Method.POST);
                    searchRequest6.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest6.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest6.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    beginning = "__EVENTTARGET=lbGoBack&__EVENTARGUMENT=&__VIEWSTATE=%" + viewstateX;
                    ending =
                        "&payDet=rbPay_CA&CreditCardDisplay1%24CreditCardType=VI&CreditCardDisplay1%24txtCardHolderName=&CreditCardDisplay1%24CreditCardNumber=&CreditCardDisplay1%24CreditCardExpiryMonth=MM&CreditCardDisplay1%24CreditCardExpiryYear=YY&CreditCardDisplay1%24CVVNumber=&FlightInfo=&AXTotal=&DCTotal=&OtherTotal=&nameMismatch=";
                    searchRequest6.AddParameter("application/x-www-form-urlencoded", beginning+ending, ParameterType.RequestBody);
                    Thread.Sleep(2000);
                    var searchResponse6 = client.Execute(searchRequest6);

                    TableLog.Log = "[Lion Air] IsIssued: Show Ticketing Queue. Url : /LionAgentsOPS/TicketQueue.aspx Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    const string url7 = @"/LionAgentsOPS/TicketQueue.aspx";
                    var searchRequest7 = new RestRequest(url7, Method.GET);
                    searchRequest7.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest7.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest7.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    Thread.Sleep(2000);
                    var searchResponse7 = client.Execute(searchRequest7);

                    LogOut(cid, client);
                    TurnInUsername(userName);
                    TableLog.Log = "[Lion Air] IsIssued: Done. Booking ID: " + bookingId;
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                }
                switch (isIssued)
                {
                    case null:
                        return IssueEnum.CheckingError;
                    case true:
                        return IssueEnum.IssueSuccess;
                    case false:
                        return IssueEnum.NotIssued;
                    default:
                        return IssueEnum.CheckingError;
                }
            }

            private enum IssueEnum
            {
                IssueSuccess,
                NotIssued,
                CheckingError
            }

        }
    }
}
