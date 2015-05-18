using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Config;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper : WrapperBase
    {
        private static readonly MystiflyWrapper Instance = new MystiflyWrapper();
        private bool _isInitialized;
        private static readonly MystiflyClientHandler Client = MystiflyClientHandler.GetClientInstance();

        private static string _accountNumber;
        private static string _userName;
        private static string _password;
        private static string _targetServer;

        private MystiflyWrapper()
        {
            
        }

        internal static MystiflyWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                _accountNumber = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiAccountNumber");
                _userName = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiUserName");
                _password = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiPassword");
                _targetServer = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiTargetServer");
                Client.Init(_accountNumber, _userName, _password, _targetServer);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("MystiflyWrapper is already initialized");
            }
        }
    }
}
