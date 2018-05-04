using System;
using System.Linq;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        public virtual decimal GetSurchargeNominal(PaymentDetails payment)
        {
            var surchargeList = GetSurchargeList();
            var surcharge =
                surchargeList.SingleOrDefault(
                    sur =>
                        payment.Method == sur.PaymentMethod &&
                        (sur.PaymentSubMethod == null || payment.Submethod == sur.PaymentSubMethod));
            return surcharge == null
                ? 0
                : Math.Ceiling((payment.OriginalPriceIdr - payment.DiscountNominal) * surcharge.Percentage / 100) +
                  surcharge.Constant;
        }
    }
}