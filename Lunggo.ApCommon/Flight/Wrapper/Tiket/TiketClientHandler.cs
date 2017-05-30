using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.types;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        private partial class TiketClientHandler
        {
            private static readonly TiketClientHandler ClientInstance = new TiketClientHandler();
            private bool _isInitialized;
            private static string basePath;
            private static string sharedSecret;
            private static string token;


            private TiketClientHandler()
            {
                
            }

            internal static TiketClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    basePath = ConfigManager.GetInstance().GetConfigValue("tiket", "apiUrl");
                    sharedSecret = ConfigManager.GetInstance().GetConfigValue("tiket", "apiSecret");
                    _isInitialized = true;
                }
            }

            public static RestClient CreateTiketClient()
            {
                var client = new RestClient("http://api-sandbox.tiket.com");
                client.CookieContainer = new CookieContainer();
                return client;
            }

            public static void GetToken()
            {
                var client = CreateTiketClient();
                var url = "/apiv1/payexpress?method=getToken&secretkey=" + sharedSecret + "&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var responseToken = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                if (responseToken == null)
                    //Do something
                    token = null;
                token = responseToken.Token;
            }

            
        }
    }
}
