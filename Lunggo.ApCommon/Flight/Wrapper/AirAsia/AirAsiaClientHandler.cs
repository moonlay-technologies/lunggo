using Lunggo.Framework.Config;
using Lunggo.Framework.Web;

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

            internal bool Login(ExtendedWebClient client)
            {
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers["Accept-Encoding"] = "gzip, deflate";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Origin"] = "http://www.airasia.com";
                client.Headers["Referer"] = "http://www.airasia.com/id/id/login/travel-agent.page";
                var postData =
                    @"ControlGroupLoginAgentView$AgentLoginView$TextBoxUserID=" + _userName +
                    @"&ControlGroupLoginAgentView$AgentLoginView$PasswordFieldPassword=" + _password +
                    @"&TimeZoneDiff=420" +
                    @"&__EVENTTARGET=ControlGroupLoginAgentView$AgentLoginView$LinkButtonLogIn" +
                    @"&__EVENTARGUMENT=" +
                    @"&pageToken=";
                client.UploadString(@"https://booking2.airasia.com/LoginAgent.aspx", postData);

                return client.ResponseUri.AbsolutePath == "/AgentHome.aspx";
            }
        }
    }
}
