using Lunggo.ApCommon.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper : FlightSupplierWrapperBase
    {
        private static readonly SriwijayaWrapper Instance = new SriwijayaWrapper();
        private bool _isInitialized;
        private static readonly SriwijayaClientHandler Client = SriwijayaClientHandler.GetClientInstance();

        private const Supplier SupplierNameField = Supplier.Sriwijaya;

        internal override Supplier SupplierName
        {
            get { return SupplierNameField; }
        }

        private SriwijayaWrapper()
        {

        }

        internal static SriwijayaWrapper GetInstance()
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

