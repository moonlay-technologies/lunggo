using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentStatus 
    {
        // TODO flight fix this
        Undefined = 0,
        Cancelled = 1,
        Pending = 2,
        Accepted = 3,
        Denied = 4,
        Error = 5
    }

    public class PaymentStatusCd
    {
        public static string Mnemonic(PaymentStatus paymentStatus)
        {
            switch (paymentStatus)
            {
                case PaymentStatus.Accepted:
                    return "ACC";
                case PaymentStatus.Cancelled:
                    return "CAN";
                case PaymentStatus.Pending:
                    return "PEN";
                case PaymentStatus.Denied:
                    return "DEN";
                case PaymentStatus.Error:
                    return "ERR";
                default:
                    return "";
            }
        }
        public static PaymentStatus Mnemonic(string paymentStatus)
        {
            switch (paymentStatus)
            {
                case "ACC":
                    return PaymentStatus.Accepted;
                case "CAN":
                    return PaymentStatus.Cancelled;
                case "PEN":
                    return PaymentStatus.Pending;
                case "DEN":
                    return PaymentStatus.Denied;
                case "ERR":
                    return PaymentStatus.Error;
                default:
                    return PaymentStatus.Undefined;
            }
        }
    }
}
