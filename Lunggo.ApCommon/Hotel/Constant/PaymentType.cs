using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
namespace Lunggo.ApCommon.Hotel.Constant
{
    public enum PaymentTypeEnum
    {
        AT_HOTEL,
        AT_WEB,
        UNDEFINED
    }
    internal class PaymentTypeCd
    {
        internal static SimpleTypes.PaymentType Mnemonic(PaymentTypeEnum paymentType)
        {
            switch (paymentType)
            {
                case PaymentTypeEnum.AT_HOTEL:
                    return SimpleTypes.PaymentType.AT_HOTEL;
                case PaymentTypeEnum.AT_WEB:
                    return SimpleTypes.PaymentType.AT_WEB;
                default:
                    return SimpleTypes.PaymentType.AT_HOTEL;
            }
        }

        internal static PaymentTypeEnum Mnemonic(SimpleTypes.PaymentType paymentType)
        {
            switch (paymentType)
            {
                case SimpleTypes.PaymentType.AT_HOTEL:
                    return PaymentTypeEnum.AT_HOTEL;
                case SimpleTypes.PaymentType.AT_WEB:
                    return PaymentTypeEnum.AT_WEB;
                default:
                    return PaymentTypeEnum.UNDEFINED;
            }
        }

        internal static PaymentTypeEnum Mnemonic(string paymentType)
        {
            switch (paymentType)
            {
                case "AT_HOTEL":
                    return PaymentTypeEnum.AT_HOTEL;
                case "AT_WEB":
                    return PaymentTypeEnum.AT_WEB;
                default:
                    return PaymentTypeEnum.UNDEFINED;
            }
        }

        internal static string MnemonicToString(PaymentTypeEnum paymentType)
        {
            switch (paymentType)
            {
                case PaymentTypeEnum.AT_HOTEL:
                    return "AT_HOTEL";
                case PaymentTypeEnum.AT_WEB:
                    return "AT_WEB";
                default:
                    return null;
            }
        }
    }
}
