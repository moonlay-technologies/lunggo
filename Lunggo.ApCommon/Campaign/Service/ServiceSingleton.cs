using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        private static readonly CampaignService Instance = new CampaignService();
        private bool _isInitialized;
        private PaymentService _paymentService = new PaymentService();

        private CampaignService()
        {
            
        }

        public static CampaignService GetInstance()
        {
            return Instance;
        }
    }
}
