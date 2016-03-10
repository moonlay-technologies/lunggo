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
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        private partial class LionAirClientHandler
        {
            private IssueEnum IsIssued(string bookingId)
            {
                const int maxRetryCount = 10;
                var counter = 0;
                var isIssued = (bool?) null;
                //string currentDeposit;
                var clientx = CreateAgentClient();
                clientx.FollowRedirects = false;
                CQ searchedHtml;
                string userId = "";
                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clienty = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/LionAirAccount/ChooseUserId", Method.GET);
                var userName = "";
                var accRs = new RestResponse();
                var reqTime = DateTime.UtcNow;
                var ctr = 0;
                var msgLogin = "Your login name is inuse";
                while (msgLogin == "Your login name is inuse" || msgLogin == "There was an error logging you in" || ctr >= 21)
                {
                    while (DateTime.UtcNow <= reqTime.AddMinutes(10) && userName.Length == 0)
                    {
                        accRs = (RestResponse) clienty.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }


                    if (userName.Length == 0)
                        isIssued = null;
                    //string msgLogin;

                    bool successLogin ;
                    do
                    {
                        clientx.BaseUrl = new Uri("https://agent.lionair.co.id");
                        const string url0 = @"/lionairagentsportal/default.aspx";
                        var searchRequest0 = new RestRequest(url0, Method.GET);
                        var searchResponse0 = clientx.Execute(searchRequest0);
                        var html0 = searchResponse0.Content;
                        searchedHtml = (CQ) html0;
                        var viewstate = HttpUtility.UrlEncode(searchedHtml["#__VIEWSTATE"].Attr("value"));
                        var eventval = HttpUtility.UrlEncode(searchedHtml["#__EVENTVALIDATION"].Attr("value"));
                        FlightService.ParseCabinClass(CabinClass.Economy);
                        if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                            (searchResponse0.StatusCode == HttpStatusCode.OK ||
                             searchResponse0.StatusCode == HttpStatusCode.Redirect))
                            return IssueEnum.CheckingError;
                        const string url1 = @"/lionairagentsportal/CaptchaGenerator.aspx";
                        var searchRequest1 = new RestRequest(url1, Method.GET);
                        var searchResponse1 = clientx.Execute(searchRequest1);
                        successLogin = Login(clientx, searchResponse1.RawBytes, viewstate, eventval, out userId,
                            userName, out msgLogin); //, out currentDeposit);
                        ctr++;
                    } while (!successLogin && (msgLogin != "Your login name is inuse"
                        && msgLogin != "There was an error logging you in"));
                }

                //if (ctr >= 21)
                //{
                //    return IssueEnum.CheckingError;
                //}

                if (userId == null)
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
                    var url3 = @"/LionAgentsOPS/TicketingQueue.aspx?consID=" + cid;
                    var searchRequest3 = new RestRequest(url3, Method.GET);
                    searchRequest3.AddHeader("Referer",
                       "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?consID=" + cid);
                    searchRequest3.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest3.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    Thread.Sleep(2000);
                    var searchResponse3 = clientx.Execute(searchRequest3);
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
                    var beginning = "__EVENTTARGET=lbSearch&__EVENTARGUMENT=&__VIEWSTATE=";
                    var ending = "&txtBookingReloc=" + bookingId + "&txtPassengerName=&ddlDateSelection=";
                    var postData4 = beginning + vs + ending;
                    searchRequest4.AddParameter("application/x-www-form-urlencoded", postData4, ParameterType.RequestBody);
                    Thread.Sleep(2000);
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
                    Thread.Sleep(2000);
                    var confirmationContent = searchResponse5.Content;
                    var bookingListCq = (CQ)confirmationContent;
                    var theTable = bookingListCq[".Step4ItinRow"];
                    var viewstateX = HttpUtility.UrlEncode(bookingListCq["#__VIEWSTATE"].Attr("value"));
                    //isIssued = theTable.Children().ToList()[7].InnerText == "Confirmed";
                    
                    var isIssuedy = theTable.Children().ToList()[7].InnerText == "Confirmed";
                    /*foreach (var row in theTable)
                    {
                        isIssuedy = theTable.Children()
                        isIssuedy &= row.Children().ToList()[7].InnerText == "Confirmed";
                    }*/

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
                    var searchResponse6 = clientx.Execute(searchRequest6);

                    const string url7 = @"/LionAgentsOPS/TicketQueue.aspx";
                    var searchRequest7 = new RestRequest(url7, Method.GET);
                    searchRequest7.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAgentsOPS/TicketBooking.aspx?BookingReloc=" + bookingId);
                    searchRequest7.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest7.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    Thread.Sleep(2000);
                    var searchResponse7 = clientx.Execute(searchRequest7);
                    
                    const string url15 = @"/LionAirAgentsPortal/Logout.aspx";
                    var searchRequest15 = new RestRequest(url15, Method.GET);
                    searchRequest15.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                    searchRequest15.AddHeader("Content-Encoding", "gzip");
                    searchRequest15.AddHeader("Host", "agent.lionair.co.id");
                    searchRequest15.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    searchRequest15.AddHeader("Referer",
                        "https://agent.lionair.co.id/LionAirAgentsPortal/Agents/Welcome.aspx?" + cid);
                    Thread.Sleep(2000);
                    var searchResponse15 = clientx.Execute(searchRequest15);

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
                    Thread.Sleep(2000);
                    var searchResponse16 = clientx.Execute(searchRequest16);

                    accReq = new RestRequest("/api/LionAirAccount/LogOut?userId=" + userName, Method.GET);
                    accRs = (RestResponse) clienty.Execute(accReq);
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
