using System;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        private class MystiflyClientHandler : OnePointClient, IClientHandler
        {
            private static readonly MystiflyClientHandler ClientInstance = new MystiflyClientHandler();
            private bool _isInitialized;
            private static string _accountNumber;
            private static string _userName;
            private static string _password;
            private static Target _target;
            public string SessionId = "";

            internal static Target Target
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

            internal void Init(string accountNumber, string userName, string password, TargetServer target)
            {
                if (!_isInitialized)
                {
                    _accountNumber = accountNumber;
                    _userName = userName;
                    _password = password;
                    switch (target)
                    {
                        case TargetServer.Test:
                            _target = Target.Test;
                            break;
                        case TargetServer.Development:
                            _target = Target.Development;
                            break;
                        case TargetServer.Production:
                            _target = Target.Production;
                            break;
                        default:
                            _target = Target.Default;
                            break;
                    }
                    _isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("MystiflyClientHandler is already initialized");
                }
            }

            void IClientHandler.Init(string accountNumber, string userName, string password, TargetServer target)
            {
                Init(accountNumber, userName, password, target);
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
