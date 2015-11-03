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
        Challenged = 7
    }

    internal class PaymentStatusCd
    {
        internal static string Mnemonic(PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.Settled:
                    return "SET";
                case PaymentStatus.Cancelled:
                    return "CAN";
                case PaymentStatus.Pending:
                    return "PEN";
                case PaymentStatus.Denied:
                    return "DEN";
                case PaymentStatus.Expired:
                    return "EXP";
                case PaymentStatus.Verifying:
                    return "VER";
                case PaymentStatus.Challenged:
                    return "CHA";
                default:
                    return null;
            }
        }
        internal static PaymentStatus Mnemonic(string paymentStatus)
        {
            switch (paymentStatus)
            {
                case "SET":
                    return PaymentStatus.Settled;
                case "CAN":
                    return PaymentStatus.Cancelled;
                case "PEN":
                    return PaymentStatus.Pending;
                case "DEN":
                    return PaymentStatus.Denied;
                case "EXP":
                    return PaymentStatus.Expired;
                case "VER":
                    return PaymentStatus.Verifying;
                case "CHA":
                    return PaymentStatus.Challenged;
                default:
                    return PaymentStatus.Undefined;
            }
        }
    }
}
