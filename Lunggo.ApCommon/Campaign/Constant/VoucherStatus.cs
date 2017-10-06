namespace Lunggo.ApCommon.Campaign.Constant
{
    public enum VoucherStatus
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
        VoucherAlreadyUsed = 10,
        ReservationNotFound = 11,
        ProductNotEligible = 12,
        NoBudgetRemaining = 13,
        ReservationNotEligible = 14,
        PlatformNotEligible = 15
    }

    internal class VoucherStatusCd
    {
        internal static string Mnemonic(VoucherStatus voucherValidationStatusType)
        {
            switch (voucherValidationStatusType)
            {
                case VoucherStatus.CampaignNotStartedYet:
                    return "CampaignNotStartedYet";
                case VoucherStatus.CampaignHasEnded:
                    return "CampaignHasEnded";
                case VoucherStatus.CampaignInactive:
                    return "CampaignInactive";
                case VoucherStatus.NoVoucherRemaining:
                    return "NoVoucherRemaining";
                case VoucherStatus.BelowMinimumSpend:
                    return "BelowMinimumSpend";
                case VoucherStatus.EmailNotEligible:
                    return "EmailNotEligible";
                case VoucherStatus.UpdateError:
                    return "UpdateError";
                case VoucherStatus.Success:
                    return "Success";
                case VoucherStatus.VoucherNotFound:
                    return "VoucherNotFound";
                case VoucherStatus.VoucherAlreadyUsed:
                    return "VoucherAlreadyUsed";
                case VoucherStatus.ReservationNotEligible:
                    return "ReservationNotEligible";
                case VoucherStatus.PlatformNotEligible:
                    return "PlatformNotEligible";
                default:
                    return null;
            }
        }
        internal static VoucherStatus Mnemonic(string voucherValidationStatusType)
        {
            switch (voucherValidationStatusType)
            {
                case "CampaignNotStartedYet":
                    return VoucherStatus.CampaignNotStartedYet;
                case "CampaignHasEnded":
                    return VoucherStatus.CampaignHasEnded;
                case "CampaignInactive":
                    return VoucherStatus.CampaignInactive;
                case "NoVoucherRemaining":
                    return VoucherStatus.NoVoucherRemaining;
                case "BelowMinimumSpend":
                    return VoucherStatus.BelowMinimumSpend;
                case "EmailNotEligible":
                    return VoucherStatus.EmailNotEligible;
                case "UpdateError":
                    return VoucherStatus.UpdateError;
                case "Success":
                    return VoucherStatus.Success;
                case "VoucherNotFound":
                    return VoucherStatus.VoucherNotFound;
                case "VoucherAlreadyUsed":
                    return VoucherStatus.VoucherAlreadyUsed;
                case "ReservationNotEligible":
                    return VoucherStatus.ReservationNotEligible;
                case "PlatformNotEligible":
                    return VoucherStatus.PlatformNotEligible;
                default:
                    return VoucherStatus.Undefined;
            }
        }
    }
}
