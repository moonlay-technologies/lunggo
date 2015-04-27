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

        private readonly static string AccountNumber = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiAccountNumber");
        private readonly static string UserName = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiUserName");
        private readonly static string Password = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiPassword");
        private readonly static string TargetServer = ConfigManager.GetInstance().GetConfigValue("mystifly", "apiTargetServer");

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
                Client.Init(AccountNumber, UserName, Password, TargetServer);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("MystiflyWrapper is already initialized");
            }
        }
    }
}
