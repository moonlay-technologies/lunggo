namespace Lunggo.ApCommon.Pricing.Service
{
    public partial class PricingService
    {
        private static readonly PricingService Instance = new PricingService();
        private bool _isInitialized;

        private PricingService()
        {
            
        }

        public static PricingService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
            }
        }
    }
}
