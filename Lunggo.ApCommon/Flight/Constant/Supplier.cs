namespace Lunggo.ApCommon.Flight.Constant
{
    public enum Supplier
    {
        Undefined = 0,
        Mystifly = 1,
        Sriwijaya = 2,
        Citilink = 3,
        AirAsia = 4,
        LionAir = 5,
        Garuda = 6,
        Tiket = 7
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
                case Supplier.LionAir:
                    return "LION";
                case Supplier.Garuda:
                    return "GARU";
                case Supplier.Tiket:
                    return "TKET";
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
                case "LION":
                    return Supplier.LionAir;
                case "GARU":
                    return Supplier.Garuda;
                case "TKET":
                    return Supplier.Tiket;
                default:
                    return Supplier.Undefined;
            }
        }
    }
}