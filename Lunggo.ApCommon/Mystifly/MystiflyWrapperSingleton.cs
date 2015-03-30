using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    public partial class MystiflyWrapper
    {
        private static readonly MystiflyWrapper Instance = new MystiflyWrapper();
        private bool _isInitialized;

        private MystiflyWrapper()
        {
            
        }

        public static MystiflyWrapper GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("MystiflyWrapper is already initialized");
            }
        }
    }
}
