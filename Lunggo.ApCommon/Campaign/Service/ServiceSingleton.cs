namespace Lunggo.ApCommon.Campaign.Service
{
    public partial class CampaignService
    {
        private static readonly CampaignService Instance = new CampaignService();
        private bool _isInitialized;

        private CampaignService()
        {
            
        }

        public static CampaignService GetInstance()
        {
            return Instance;
        }
    }
}
