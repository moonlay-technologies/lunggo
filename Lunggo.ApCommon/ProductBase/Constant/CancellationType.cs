namespace Lunggo.ApCommon.ProductBase.Constant
{
    public enum CancellationType
    {
        NotCancelled = 0,
        Customer = 1,
        Supplier = 2
    }

    internal class CancellationTypeCd
    {
        internal static string Mnemonic(CancellationType type)
        {
            switch (type)
            {
                case CancellationType.Customer:
                    return "CUS";
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
                case "SUP":
                    return CancellationType.Supplier;
                default:
                    return CancellationType.NotCancelled;
            }
        }
    }
}
