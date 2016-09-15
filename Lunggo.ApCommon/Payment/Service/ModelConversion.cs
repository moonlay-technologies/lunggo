using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        internal PaymentDetailsForDisplay ConvertToPaymentDetailsForDisplay(PaymentDetails payment)
        {
            if (payment == null)
                return null;

            var time = payment.Time.HasValue
                ? payment.Time.Value.AddMilliseconds(-payment.Time.Value.Millisecond)
                : (DateTime?) null;
            var timeLimit = payment.TimeLimit.AddMilliseconds(-payment.TimeLimit.Millisecond);

            return new PaymentDetailsForDisplay
            {
                Method = payment.Method,
                Status = payment.Status,
                Time = time,
                TimeLimit = timeLimit,
                TransferAccount = payment.TransferAccount,
                RedirectionUrl = payment.RedirectionUrl,
                OriginalPrice = payment.OriginalPriceIdr / payment.LocalCurrency.Rate,
                DiscountCode = payment.DiscountCode,
                DiscountNominal = payment.DiscountNominal / payment.LocalCurrency.Rate,
                DiscountName = payment.Discount != null ? payment.Discount.DisplayName : null,
                UniqueCode = payment.UniqueCode / payment.LocalCurrency.Rate,
                Currency = payment.LocalCurrency,
                FinalPrice = payment.LocalFinalPrice,
                Refund = ConvertToRefundForDisplay(payment.Refund),
                InvoiceNo = payment.InvoiceNo
            };
        }

        private RefundForDisplay ConvertToRefundForDisplay(Refund refund)
        {
            if (refund == null)
                return null;

            return new RefundForDisplay
            {
                Time = refund.Time,
                BeneficiaryBank = refund.BeneficiaryBank,
                BeneficiaryAccount = refund.BeneficiaryAccount,
                Currency = refund.Currency,
                Amount = refund.Amount
            };
        }
    }
}
