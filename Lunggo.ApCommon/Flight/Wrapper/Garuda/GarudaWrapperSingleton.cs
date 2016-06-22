using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Garuda
{
    internal partial class GarudaWrapper : FlightSupplierWrapperBase
    {
        private static readonly GarudaWrapper Instance = new GarudaWrapper();
        private bool _isInitialized;
        private static readonly GarudaClientHandler Client = GarudaClientHandler.GetClientInstance();
        private const Supplier SupplierNameField = Supplier.Garuda;

        internal override Supplier SupplierName
        {
            get { return SupplierNameField; }
        }

        private GarudaWrapper()
        {
            
        }

        internal static GarudaWrapper GetInstance()
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
