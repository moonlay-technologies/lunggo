using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum FailureReason
    {
        None,
        InvalidId,
        PaymentFailure,
        VoucherNoLongerAvailable,
        BinPromoNoLongerEligible,
        MethodDiscountNoLongerEligible,
        MethodNotAvailable,
        VoucherNotEligible
    }
}
