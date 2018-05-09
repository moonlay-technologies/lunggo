using System;

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
        //Credit = 15,
        //Deposit = 16,
        IbMuamalat = 17,
        DanamonOnlineBanking = 18,
        SbiiOnlineShopping = 19,
        DooEtQnb = 20,
        BtnMobileBanking = 21
    }

    public class PaymentMethodCd
    {
        public static string Mnemonic(PaymentMethod paymentMethod)
        {
            switch (paymentMethod)
            {
                case PaymentMethod.Undefined:
                    return null;
                case PaymentMethod.CreditCard:
                    return "CREDITCARD";
                case PaymentMethod.BankTransfer:
                    return "BANKTRANSFER";
                case PaymentMethod.MandiriClickPay:
                    return "MANDIRICLICKPAY";
                case PaymentMethod.CimbClicks:
                    return "CIMBCLICKS";
                case PaymentMethod.VirtualAccount:
                    return "VIRTUALACCOUNT";
                case PaymentMethod.BcaKlikpay:
                    return "BCAKLIKPAY";
                case PaymentMethod.EpayBri:
                    return "EPAYBRI";
                case PaymentMethod.TelkomselTcash:
                    return "TELKOMSELTCASH";
                case PaymentMethod.XlTunai:
                    return "XLTUNAI";
                case PaymentMethod.BbmMoney:
                    return "BBMMONEY";
                case PaymentMethod.IndosatDompetku:
                    return "INDOSATDOMPETKU";
                case PaymentMethod.MandiriEcash:
                    return "MANDIRIECASH";
                case PaymentMethod.MandiriBillPayment:
                    return "MANDIRIBILLPAYMENT";
                case PaymentMethod.Indomaret:
                    return "INDOMARET";
                //case PaymentMethod.Credit:
                //    return "Credit";
                //case PaymentMethod.Deposit:
                //    return "DPS";
                case PaymentMethod.IbMuamalat:
                    return "IBMUAMALAT";
                case PaymentMethod.DanamonOnlineBanking:
                    return "DANAMONONLINEBANKING";
                case PaymentMethod.SbiiOnlineShopping:
                    return "SBIIONLINESHOPPING";
                case PaymentMethod.DooEtQnb:
                    return "DOOETQNB";
                case PaymentMethod.BtnMobileBanking:
                    return "BTNMOBILEBANKING";
                default:
                    throw new ArgumentException("Payment method not implemented: " + paymentMethod);
            }
        }
        public static PaymentMethod Mnemonic(string paymentMethod)
        {
            switch (paymentMethod)
            {
                case "CRC":
                case "CREDITCARD":
                    return PaymentMethod.CreditCard;
                case "TRF":
                case "BANKTRANSFER":
                    return PaymentMethod.BankTransfer;
                case "MCP":
                case "MANDIRICLICKPAY":
                    return PaymentMethod.MandiriClickPay;
                case "CCL":
                case "CIMBCLICKS":
                    return PaymentMethod.CimbClicks;
                case "VIR":
                case "VIRTUALACCOUNT":
                    return PaymentMethod.VirtualAccount;
                case "BKP":
                case "BCAKLIKPAY":
                    return PaymentMethod.BcaKlikpay;
                case "EPB":
                case "EPAYBRI":
                    return PaymentMethod.EpayBri;
                case "TTC":
                case "TELKOMSELTCASH":
                    return PaymentMethod.TelkomselTcash;
                case "XLT":
                case "XLTUNAI":
                    return PaymentMethod.XlTunai;
                case "BBM":
                case "BBMMONEY":
                    return PaymentMethod.BbmMoney;
                case "IND":
                case "INDOSATDOMPETKU":
                    return PaymentMethod.IndosatDompetku;
                case "MEC":
                case "MANDIRIECASH":
                    return PaymentMethod.MandiriEcash;
                case "MBP":
                case "MANDIRIBILLPAYMENT":
                    return PaymentMethod.MandiriBillPayment;
                case "IDM":
                case "INDOMARET":
                    return PaymentMethod.Indomaret;
                //case "CRD":
                //    return PaymentMethod.Credit;
                //case "DPS":
                //    return PaymentMethod.Deposit;
                case "IBM":
                case "IBMUAMALAT":
                    return PaymentMethod.IbMuamalat;
                case "DOB":
                case "DANAMONONLINEBANKING":
                    return PaymentMethod.DanamonOnlineBanking;
                case "SBI":
                case "SBIIONLINESHOPPING":
                    return PaymentMethod.SbiiOnlineShopping;
                case "QNB":
                case "DOOETQNB":
                    return PaymentMethod.DooEtQnb;
                case "BTN":
                case "BTNMOBILEBANKING":
                    return PaymentMethod.BtnMobileBanking;
                case "":
                case null:
                    return PaymentMethod.Undefined;
                default:
                    throw new ArgumentException("Payment method not implemented: " + paymentMethod);
            }
        }
    }
}
