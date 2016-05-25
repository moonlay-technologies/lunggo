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

            return new PaymentDetailsForDisplay
            {
                Method = payment.Method,
                Status = payment.Status,
                Time = payment.Time,
                TimeLimit = payment.TimeLimit,
                TransferAccount = payment.TransferAccount,
                RedirectionUrl = payment.RedirectionUrl,
                OriginalPrice = payment.OriginalPriceIdr / payment.LocalCurrency.Rate,
                DiscountCode = payment.DiscountCode,
                DiscountNominal = payment.DiscountNominal / payment.LocalCurrency.Rate,
                DiscountName = payment.Discount.DisplayName,
                TransferFee = payment.TransferFee / payment.LocalCurrency.Rate,
                Currency = payment.LocalCurrency,
                FinalPrice = payment.LocalFinalPrice,
                Refund = payment.Refund,
                InvoiceNo = payment.InvoiceNo
            };
        }
    }
}
