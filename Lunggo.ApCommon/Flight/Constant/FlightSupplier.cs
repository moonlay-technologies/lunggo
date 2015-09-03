using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum FlightSupplier
    {
        Undefined = 0,
        Mystifly = 1,
        Sriwijaya = 2,
        Citilink = 3
    }

    internal class FlightSupplierCd
    {
        internal static string Mnemonic(FlightSupplier supplier)
        {
            switch (supplier)
            {
                case FlightSupplier.Mystifly:
                    return "MYST";
                    case FlightSupplier.Sriwijaya:
                    return "SRIW";
                    case FlightSupplier.Citilink:
                    return "CITI";
                default:
                    return "";
            }
        }

        internal static FlightSupplier Mnemonic(string supplier)
        {
            switch (supplier)
            {
                case "MYST":
                    return FlightSupplier.Mystifly;
                case "SRIW":
                    return FlightSupplier.Sriwijaya;
                case "CITI":
                    return FlightSupplier.Citilink;
                default:
                    return FlightSupplier.Undefined;
            }
        }
    }
}
