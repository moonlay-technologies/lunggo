namespace Lunggo.ApCommon.Pricing.Service
{
    public partial class PricingService
    {
        public void SetCurrencyRate(string currency, decimal rate)
        {
            SetCurrencyRateInCache(currency.ToUpper(), rate);
        }

        public decimal GetCurrencyRate(string currency)
        {
            return GetCurrencyRateInCache(currency.ToUpper());
        }
    }
}
