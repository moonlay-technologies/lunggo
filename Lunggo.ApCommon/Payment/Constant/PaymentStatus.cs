using System;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentStatus
    {
        Undefined = 0,
        Cancelled = 1,
        Pending = 2,
        Settled = 3,
        Denied = 4,
        Expired = 5,
        Verifying = 6,
        Challenged = 7,
        Failed = 8,
        MethodNotSet = 9,
    }

    public class PaymentStatusCd
    {
        public static string Mnemonic(PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.MethodNotSet:
                    return "METHODNOTSET";
                case PaymentStatus.Settled:
                    return "SETTLED";
                case PaymentStatus.Cancelled:
                    return "CANCELLED";
                case PaymentStatus.Pending:
                    return "PENDING";
                case PaymentStatus.Denied:
                    return "DENIED";
                case PaymentStatus.Expired:
                    return "EXPIRED";
                case PaymentStatus.Verifying:
                    return "VERIFYING";
                case PaymentStatus.Challenged:
                    return "CHALLENGED";
                case PaymentStatus.Failed:
                    return "FAILED";
                default:
                    throw new ArgumentException("Payment status not defined: " + paymentStatus);
            }
        }
        public static PaymentStatus Mnemonic(string paymentStatus)
        {
            switch (paymentStatus)
            {
                case "METHODNOTSET":
                    return PaymentStatus.MethodNotSet;
                case "SET":
                case "SETTLED":
                    return PaymentStatus.Settled;
                case "CAN":
                case "CANCELLED":
                    return PaymentStatus.Cancelled;
                case "PEN":
                case "PENDING":
                    return PaymentStatus.Pending;
                case "DEN":
                case "DENIED":
                    return PaymentStatus.Denied;
                case "EXP":
                case "EXPIRED":
                    return PaymentStatus.Expired;
                case "VER":
                case "VERIFYING":
                    return PaymentStatus.Verifying;
                case "CHA":
                case "CHALLENGED":
                    return PaymentStatus.Challenged;
                case "FAI":
                case "FAILED":
                    return PaymentStatus.Failed;
                default:
                    throw new ArgumentException("Payment status not defined: " + paymentStatus);
            }
        }
    }
}
