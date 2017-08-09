using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Payment.Logic
{
    public static partial class PaymentLogic
    {
        public static ApiResponseBase CheckPaymentNotification(NotificationResult notif)
        {
            if (notif.resultCd != "0000")
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERPPMP01"
                };
            }

            var status = MapNicepayPaymentStatus(notif.status,notif.resultMsg);
            var method = MapNicepayPaymentMethod(notif.payMethod);
            var paymentInfo = new PaymentDetails
            {
                Medium = PaymentMedium.Veritrans,
                Method = method,
                SubMethod = (method == PaymentMethod.BankTransfer || method == PaymentMethod.VirtualAccount) ? MappingNicepayPaymentSubMethod(notif.bankCd) : PaymentSubMethod.Undefined,
                Status = status,
                Time = status == PaymentStatus.Settled ? DateTime.UtcNow : (DateTime?) null,
                ExternalId = notif.tXid,
                TransferAccount = notif.vacctNo,
                FinalPriceIdr = notif.amt == null ? 0 : Decimal.Parse(notif.amt),
                LocalCurrency = new Currency("IDR")
            };

            if (paymentInfo.Status != PaymentStatus.Failed && paymentInfo.Status != PaymentStatus.Denied)
                PaymentService.GetInstance().UpdatePayment(notif.referenceNo, paymentInfo);
            return new ApiResponseBase();
        }


        private static PaymentStatus MapNicepayPaymentStatus(string status, string resultMessage)
        {
            switch (status)
            {
                case "0":
                    return PaymentStatus.Settled; //Paid
                case "1":
                    return PaymentStatus.Challenged; //Reversal
                case "2":
                    return PaymentStatus.Challenged; // Refund
                case "3": // Unpaid/Expired
                    return resultMessage.ToLower().Contains("unpaid") ? PaymentStatus.Pending : PaymentStatus.Expired;
                case "4":
                    return PaymentStatus.Cancelled; //Cancelled
                case "9":
                    return PaymentStatus.Pending; //Initialization
                default:
                    return PaymentStatus.Failed;
            }
        }

        private static PaymentMethod MapNicepayPaymentMethod(string method)
        {
            switch (method)
            {
                case "01":
                    return PaymentMethod.CreditCard; //Credit Card
                case "02":
                    return PaymentMethod.VirtualAccount; // Convenience Store
                case "03":
                    return PaymentMethod.Undefined; // Unpaid/Expired
                case "04":
                    return PaymentMethod.Undefined; //Click Pay
                case "05":
                    return PaymentMethod.Undefined; //E-Wallet
                default:
                    return PaymentMethod.Undefined;
            }
        }

        public  static PaymentSubMethod MappingNicepayPaymentSubMethod(string bankCd)
        {
            switch (bankCd)
            {
                case "CENA":
                    return PaymentSubMethod.BCA;
                case "IBBK":
                    return PaymentSubMethod.Maybank;
                case "BNIN":
                    return PaymentSubMethod.BNI;
                case "BMRI":
                    return PaymentSubMethod.Mandiri;
                case "BBBA":
                    return PaymentSubMethod.Permata;
                case "BNIA":
                    return PaymentSubMethod.CIMB;
                case "BRIN":
                    return PaymentSubMethod.BRI;
                case "BDMN":
                    return PaymentSubMethod.Danamon;
                default:
                    return PaymentSubMethod.Undefined;
            }
        }
    }
}