using System.Net;
using System.Security.Cryptography;
using CsQuery;
using Lunggo.Framework.Config;
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
                    _userName = ConfigManager.GetInstance().GetConfigValue("Garuda", "webUserName");
                    _password = ConfigManager.GetInstance().GetConfigValue("Garuda", "webPassword");
                    _isInitialized = true;
                }
            }

            private static RestClient CreateCustomerClient()
            {
                var client = new RestClient("https://www.garuda-indonesia.com");
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                client.CookieContainer = new CookieContainer();
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
                //client.FollowRedirects = false;
                return client;
            }

            private bool Login(RestClient client, string username, string password, out string returnpath)
            {

                var url = "web/user/login";
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

            private void TurnInId(RestClient client, string username)
            {
                var accReq = new RestRequest("/api/GarudaAccount/LogOut?userId=" + username, Method.GET);
                var accRs = (RestResponse)client.Execute(accReq);
            }
           
            //Get Deposit for Lion Air
            private static decimal getDeposit() 
            {
                return currentDeposit;
            }
        }
    }
}

