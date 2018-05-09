using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Extension;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Service
{
    public partial class PaymentService
    {
        internal PaymentDetailsForDisplay ConvertToPaymentDetailsForDisplay(RsvPaymentDetails payment)
        {
            if (payment == null)
                return null;

            var time = payment.Time?.AddMilliseconds(-payment.Time.Value.Millisecond);
            var timeLimit = payment.TimeLimit.TruncateMilliseconds();

            var paymentForDisplay = new PaymentDetailsForDisplay();
            paymentForDisplay.Medium = payment.Medium;
            paymentForDisplay.Method = payment.Method;
            paymentForDisplay.Submethod = payment.Submethod;
            paymentForDisplay.Status = payment.Status;
            paymentForDisplay.Time = time;
            paymentForDisplay.TimeLimit = timeLimit;
            paymentForDisplay.TransferAccount = payment.TransferAccount;
            paymentForDisplay.RedirectionUrl = payment.RedirectionUrl;
            paymentForDisplay.OriginalPrice = payment.OriginalPriceIdr / payment.LocalCurrency.Rate;
            paymentForDisplay.DiscountCode = payment.DiscountCode;
            paymentForDisplay.DiscountNominal = payment.DiscountNominal / payment.LocalCurrency.Rate;
            paymentForDisplay.DiscountName = payment.Discount != null ? payment.Discount.DisplayName : null;
            paymentForDisplay.UniqueCode = payment.UniqueCode / payment.LocalCurrency.Rate;
            paymentForDisplay.TransferFee = payment.UniqueCode / payment.LocalCurrency.Rate;
            paymentForDisplay.Currency = payment.LocalCurrency;
            paymentForDisplay.FinalPrice = payment.LocalFinalPrice;
            paymentForDisplay.Refund = ConvertToRefundForDisplay(payment.Refund);
            paymentForDisplay.InvoiceNo = payment.InvoiceNo;
            
            return paymentForDisplay;
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
