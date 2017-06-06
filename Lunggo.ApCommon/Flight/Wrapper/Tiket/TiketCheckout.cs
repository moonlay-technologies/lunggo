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
        internal TiketBaseResponse CheckoutPage(string flightId, DateTime date)
        {
            return Client.CheckoutPage(flightId, date);
        }

        private partial class TiketClientHandler
        {
            internal TiketBaseResponse CheckoutPage(string flightId, DateTime date)
            {
                var client = CreateTiketClient();
                var url = "/order/checkout/20604252/IDR?token="+token+"&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var flightData = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                var temp = flightData;
                if (flightData.Diagnostic.Status != "200")
                    return null;
                return flightData;
            }
        }
    }

}
