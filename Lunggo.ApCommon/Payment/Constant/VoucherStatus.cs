using System;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum VoucherStatus
    {
        Undefined = 0,
        Success = 1,
        OutsidePeriod = 2,
        VoucherDepleted = 3,
        BelowMinimumSpend = 4,
        VoucherNotFound = 5,
        TermsConditionsNotEligible = 6,
        InternalError = 7
    }
}
