namespace Lunggo.ApCommon.Constant
{
    public enum Supplier
    {
        Undefined = 0,
        Mystifly = 1,
        Sriwijaya = 2,
        Citilink = 3,
        AirAsia = 4,
        HotelsPro = 5
    }

    internal class SupplierCd
    {
        internal static string Mnemonic(Supplier supplier)
        {
            switch (supplier)
            {
                case Supplier.Mystifly:
                    return "MYST";
                case Supplier.Sriwijaya:
                    return "SRIW";
                case Supplier.Citilink:
                    return "CITI";
                case Supplier.AirAsia:
                    return "AIRA";
                default:
                    return null;
            }
        }

        internal static Supplier Mnemonic(string supplier)
        {
            switch (supplier)
            {
                case "MYST":
                    return Supplier.Mystifly;
                case "SRIW":
                    return Supplier.Sriwijaya;
                case "CITI":
                    return Supplier.Citilink;
                case "AIRA":
                    return Supplier.AirAsia;
                default:
                    return Supplier.Undefined;
            }
        }
    }
}