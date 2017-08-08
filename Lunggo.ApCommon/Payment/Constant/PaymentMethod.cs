namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentMethod
    {
        Undefined = 0,
        CreditCard = 1,
        BankTransfer = 2,
        MandiriClickPay = 3,
        CimbClicks = 4,
        VirtualAccount = 5,
        BcaKlikpay = 6,
        EpayBri = 7,
        TelkomselTcash = 8,
        XlTunai = 9,
        BbmMoney = 10,
        IndosatDompetku = 11,
        MandiriEcash = 12,
        MandiriBillPayment = 13,
        Indomaret = 14,
        Credit = 15,
        Deposit = 16
    }

    public enum PaymentSubMethod
    {
        Undefined = 0,
        Mandiri = 1,
        BCA = 2,
        Permata = 3,
        BNI = 4,
        BRI = 5,
        CIMB = 6
    }

    internal class PaymentMethodCd
    {
        internal static string Mnemonic(PaymentMethod paymentMedium)
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
                case PaymentMethod.BcaKlikpay:
                    return "BKP";
                case PaymentMethod.EpayBri:
                    return "EPB";
                case PaymentMethod.TelkomselTcash:
                    return "TTC";
                case PaymentMethod.XlTunai:
                    return "XLT";
                case PaymentMethod.BbmMoney:
                    return "BBM";
                case PaymentMethod.IndosatDompetku:
                    return "IND";
                case PaymentMethod.MandiriEcash:
                    return "MEC";
                case PaymentMethod.MandiriBillPayment:
                    return "MBP";
                case PaymentMethod.Indomaret:
                    return "IDM";
                case PaymentMethod.Credit:
                    return "CRD";
                case PaymentMethod.Deposit:
                    return "DPS";
                default:
                    return "";
            }
        }
        internal static PaymentMethod Mnemonic(string paymentMedium)
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
                case "BKP":
                    return PaymentMethod.BcaKlikpay;
                case "EPB":
                    return PaymentMethod.EpayBri;
                case "TTC":
                    return PaymentMethod.TelkomselTcash;
                case "XLT":
                    return PaymentMethod.XlTunai;
                case "BBM":
                    return PaymentMethod.BbmMoney;
                case "IND":
                    return PaymentMethod.IndosatDompetku;
                case "MEC":
                    return PaymentMethod.MandiriEcash;
                case "MBP":
                    return PaymentMethod.MandiriBillPayment;
                case "IDM":
                    return PaymentMethod.Indomaret;
                case "CRD":
                    return PaymentMethod.Credit;
                case "DPS":
                    return PaymentMethod.Deposit;
                default:
                    return PaymentMethod.Undefined;
            }
        }
    }

    internal class PaymentSubMethodCd
    {
        internal static string Mnemonic(PaymentSubMethod paymentMedium)
        {
            switch (paymentMedium)
            {
                case PaymentSubMethod.BCA:
                    return "BCA";
                case PaymentSubMethod.Mandiri:
                    return "Mandiri";
                case PaymentSubMethod.Permata:
                    return "Permata";
                case PaymentSubMethod.BNI:
                    return "BNI";
                case PaymentSubMethod.BRI:
                    return "BRI";
                case PaymentSubMethod.CIMB:
                    return "CIMB";
                default:
                    return null;
            }
        }

        internal static PaymentSubMethod Mnemonic(string paymentMedium)
        {
            switch (paymentMedium)
            {
                case "BCA":
                    return PaymentSubMethod.BCA;
                case "Mandiri":
                    return PaymentSubMethod.Mandiri;
                case "Permata":
                    return PaymentSubMethod.Permata;
                case "BNI":
                    return PaymentSubMethod.BNI;
                case "BRI":
                    return PaymentSubMethod.BRI;
                case "CIMB":
                    return PaymentSubMethod.CIMB;
                default:
                    return PaymentSubMethod.Undefined;
            }
        }
    }
}
