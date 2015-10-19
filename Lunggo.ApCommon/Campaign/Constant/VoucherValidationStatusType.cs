using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Constant
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
        UpdateSuccess = 7,
        Undefined = 8
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
                case VoucherValidationStatusType.UpdateSuccess:
                    return "UpdateSuccess";
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
                case "UpdateSuccess":
                    return VoucherValidationStatusType.UpdateSuccess;
                default:
                    return VoucherValidationStatusType.Undefined;
            }
        }
    }
}
