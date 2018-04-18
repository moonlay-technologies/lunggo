namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentSubmethod
    {
        Undefined = 0,
        Mandiri = 1,
        BCA = 2,
        Permata = 3,
        BNI = 4,
        BRI = 5,
        CIMB = 6,
        Maybank = 7,
        Danamon = 8,
        KEBHana = 9,
        Other = 99
    }

    public class PaymentSubmethodCd
    {
        public static string Mnemonic(PaymentSubmethod paymentMedium)
        {
            switch (paymentMedium)
            {
                case PaymentSubmethod.BCA:
                    return "BCA";
                case PaymentSubmethod.Mandiri:
                    return "Mandiri";
                case PaymentSubmethod.Permata:
                    return "Permata";
                case PaymentSubmethod.BNI:
                    return "BNI";
                case PaymentSubmethod.BRI:
                    return "BRI";
                case PaymentSubmethod.CIMB:
                    return "CIMB";
                case PaymentSubmethod.Maybank:
                    return "Maybank";
                case PaymentSubmethod.Danamon:
                    return "Danamon";
                case PaymentSubmethod.KEBHana:
                    return "KEBHana";
                case PaymentSubmethod.Other:
                    return "Other";
                default:
                    return null;
            }
        }

        public static PaymentSubmethod Mnemonic(string paymentMedium)
        {
            switch (paymentMedium)
            {
                case "BCA":
                    return PaymentSubmethod.BCA;
                case "Mandiri":
                    return PaymentSubmethod.Mandiri;
                case "Permata":
                    return PaymentSubmethod.Permata;
                case "BNI":
                    return PaymentSubmethod.BNI;
                case "BRI":
                    return PaymentSubmethod.BRI;
                case "CIMB":
                    return PaymentSubmethod.CIMB;
                case "Maybank":
                    return PaymentSubmethod.Maybank;
                case "Danamon":
                    return PaymentSubmethod.Danamon;
                case "KEBHana":
                    return PaymentSubmethod.KEBHana;
                case "Other":
                    return PaymentSubmethod.Other;
                default:
                    return PaymentSubmethod.Undefined;
            }
        }
    }
}
