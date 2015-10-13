using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Web;

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
                    _userName = "Travelmadezy";
                    _password = "Standar1234";
                    _isInitialized = true;
                }
            }

            internal void CreateSession(ExtendedWebClient client)
            {
                string URI = "https://book.citilink.co.id/LoginAgent.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/LoginAgent.aspx?culture=id-ID";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Upgrade-Insecure-Requests"] = "1";

                string myParameters = @"ControlGroupLoginAgentView$AgentLoginView$ButtonLogIn=Log+In" +
                     @"&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=" + _password +
                     @"&ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=" + _userName +
                     @"&__EVENTARGUMENT=" +
                     @"&__EVENTTARGET=" +
                     @"&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=" +
                     @"&pageToken";
                client.UploadString(URI, "POST", myParameters);
            }
        }
    }
}
