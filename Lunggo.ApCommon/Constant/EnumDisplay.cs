using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Context;
using Lunggo.Framework.I18nMessage;

namespace Lunggo.ApCommon.Constant
{
    public static class EnumDisplay
    {
        private static readonly MessageManager Msg = MessageManager.GetInstance();

        public static string Title(Title title)
        {
            var lang = "en_US";
            try
            {
                lang = LangToIsoCodeMapper.GetIsoCode(OnlineContext.GetActiveLanguageCode());
            }
            catch { }
            switch (title)
            {
                case Product.Constant.Title.Mister:
                    return Msg.GetMessageValue("AC00001", lang);
                case Product.Constant.Title.Mistress:
                    return Msg.GetMessageValue("AC00002", lang);
                case Product.Constant.Title.Miss:
                    return Msg.GetMessageValue("AC00003", lang);
                default:
                    return "";
            }
        }

        public static string PassengerType(PaxType type)
        {
            var lang = "en_US";
            try
            {
                lang = LangToIsoCodeMapper.GetIsoCode(OnlineContext.GetActiveLanguageCode());
            }
            catch { }
            switch (type)
            {
                case Flight.Constant.PaxType.Adult:
                    return Msg.GetMessageValue("AC00025", lang);
                case Flight.Constant.PaxType.Child:
                    return Msg.GetMessageValue("AC00026", lang);
                case Flight.Constant.PaxType.Infant:
                    return Msg.GetMessageValue("AC00027", lang);
                default:
                    return "";
            }
        }

        public static string PaymentMethod(PaymentMethod method)
        {
            var lang = "en_US";
            try
            {
                lang = LangToIsoCodeMapper.GetIsoCode(OnlineContext.GetActiveLanguageCode());
            }
            catch { }
            switch (method)
            {
                case Payment.Constant.PaymentMethod.BankTransfer:
                    return Msg.GetMessageValue("AC00004", lang);
                case Payment.Constant.PaymentMethod.CreditCard:
                    return Msg.GetMessageValue("AC00005", lang);
                case Payment.Constant.PaymentMethod.VirtualAccount:
                    return Msg.GetMessageValue("AC00006", lang);
                case Payment.Constant.PaymentMethod.MandiriClickPay:
                    return Msg.GetMessageValue("AC00007", lang);
                case Payment.Constant.PaymentMethod.CimbClicks:
                    return Msg.GetMessageValue("AC00008", lang);
                case Payment.Constant.PaymentMethod.BcaKlikpay:
                    return Msg.GetMessageValue("AC00009", lang);
                case Payment.Constant.PaymentMethod.EpayBri:
                    return Msg.GetMessageValue("AC00010", lang);
                case Payment.Constant.PaymentMethod.TelkomselTcash:
                    return Msg.GetMessageValue("AC00011", lang);
                case Payment.Constant.PaymentMethod.XlTunai:
                    return Msg.GetMessageValue("AC00012", lang);
                case Payment.Constant.PaymentMethod.BbmMoney:
                    return Msg.GetMessageValue("AC00013", lang);
                case Payment.Constant.PaymentMethod.IndosatDompetku:
                    return Msg.GetMessageValue("AC00014", lang);
                case Payment.Constant.PaymentMethod.MandiriEcash:
                    return Msg.GetMessageValue("AC00015", lang);
                case Payment.Constant.PaymentMethod.MandiriBillPayment:
                    return Msg.GetMessageValue("AC00016", lang);
                case Payment.Constant.PaymentMethod.Indomaret:
                    return Msg.GetMessageValue("AC00017", lang);
                default:
                    return "";
            }
        }

        public static string PaymentStatus(PaymentStatus status)
        {
            var lang = "en_US";
            try
            {
                lang = LangToIsoCodeMapper.GetIsoCode(OnlineContext.GetActiveLanguageCode());
            }
            catch { }
            switch (status)
            {
                case Payment.Constant.PaymentStatus.Pending:
                    return Msg.GetMessageValue("AC00018", lang);
                case Payment.Constant.PaymentStatus.Settled:
                    return Msg.GetMessageValue("AC00019", lang);
                case Payment.Constant.PaymentStatus.Denied:
                    return Msg.GetMessageValue("AC00020", lang);
                case Payment.Constant.PaymentStatus.Expired:
                    return Msg.GetMessageValue("AC00021", lang);
                case Payment.Constant.PaymentStatus.Verifying:
                    return Msg.GetMessageValue("AC00022", lang);
                case Payment.Constant.PaymentStatus.Challenged:
                    return Msg.GetMessageValue("AC00023", lang);
                case Payment.Constant.PaymentStatus.Cancelled:
                    return Msg.GetMessageValue("AC00024", lang);
                default:
                    return "";
            }
        }
    }
}
