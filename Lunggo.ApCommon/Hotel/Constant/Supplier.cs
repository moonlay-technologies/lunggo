using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum Supplier
    {
        Undefined = 0,
        HotelBeds = 1,
        Tiket = 2,
    }

    internal class SupplierCd
    {
        internal static string Mnemonic(Supplier supplier)
        {
            switch (supplier)
            {
                case Supplier.Tiket:
                    return "TKET";
                case Supplier.HotelBeds:
                    return "HBED";
                default:
                    return null;
            }
        }

        internal static Supplier Mnemonic(string supplier)
        {
            switch (supplier)
            {
                case "HBED":
                    return Supplier.HotelBeds;
                case "TKET":
                    return Supplier.Tiket;
                default:
                    return Supplier.Undefined;
            }
        }
    }
}
