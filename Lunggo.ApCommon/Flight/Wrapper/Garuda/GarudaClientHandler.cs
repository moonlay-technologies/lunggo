using System.Net;
using Lunggo.Framework.Environment;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper
    {
        private partial class GarudaClientHandler
        {
            private static readonly GarudaClientHandler ClientInstance = new GarudaClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;
            private static decimal currentDeposit;

            private GarudaClientHandler()
            {
            
            }

            internal static GarudaClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = EnvVariables.Get("Garuda", "webUserName");
                    _password = EnvVariables.Get("Garuda", "webPassword");
                    _isInitialized = true;
                }
            }

            private static RestClient CreateCustomerClient()
            {
                var client = new RestClient("https://www.garuda-indonesia.com");
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                client.Proxy = new WebProxy("zproxy.luminati.io:22225");
                client.Proxy.Credentials = new NetworkCredential("lum-customer-travelmadezy-zone-gen-country-id", "9f0760f20d0e");
                client.FollowRedirects = false;

                return client;
            }

            private static RestClient CreateAgentClient()
            {
                var client = new RestClient("https://gosga.garuda-indonesia.com");
                
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                //client.Proxy = new WebProxy("185.77.167.128", 60000)
                //{
                //    Credentials = new NetworkCredential("travelmadezy", "9T8XCty9MT")
                //};
                client.Proxy = new WebProxy("zproxy.luminati.io:22225");
                client.Proxy.Credentials = new NetworkCredential("lum-customer-travelmadezy-zone-gen-country-id", "9f0760f20d0e");
                return client;
            }

            private bool Login(RestClient client, string username, string password, out string returnpath)
            {

                const string url = "web/user/login";
                var request = new RestRequest(url, Method.POST);
                var postData =
                    @"Inputs%5BbookNow%5D=" +
                    @"&Inputs%5Busername%5D=" + username +
                    @"&Inputs%5Bpassword%5D=" + password +
                    @"&Login=" + "Sign+In" ;
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                request.AddHeader("Accept", "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                request.AddHeader("Host", "gosga.garuda-indonesia.com");
                request.AddHeader("Origin", "https://gosga.garuda-indonesia.com");
                request.AddHeader("Referer", "https://gosga.garuda-indonesia.com/web/user/login/id");
                request.AddHeader("Cache-Control", "max-age=0");
                var response = client.Execute(request);
                returnpath = response.ResponseUri.AbsolutePath;

                return response.ResponseUri.AbsolutePath == "/web/dashboard/welcome";             
            }
        }
    }
}

