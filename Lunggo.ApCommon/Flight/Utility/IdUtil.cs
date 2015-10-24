using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal class IdUtil
        {
            internal static string GetCoreId(string id)
            {
                return id.Substring(7);
            }

            internal static Supplier GetSupplier(string id)
            {
                return SupplierCd.Mnemonic(id.Substring(0, 4));
            }

            internal static FareType GetFareType(string id)
            {
                return FareTypeCd.Mnemonic(id.Substring(4, 3));
            }

            internal static string ConstructIntegratedId(string coreId, Supplier supplier, FareType fareType)
            {
                return SupplierCd.Mnemonic(supplier) + FareTypeCd.Mnemonic(fareType) + coreId;
            }
        }
    }
}