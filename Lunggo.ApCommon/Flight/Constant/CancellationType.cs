namespace Lunggo.ApCommon.Flight.Constant
{
    public enum CancellationType
    {
        Undefined = 0,
        Customer = 1,
        Airline = 2,
        Supplier = 3
    }

    internal class CancellationTypeCd
    {
        internal static string Mnemonic(CancellationType type)
        {
            switch (type)
            {
                case CancellationType.Customer:
                    return "CUS";
                case CancellationType.Airline:
                    return "AIR";
                case CancellationType.Supplier:
                    return "SUP";
                default:
                    return null;
            }
        }
        internal static CancellationType Mnemonic(string type)
        {
            switch (type)
            {
                case "CUS":
                    return CancellationType.Customer;
                case "AIR":
                    return CancellationType.Airline;
                case "SUP":
                    return CancellationType.Supplier;
                default:
                    return CancellationType.Undefined;
            }
        }
    }
}
