using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using CsQuery;
using CsQuery.StringScanner.ExtensionMethods;
using Lunggo.ApCommon.Constant;
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
                var client = CreateAgentClient();
                client.FollowRedirects = false;
                string userName;
                var userId = "";
                string errorMessage;
                decimal balance = 0;
                string currentDeposit;

                var succeedLogin = Login(client, out userName, out userId, out errorMessage, out currentDeposit);
                if (!succeedLogin)
                {
                    return balance;
                }

                if (!string.IsNullOrEmpty(currentDeposit)) 
                {
                    balance = decimal.Parse(currentDeposit);
                }
                return balance;
            }
        }
    }
}
