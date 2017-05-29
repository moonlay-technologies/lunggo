using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    class TiketWrapperSingleton
    {
    }

    internal partial class TiketWrapper : FlightSupplierWrapperBase
    {
        private static readonly TiketWrapper Instance = new TiketWrapper();
        private bool _isInitialized;
        private static readonly TiketClientHandler Client = TiketClientHandler.GetClientInstance();
        private const Supplier SupplierNameField = Supplier.Tiket;

        internal override Supplier SupplierName
        {
            get { return SupplierNameField; }
        }

        private TiketWrapper()
        {

        }

        internal static TiketWrapper GetInstance()
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
