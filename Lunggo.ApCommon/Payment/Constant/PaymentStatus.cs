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
        Captured = 3,
        Settled = 4,
        Denied = 5
    }

    public class PaymentStatusCd
    {
        public static string Mnemonic(PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.Settled:
                    return "SET";
                case PaymentStatus.Captured:
                    return "CAP";
                case PaymentStatus.Cancelled:
                    return "CAN";
                case PaymentStatus.Pending:
                    return "PEN";
                case PaymentStatus.Denied:
                    return "DEN";
                default:
                    return "";
            }
        }
        public static PaymentStatus Mnemonic(string paymentStatus)
        {
            switch (paymentStatus)
            {
                case "SET":
                    return PaymentStatus.Settled;
                case "CAP":
                    return PaymentStatus.Captured;
                case "CAN":
                    return PaymentStatus.Cancelled;
                case "PEN":
                    return PaymentStatus.Pending;
                case "DEN":
                    return PaymentStatus.Denied;
                default:
                    return PaymentStatus.Undefined;
            }
        }
    }
}
