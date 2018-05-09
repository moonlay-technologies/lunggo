using System;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentMedium
    {
        Undefined = 0,
        Direct = 1,
        Veritrans = 2,
        Nicepay = 3,
        E2Pay = 4
    }

    public class PaymentMediumCd
    {
        public static string Mnemonic(PaymentMedium paymentMedium)
        {

            switch (paymentMedium)
            {
                case PaymentMedium.Undefined:
                    return null;
                case PaymentMedium.Direct:
                    return "DIRECT";
                case PaymentMedium.Veritrans:
                    return "VERITRANS";
                case PaymentMedium.Nicepay:
                    return "NICEPAY";
                case PaymentMedium.E2Pay:
                    return "E2PAY";
                default:
                    throw new ArgumentException("Payment medium not implemented: " + paymentMedium);
            }
        }
        public static PaymentMedium Mnemonic(string paymentMedium)
        {
            switch (paymentMedium)
            {
                case "DRCT":
                case "DIRECT":
                    return PaymentMedium.Direct;
                case "VERI":
                case "VERITRANS":
                    return PaymentMedium.Veritrans;
                case "NICE":
                case "NICEPAY":
                    return PaymentMedium.Nicepay;
                case "E2PA":
                case "E2PAY":
                    return PaymentMedium.E2Pay;
                case "":
                case null:
                    return PaymentMedium.Undefined;
                default:
                    throw new ArgumentException("Payment medium not implemented: " + paymentMedium);
            }
        }
    }
}
