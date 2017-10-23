namespace Lunggo.ApCommon.Campaign.Constant
{
    public enum CampaignType
    {
        Undefined = 0,
        Public = 1,
        Private = 2,
        Member = 3,
        Invite = 4
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
                case CampaignType.Invite:
                    return "INV";
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
                case "INV":
                    return CampaignType.Invite;
                default:
                    return CampaignType.Undefined;
            }
        }
    }
}
