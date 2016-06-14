using System;
using System.Linq;
using CsQuery;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.Framework.Config;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        private partial class GarudaClientHandler
        {
            private IssueEnum IsIssued(string bookingId)
            {
                const int maxRetryCount = 10;
                var counter = 0;
                bool? isIssued = null;
                
                var clientx = CreateAgentClient();
                
                var cloudAppUrl = ConfigManager.GetInstance().GetConfigValue("general", "cloudAppUrl");
                var clienty = new RestClient(cloudAppUrl);
                var accReq = new RestRequest("/api/GarudaAccount/ChooseUserId", Method.GET);
                var userName = "";
                var reqTime = DateTime.UtcNow;

                var successLogin = false;
                var returnPath = "";

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

                //successLogin = Login(clientx, "SA3ALEU", "Travorama1234", out returnPath);

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
                        isIssued = null;
                    }

                    var password = userName == "SA3ALEU1" ? "Standar123" : "Travorama1234";
                    counter++;
                    successLogin = Login(clientx, userName, password, out returnPath);
                }

                if (counter >= 31)
                {
                    TurnInUsername(clientx, userName);
                    return IssueEnum.CheckingError;
                }

                //if (userId == null)
                //{
                //    return IssueEnum.CheckingError;
                //}


                // Page Welcome
                
                while (counter++ < maxRetryCount && isIssued == null)
                {
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

                    var htmlListTickets = (CQ)searchResAgent0.Content;

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

                    var htmlConfirmation = (CQ)searchResAgent0.Content;
                    var tableConf = htmlConfirmation[".inline"];
                    var confirmed = tableConf[0].ChildElements.ToList()[7].InnerText;
                    isIssued = confirmed == "TKT";

                    LogOut(returnPath, clientx);
                    TurnInUsername(clienty, userName);
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
