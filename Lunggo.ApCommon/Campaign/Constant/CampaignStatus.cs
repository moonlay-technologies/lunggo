namespace Lunggo.ApCommon.Campaign.Constant
{
    public enum CampaignStatus
    {
        Undefined = 0,
        Success = 1,
        CampaignNotStartedYet = 2,
        CampaignHasEnded = 3,
        CampaignInactive = 4,
        NoVoucherRemaining = 5,
        BelowMinimumSpend = 6,
        EmailNotEligible = 7,
        UpdateError = 8,
        VoucherNotFound = 9,
        VoucherAlreadyUsed = 10
    }
}
