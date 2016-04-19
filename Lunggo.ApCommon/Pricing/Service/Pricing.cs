namespace Lunggo.ApCommon.Pricing.Service
{
    public partial class PricingService
    {
        internal void SetSupplierPrice(decimal supplierPrice, string suppplierCurrency)
        {
            Supplier = supplierPrice;
            SupplierCurrency = suppplierCurrency;
            SupplierRate = PricingService.GetInstance().GetCurrencyRate(suppplierCurrency);
        }

        internal void CalculateLocalPrice(string localCurrency)
        {

        }
    }
}
