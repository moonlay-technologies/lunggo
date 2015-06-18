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
        Denied = 4
    }

    public class PaymentStatusCd
    {
        public static string Mnemonic(PaymentStatus paymentStatus)
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
