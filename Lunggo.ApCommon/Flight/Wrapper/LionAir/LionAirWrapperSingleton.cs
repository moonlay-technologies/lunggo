using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.LionAir
{
    internal partial class LionAirWrapper : FlightSupplierWrapperBase
    {
        private static readonly LionAirWrapper Instance = new LionAirWrapper();
        private bool _isInitialized;
        private static readonly LionAirClientHandler Client = LionAirClientHandler.GetClientInstance();
        private const Supplier SupplierNameField = Supplier.LionAir;

        internal override Supplier SupplierName
        {
            get { return SupplierNameField; }
        }

        private LionAirWrapper()
        {
            
        }

        internal static LionAirWrapper GetInstance()
        {
            return Instance;
        }

        internal override void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
        }
    }
}
