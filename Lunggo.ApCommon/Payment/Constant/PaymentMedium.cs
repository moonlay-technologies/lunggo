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
                case PaymentMedium.Direct:
                    return "DRCT";
                case PaymentMedium.Veritrans:
                    return "VERI";
                case PaymentMedium.Nicepay:
                    return "NICE";
                case PaymentMedium.E2Pay:
                    return "E2PA";
                default:
                    return "";
            }
        }
        public static PaymentMedium Mnemonic(string paymentMedium)
        {
            switch (paymentMedium)
            {
                case "DRCT":
                    return PaymentMedium.Direct;
                case "VERI":
                    return PaymentMedium.Veritrans;
                case "NICE":
                    return PaymentMedium.Nicepay;
                case "E2PA":
                    return PaymentMedium.E2Pay;
                default:
                    return PaymentMedium.Undefined;
            }
        }
    }
}
