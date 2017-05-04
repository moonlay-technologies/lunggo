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
using System.Collections.Generic;
using System;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override decimal GetDeposit()
        {
            return Client.GetCurrentBalance();
        }
        private partial class SriwijayaClientHandler
        {
            public decimal GetCurrentBalance()
            {
                decimal balance = 0;
                var client = CreateAgentClient();
                Login(client);

                var url = "SJ-Eticket/login.php?action=in";
                var postData =
                    "username=" + _userName +
                    "&PassLogin=" + _password +
                    "&Submit=Log+In" +
                    "&actions=LOGIN";
                var request = new RestRequest(url, Method.POST);
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var response = client.Execute(request);
                var temp = response.ResponseUri.AbsoluteUri.Contains("/SJ-Eticket/application/index.php");
                var html = response.Content;

                var searchedHtml = (CQ)html;
                var data = searchedHtml[".userDeposit"].Text();
                if (string.IsNullOrEmpty(data))
                    return balance;
                var stringDeposit = data.Split(':');
                if (stringDeposit.Length < 1)
                    return balance;
                var deposit = stringDeposit[1].Replace("IDR", "").Replace(",", "").Trim();
                if (deposit != null || deposit != "")
                {
                    balance = decimal.Parse(deposit);
                }

                Logout(client);
                return balance;
            }

        }
    }
}
