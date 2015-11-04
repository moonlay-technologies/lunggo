﻿namespace Lunggo.ApCommon.Campaign.Constant
{
    public enum VoucherValidationStatusType
    {
        CampaignNotStartedYet = 0,
        CampaignHasEnded = 1,
        CampaignInactive = 2,
        NoVoucherRemaining = 3,
        BelowMinimumSpend = 4,
        EmailNotEligible = 5,
        UpdateError = 6,
        Success = 7,
        VoucherNotFound = 8,
        VoucherAlreadyUsed = 9,
        Undefined = 10
    }

    internal class VoucherValidationStatusTypeMessage
    {
        internal static string Mnemonic(VoucherValidationStatusType voucherValidationStatusType)
        {
            switch (voucherValidationStatusType)
            {
                case VoucherValidationStatusType.CampaignNotStartedYet:
                    return "CampaignNotStartedYet";
                case VoucherValidationStatusType.CampaignHasEnded:
                    return "CampaignHasEnded";
                case VoucherValidationStatusType.CampaignInactive:
                    return "CampaignInactive";
                case VoucherValidationStatusType.NoVoucherRemaining:
                    return "NoVoucherRemaining";
                case VoucherValidationStatusType.BelowMinimumSpend:
                    return "BelowMinimumSpend";
                case VoucherValidationStatusType.EmailNotEligible:
                    return "EmailNotEligible";
                case VoucherValidationStatusType.UpdateError:
                    return "UpdateError";
                case VoucherValidationStatusType.Success:
                    return "Success";
                case VoucherValidationStatusType.VoucherNotFound:
                    return "VoucherNotFound";
                case VoucherValidationStatusType.VoucherAlreadyUsed:
                    return "VoucherAlreadyUsed";
                default:
                    return null;
            }
        }
        internal static VoucherValidationStatusType Mnemonic(string voucherValidationStatusType)
        {
            switch (voucherValidationStatusType)
            {
                case "CampaignNotStartedYet":
                    return VoucherValidationStatusType.CampaignNotStartedYet;
                case "CampaignHasEnded":
                    return VoucherValidationStatusType.CampaignHasEnded;
                case "CampaignInactive":
                    return VoucherValidationStatusType.CampaignInactive;
                case "NoVoucherRemaining":
                    return VoucherValidationStatusType.NoVoucherRemaining;
                case "BelowMinimumSpend":
                    return VoucherValidationStatusType.BelowMinimumSpend;
                case "EmailNotEligible":
                    return VoucherValidationStatusType.EmailNotEligible;
                case "UpdateError":
                    return VoucherValidationStatusType.UpdateError;
                case "Success":
                    return VoucherValidationStatusType.Success;
                case "VoucherNotFound":
                    return VoucherValidationStatusType.VoucherNotFound;
                case "VoucherAlreadyUsed":
                    return VoucherValidationStatusType.VoucherAlreadyUsed;
                default:
                    return VoucherValidationStatusType.Undefined;
            }
        }
    }
}
