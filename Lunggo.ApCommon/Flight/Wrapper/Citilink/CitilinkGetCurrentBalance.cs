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
using Lunggo.ApCommon.Dictionary;
using System.Collections.Generic;
using System;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        private partial class CitilinkClientHandler
        {
            public decimal GetCurrentBalance()
            {
                decimal balance = 0;
                var client = CreateAgentClient();
                Login(client);

                var getDepositRequest = new RestRequest("AccountBalance.aspx", Method.GET);
                getDepositRequest.AddHeader("Referer", "https://book.citilink.co.id/BookingListTravelAgent.aspx");
                var getDepositResponse = client.Execute(getDepositRequest);

                var html = getDepositResponse.Content;
                var searchedHtml = (CQ)html;
                var data = searchedHtml[".mainBody > div"].FirstElement().InnerText;
                var stringDeposit = data.Split(new string[] { "Rp." }, StringSplitOptions.None);
                var deposit = stringDeposit[1].Trim().Replace(",", "");
                if (deposit != null || deposit != "")
                {
                    balance = decimal.Parse(deposit);
                }
                return balance;
            }
        }
    }
}
