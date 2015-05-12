using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Utility
{
    internal class FlightIdUtil
    {
        internal static string GetCoreId(string id)
        {
            return id.Substring(7);
        }

        internal static FlightSupplier GetSupplier(string id)
        {
            return FlightSupplierCd.Mnemonic(id.Substring(0, 4));
        }

        internal static FareType GetFareType(string id)
        {
            return FareTypeCd.Mnemonic(id.Substring(4, 3));
        }

        internal static string ConstructIntegratedId(string coreId, FlightSupplier supplier, FareType fareType)
        {
            return FlightSupplierCd.Mnemonic(supplier) + FareTypeCd.Mnemonic(fareType) + coreId;
        }
    }
}
