using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        private class MystiflyClientHandler : OnePointClient
        {
            private static readonly MystiflyClientHandler ClientInstance = new MystiflyClientHandler();
            private bool _isInitialized;
            private static string _accountNumber;
            private static string _userName;
            private static string _password;
            private static Target _target;
            internal string SessionId = "";

            internal Target Target
            {
                get { return _target; }
            }

            private MystiflyClientHandler()
            {
            
            }

            internal static MystiflyClientHandler GetClientInstance()
            {
                return ClientInstance;
            }

            internal void Init()
            {
                if (!_isInitialized)
                {
                    _accountNumber = EnvVariables.Get("mystifly", "apiAccountNumber");
                    _userName = EnvVariables.Get("mystifly", "apiUserName");
                    _password = EnvVariables.Get("mystifly", "apiPassword");
                    var targetServer = EnvVariables.Get("mystifly", "apiTargetServer");
                    switch (targetServer)
                    {
                        case "Test":
                            _target = Target.Test;
                            break;
                        case "Development":
                            _target = Target.Development;
                            break;
                        case "Production":
                            _target = Target.Production;
                            break;
                        default:
                            _target = Target.Default;
                            break;
                    }
                    _isInitialized = true;
                }
            }

            internal void CreateSession()
            {
                var request = new SessionCreateRQ()
                {
                    AccountNumber = _accountNumber,
                    UserName = _userName,
                    Password = _password,
                    Target = _target
                };
                var response = CreateSession(request);
                SessionId = response.SessionId;
            }
        }
    }
}
