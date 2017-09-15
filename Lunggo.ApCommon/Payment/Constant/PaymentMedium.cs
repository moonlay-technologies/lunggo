namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentMedium
    {
        Undefined = 0,
        Direct = 1,
        Veritrans = 2,
        Nicepay = 3
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
                case PaymentMedium.Nicepay:
                    return "NICE";
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
                case "NICE":
                    return PaymentMedium.Nicepay;
                default:
                    return PaymentMedium.Undefined;
            }
        }
    }
}
