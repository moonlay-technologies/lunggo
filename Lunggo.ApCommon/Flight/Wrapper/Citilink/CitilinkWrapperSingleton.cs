using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper : FlightSupplierWrapperBase
    {
        private static readonly CitilinkWrapper Instance = new CitilinkWrapper();
        private bool _isInitialized;
        private static readonly CitilinkClientHandler Client = CitilinkClientHandler.GetClientInstance();

        private const Supplier SupplierNameField = Supplier.Citilink;

        internal override Supplier SupplierName
        {
            get { return SupplierNameField; }
        }

        private CitilinkWrapper()
        {

        }

        internal static CitilinkWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
        }
    }
}