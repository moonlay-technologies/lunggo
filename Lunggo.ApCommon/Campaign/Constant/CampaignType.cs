namespace Lunggo.ApCommon.Campaign.Constant
{
    public enum CampaignType
    {
        Public = 0,
        Private = 1,
        Member = 2,
        Undefined = 3
    }

    internal class CampaignTypeCd
    {
        internal static string Mnemonic(CampaignType campaignType)
        {
            switch (campaignType)
            {
                case CampaignType.Public:
                    return "PUB";
                case CampaignType.Private:
                    return "PRI";
                case CampaignType.Member:
                    return "MEM";
                default:
                    return null;
            }
        }
        internal static CampaignType Mnemonic(string campaignType)
        {
            switch (campaignType)
            {
                case "PUB":
                    return CampaignType.Public;
                case "PRI":
                    return CampaignType.Private;
                case "MEM":
                    return CampaignType.Member;
                default:
                    return CampaignType.Undefined;
            }
        }
    }
}
