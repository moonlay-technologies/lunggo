using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Web;
using RestSharp;
using Lunggo.ApCommon.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override decimal GetDeposit()
        {
            return Client.GetCurrentBalance();
        }
        private partial class AirAsiaClientHandler
        {
            public decimal GetCurrentBalance()
            {
                decimal balance = 0;
                var client = CreateAgentClient();
                if (!Login(client))
                    return balance;

                /*var homeAgentRequest = new RestRequest("AgentHome.aspx", Method.GET);
                homeAgentRequest.AddHeader("Referer", "https://booking2.airasia.com/LoginAgent.aspx");
                var homeAgentResponse = client.Execute(homeAgentRequest);*/

                var getDepositRequest = new RestRequest("UpdateProfileAgency.aspx", Method.GET);
                getDepositRequest.AddHeader("Referer", "https://booking2.airasia.com/AgentHome.aspx");
                var getDepositResponse = client.Execute(getDepositRequest);

                var html = getDepositResponse.Content;
                var searchedHtml = (CQ)html;
                var data = searchedHtml["#agencyForm > div"].FirstElement();
                var dummy = data.LastElementChild.InnerText;
                var stringDeposit = dummy.Split(new string[] { "IDR" }, StringSplitOptions.None);
                var deposit = stringDeposit[0].Trim().Replace(",", "");
                if (deposit != null || deposit != "")
                {
                    balance = decimal.Parse(deposit);
                }
                return balance;
            }

        }
        
    }
    
}
