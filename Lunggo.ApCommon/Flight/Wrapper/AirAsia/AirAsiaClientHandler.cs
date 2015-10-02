using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Config;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        private partial class AirAsiaClientHandler : ExtendedWebClient
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
                    _userName = "IDTDEZYCGK_ADMIN";
                    _password = "Travorama123";
                    _isInitialized = true;
                }
            }

            internal void CreateSession()
            {
                Headers["Content-Type"] = "application/x-www-form-urlencoded";
                Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Headers["Accept-Encoding"] = "gzip, deflate";
                Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers["Upgrade-Insecure-Requests"] = "1";
                Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                Headers["Origin"] = "http://www.airasia.com";
                Headers["Referer"] = "http://www.airasia.com/id/id/login/travel-agent.page";
                var postData =
                    @"ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=" + _userName +
                    @"&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=" + _password +
                    @"&TimeZoneDiff=420" +
                    @"&__EVENTTARGET=ControlGroupLoginAgentView$AgentLoginView$LinkButtonLogIn" +
                    @"&__EVENTARGUMENT=" +
                    @"&pageToken=";
                UploadString(@"https://booking2.airasia.com/LoginAgent.aspx", postData);
            }
        }
    }
}
