using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override decimal GetDeposit()
        {
            return Client.GetCurrentBalance();
        }

        private partial class TiketClientHandler
        {
            internal decimal GetCurrentBalance()
            {
                //http://api-sandbox.tiket.com/partner/transactionApi/get_saldo?secretkey=[SECRET_KEY]&confirmkey=[CONFIRM_KEY]&username=[USERNAME]
                var client = CreateTiketClient();
                var url = "/partner/transactionApi/get_saldo";
                var request = new RestRequest(url, Method.GET);
                request.AddQueryParameter("secretkey", _sharedSecret);
                request.AddQueryParameter("confirmkey", _confirmKey);
                request.AddQueryParameter("username", "suheri@travelmadezy.com");
                request.AddQueryParameter("output", "json");
                var response = client.Execute(request);
                var checkBalanceResponse = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                var temp = checkBalanceResponse;
                if (checkBalanceResponse.Diagnostic.Status != "200")
                    return 0;
                return checkBalanceResponse.Deposit;
            }
        }
    }

}
