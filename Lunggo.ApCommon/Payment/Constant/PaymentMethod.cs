using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentMethod
    {
        Undefined = 0,
        CreditCard = 1,
        BankTransfer = 2,
        MandiriClickPay = 3,
        CimbClicks = 4,
        VirtualAccount = 5
    }

    public class PaymentMethodCd
    {
        public static string Mnemonic(PaymentMethod paymentMedium)
        {
            switch (paymentMedium)
            {
                case PaymentMethod.CreditCard:
                    return "CRC";
                case PaymentMethod.BankTransfer:
                    return "TRF";
                case PaymentMethod.MandiriClickPay:
                    return "MCP";
                case PaymentMethod.CimbClicks:
                    return "CCL";
                case PaymentMethod.VirtualAccount:
                    return "VIR";
                default:
                    return "";
            }
        }
        public static PaymentMethod Mnemonic(string paymentMedium)
        {
            switch (paymentMedium)
            {
                case "CRC":
                    return PaymentMethod.CreditCard;
                case "TRF":
                    return PaymentMethod.BankTransfer;
                case "MCP":
                    return PaymentMethod.MandiriClickPay;
                case "CCL":
                    return PaymentMethod.CimbClicks;
                case "VIR":
                    return PaymentMethod.VirtualAccount;
                default:
                    return PaymentMethod.Undefined;
            }
        }
    }
}
