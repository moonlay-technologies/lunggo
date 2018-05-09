using System;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Environment;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        private static readonly VeritransWrapper Veritrans = new VeritransWrapper();
        private static readonly NicepayWrapper Nicepay = new NicepayWrapper();
        private static readonly E2PayWrapper E2Pay = new E2PayWrapper();

        internal virtual bool ProcessPayment(PaymentDetails paymentDetails, TransactionDetails trxDetails)
        {
            if (paymentDetails.Medium != PaymentMedium.Undefined)
                ProcessPaymentInternal(paymentDetails, trxDetails);
            else
            {
                paymentDetails.Status = PaymentStatus.Failed;
                paymentDetails.FailureReason = FailureReason.MethodNotAvailable;
            }

            paymentDetails.PaidAmountIdr = paymentDetails.FinalPriceIdr;
            paymentDetails.LocalFinalPrice = paymentDetails.FinalPriceIdr;
            paymentDetails.LocalPaidAmount = paymentDetails.FinalPriceIdr;

            return true;
        }

        private void ProcessPaymentInternal(PaymentDetails payment, TransactionDetails transactionDetails)
        {
            switch (payment.Medium)
            {
                case PaymentMedium.Nicepay:
                    Nicepay.ProcessPayment(payment, transactionDetails);
                    break;
                case PaymentMedium.Veritrans:
                    Veritrans.ProcessPayment(payment, transactionDetails);
                    break;
                case PaymentMedium.E2Pay:
                    E2Pay.ProcessPayment(payment, transactionDetails);
                    break;
                case PaymentMedium.Direct:
                    var env = EnvVariables.Get("general", "environment");
                    if (env != "production")
                    {
                        payment.Status = PaymentStatus.Settled;
                    }
                    else
                    {
                        payment.Status = PaymentStatus.Pending;
                        payment.TransferAccount = GetBankTransferAccount(payment.Submethod);
                    }
                    break;
                default:
                    throw new Exception("Invalid payment medium. \"" + payment.Medium + "\" shouldn't be directed here.");
            }
        }

        public string GetBankTransferAccount(PaymentSubmethod submethod)
        {
            switch (submethod)
            {
                case PaymentSubmethod.Mandiri:
                    return "1020006675802";
                default:
                    return null;
            }
        }
    }
}
