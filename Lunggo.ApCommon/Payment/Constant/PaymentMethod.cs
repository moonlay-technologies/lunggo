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
        Indomaret = 14
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
                default:
                    return PaymentMethod.Undefined;
            }
        }
    }
}
