using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public class TiketClientHandler
    {
        private static string basePath;
        private static string sharedSecret;
        private static string confirmKey;

        public TiketClientHandler()
        {
            basePath = ConfigManager.GetInstance().GetConfigValue("tiket", "apiUrl");
            sharedSecret = ConfigManager.GetInstance().GetConfigValue("tiket", "apiSecret");
            confirmKey = ConfigManager.GetInstance().GetConfigValue("tiket", "apiConfirmKey");

        }

        public RestClient CreateTiketClient()
        {
            var client = new RestClient(basePath);
            client.CookieContainer = new CookieContainer();
            return client;
        }

        public string GetToken()
        {
            var client = CreateTiketClient();
            var url = "/apiv1/payexpress?method=getToken&secretkey=" + sharedSecret + "&output=json";
            var request = new RestRequest(url, Method.GET);
            var response = client.Execute(request);
            var responseToken = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (responseToken == null)
                //Do something
                return null;
            return responseToken.Token;
        }
    }

}
