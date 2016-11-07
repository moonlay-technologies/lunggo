using System.Net;
using Lunggo.Framework.Config;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        private partial class AirAsiaClientHandler
        {
            private static readonly AirAsiaClientHandler ClientInstance = new AirAsiaClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;

            private AirAsiaClientHandler()
            {
            
            }

            internal static AirAsiaClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = ConfigManager.GetInstance().GetConfigValue("airAsia", "webUserName");
                    _password = ConfigManager.GetInstance().GetConfigValue("airAsia", "webPassword");
                    _isInitialized = true;
                }
            }

            private static RestClient CreateCustomerClient()
            {
                var client = new RestClient("http://booking.airasia.com");
                client.AddDefaultHeader("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Origin", "https://booking.airasia.com");
                //client.AddDefaultHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                client.Proxy = new WebProxy("128.199.91.32", 80)
                {
                    Credentials = new NetworkCredential("travorama", "tmi12345")
                };
                client.CookieContainer = new CookieContainer();
                return client;
            }

            private static RestClient CreateAgentClient()
            {
                var client = new RestClient("https://booking2.airasia.com");
                //client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");
                client.AddDefaultHeader("Accept-Language", "en-US,en;q=0.8");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Origin", "https://booking2.airasia.com");
                //client.AddDefaultHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                client.AddDefaultHeader("Cache-Control", "max-age=0");
                client.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                client.Proxy = new WebProxy("128.199.91.32", 80)
                {
                    Credentials = new NetworkCredential("travorama", "tmi12345")
                };
                client.CookieContainer = new CookieContainer();
                return client;
            }

            private static bool Login(RestClient client)
            {
                _userName = "IDTDEZYCGK_ADMIN";
                _password = "Travorama123";
                var url = "LoginAgent.aspx";
                var request = new RestRequest(url, Method.POST);
                var postData =
                    @"ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=" + _userName +
                    @"&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=" + _password +
                    @"&TimeZoneDiff=420" +
                    @"&__EVENTTARGET=ControlGroupLoginAgentView$AgentLoginView$LinkButtonLogIn" +
                    @"&__EVENTARGUMENT=" +
                    @"&pageToken=";
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                request.AddHeader("Origin", "http://www.airasia.com");
                request.AddHeader("Referer", "http://www.airasia.com/id/id/login/travel-agent.page");
                var response = client.Execute(request);

                return response.ResponseUri.AbsolutePath == "/AgentHome.aspx";
            }
        }
    }
}
