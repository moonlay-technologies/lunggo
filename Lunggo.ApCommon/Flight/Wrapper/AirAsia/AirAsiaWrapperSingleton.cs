using Lunggo.ApCommon.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper : FlightSupplierWrapperBase
    {
        private static readonly AirAsiaWrapper Instance = new AirAsiaWrapper();
        private bool _isInitialized;
        private static readonly AirAsiaClientHandler Client = AirAsiaClientHandler.GetClientInstance();
        private const Supplier SupplierNameField = Supplier.AirAsia;

        internal override Supplier SupplierName
        {
            get { return SupplierNameField; }
        }

        private AirAsiaWrapper()
        {
            
        }

        internal static AirAsiaWrapper GetInstance()
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
