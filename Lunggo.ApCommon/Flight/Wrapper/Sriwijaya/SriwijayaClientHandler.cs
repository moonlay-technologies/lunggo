using System.Net;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        private partial class SriwijayaClientHandler
        {
            private static readonly SriwijayaClientHandler ClientInstance = new SriwijayaClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;

            private SriwijayaClientHandler()
            {

            }

            //
            internal static SriwijayaClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            //
            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = "MLWAG02152";
                    _password = "Dev12345";
                    _isInitialized = true;
                }
            }

            private static RestClient CreateCustomerClient()
            {
                var client = new RestClient("https://www.sriwijayaair.co.id");
                client.AddDefaultHeader("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Referer", "https://www.sriwijayaair.co.id/welcome.php");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                return client;
            }

            private static RestClient CreateAgentClient()
            {
                var client = new RestClient("http://agent.sriwijayaair.co.id");
                client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");
                client.AddDefaultHeader("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Host", "agent.sriwijayaair.co.id");
                client.AddDefaultHeader("Origin", "https://www.sriwijayaair.co.id");
                client.AddDefaultHeader("Referer", "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                return client;
            }

            internal static void Login(RestClient client)
            {
                var url = "SJ-Eticket/login.php?action=in";
                var postData =
                    "username=" + _userName +
                    "&password=" + _password +
                    "&Submit=Log+In" +
                    "&actions=LOGIN";
                var request = new RestRequest(url, Method.POST);
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var response = client.Execute(request);
                var temp = response.ResponseUri.AbsoluteUri.Contains("/SJ-Eticket/application/index.php");
                var loginResult = response.Content;
            }
            internal static void Logout(RestClient client)
            {
                var url = "SJ-Eticket/login.php?action=out";
                var request = new RestRequest(url, Method.GET);
                client.Execute(request);
            }
        }
    }
}

