using System.Net;
using Lunggo.Framework.Config;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        private partial class CitilinkClientHandler
        {
            private static readonly CitilinkClientHandler ClientInstance = new CitilinkClientHandler();
            private bool _isInitialized;
            private static string _userName;
            private static string _password;

            private CitilinkClientHandler()
            {

            }

            internal static CitilinkClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    _userName = EnvVariables.Get("citilink", "webUserName");
                    _password = EnvVariables.Get("citilink", "webPassword");
                    _isInitialized = true;
                }
            }

            private static RestClient CreateCustomerClient()
            {
                var client = new RestClient("https://book.citilink.co.id");
                client.AddDefaultHeader("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Referer", "https://www.citilink.co.id/");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                return client;
            }

            private static RestClient CreateAgentClient()
            {
                var client = new RestClient("https://book.citilink.co.id");
                client.AddDefaultHeader("Content-Type", "application/x-www-form-urlencoded");
                client.AddDefaultHeader("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");
                client.AddDefaultHeader("Upgrade-Insecure-Requests", "1");
                client.AddDefaultHeader("Host", "book.citilink.co.id");
                client.AddDefaultHeader("Origin", "https://book.citilink.co.id");
                //client.AddDefaultHeader("Referer", "https://book.citilink.co.id/LoginAgent.aspx?culture=id-ID");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.CookieContainer = new CookieContainer();
                return client;
            }

            private static void Login(RestClient client)
            {
                var url = "LoginAgent.aspx";
                var request = new RestRequest(url, Method.POST);
                string postData = @"ControlGroupLoginAgentView%24AgentLoginView%24ButtonLogIn=Log+In" +
                     @"&ControlGroupLoginAgentView%24AgentLoginView%24PasswordFieldPassword=" + _password +
                     @"&ControlGroupLoginAgentView%24AgentLoginView%24TextBoxUserID=" + _userName +
                     @"&__EVENTARGUMENT=" +
                     @"&__EVENTTARGET=" +
                     @"&__VIEWSTATE=%2FwEPDwUBMGRkBsrCYiDYbQKCOcoq%2FUTudEf14vk%3D" +
                     @"&pageToken=";
                request.AddHeader("Referer", "https://book.citilink.co.id/LoginAgent.aspx?culture=id-ID");
                request.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                client.Execute(request);
            }
        }
    }
}
