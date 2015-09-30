using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        private class AirAsiaClientHandler
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
                    //_userName = ConfigManager.GetInstance().GetConfigValue("AirAsia", "agentUserName");
                    //_password = ConfigManager.GetInstance().GetConfigValue("AirAsia", "agentPassword");
                    _isInitialized = true;
                }
            }
        }
    }
}
