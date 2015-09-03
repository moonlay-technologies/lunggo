using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentMedium
    {
        Undefined = 0,
        Direct = 1,
        Veritrans = 2
    }

    internal class PaymentMediumCd
    {
        internal static string Mnemonic(PaymentMedium paymentMedium)
        {
            switch (paymentMedium)
            {
                case PaymentMedium.Direct:
                    return "DRCT";
                case PaymentMedium.Veritrans:
                    return "VERI";
                default:
                    return "";
            }
        }
        internal static PaymentMedium Mnemonic(string paymentMedium)
        {
            switch (paymentMedium)
            {
                case "DRCT":
                    return PaymentMedium.Direct;
                case "VERI":
                    return PaymentMedium.Veritrans;
                default:
                    return PaymentMedium.Undefined;
            }
        }
    }
}
