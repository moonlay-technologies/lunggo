namespace Lunggo.ApCommon.Product.Constant
{
    public enum ProductType
    {
        Undefined = 0,
        Flight = 1,
        Hotel = 2,
        Activity = 3
    }

    internal class ProductTypeCd
    {
        internal static ProductType Parse(string rsvNo)
        {
            return (ProductType) int.Parse(rsvNo.Substring(0, 1));
        }
    }
}
