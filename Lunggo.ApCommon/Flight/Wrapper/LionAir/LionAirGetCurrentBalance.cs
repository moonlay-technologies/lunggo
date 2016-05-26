using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using RestSharp;
using System.Globalization;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper
    {
        private partial class LionAirClientHandler
        {
            public decimal GetCurrentBalance()
            {
                var clientx = CreateAgentClient();
                clientx.FollowRedirects = false;
                CQ searchedHtml;
                var userId = "";
                decimal balance = 0;

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
                        accRs = (RestResponse)clienty.Execute(accReq);
                        userName = accRs.Content.Trim('"');
                    }

                    if (userName.Length == 0)
                        return balance;

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
                        FlightService.ParseCabinClass(CabinClass.Economy);
                        if (searchResponse0.ResponseUri.AbsolutePath != "/lionairagentsportal/default.aspx" &&
                            (searchResponse0.StatusCode == HttpStatusCode.OK ||
                             searchResponse0.StatusCode == HttpStatusCode.Redirect)) 
                        {
                            return balance;
                        }
                        const string url1 = @"/lionairagentsportal/CaptchaGenerator.aspx";
                        var searchRequest1 = new RestRequest(url1, Method.GET);
                        var searchResponse1 = clientx.Execute(searchRequest1);
                        successLogin = Login(clientx, searchResponse1.RawBytes, viewstate, eventval, out userId,
                            userName, out msgLogin, out currentDeposit);
                    } while (!successLogin && (msgLogin != "Your login name is inuse"
                        && msgLogin != "There was an error logging you in"));
                }
                if (currentDeposit != null || currentDeposit != "") 
                {
                    balance = decimal.Parse(currentDeposit);
                }
                return balance;
            }
        }
    }
}
