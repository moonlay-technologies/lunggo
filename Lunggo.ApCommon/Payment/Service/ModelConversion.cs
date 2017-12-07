using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Repository.TableRecord;

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
                Medium = payment.Medium,
                Method = payment.Method,
                Submethod = payment.Submethod,
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
                TransferFee = payment.UniqueCode / payment.LocalCurrency.Rate,
                Currency = payment.LocalCurrency,
                FinalPrice = payment.LocalFinalPrice,
                Refund = ConvertToRefundForDisplay(payment.Refund),
                InvoiceNo = payment.InvoiceNo
            };
        }

        internal Transaction ConvertToTransaction(TransactionJournalTableRecord record)
        {
            return new Transaction
            {
                Id = record.Id,
                Time = record.Time.GetValueOrDefault(),
                Amount = record.Amount.GetValueOrDefault(),
                BalanceAfter = record.BalanceAfter.GetValueOrDefault(),
                Remark = record.Remark
            };
        }

        internal AccountBalance ConvertToAccountBalance(AccountBalanceTableRecord record)
        {
            return new AccountBalance
            {
                Balance = record.Balance.GetValueOrDefault(),
                Withdrawable = record.Withdrawable.GetValueOrDefault()
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
