using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        TransferConfirmed = 6,
        Challenged = 7,
        ReceiptSubmitted = 8
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
                case PaymentStatus.TransferConfirmed:
                    return "TRC";
                case PaymentStatus.Challenged:
                    return "CHA";
                case PaymentStatus.ReceiptSubmitted:
                    return "RCP";
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
                case "TRC":
                    return PaymentStatus.TransferConfirmed;
                case "CHA":
                    return PaymentStatus.Challenged;
                case "RCP":
                    return PaymentStatus.ReceiptSubmitted;
                default:
                    return PaymentStatus.Undefined;
            }
        }
    }
}
