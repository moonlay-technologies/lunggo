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
            //if (notif.resultCd != "0000")
            //{
            //    return new ApiResponseBase
            //    {
            //        StatusCode = HttpStatusCode.BadRequest,
            //        ErrorCode = "ERPPMP01"
            //    };
            //}

            var status = MapNicepayPaymentStatus(notif.status,notif.resultMsg);
            var method = MapNicepayPaymentMethod(notif.payMethod);
            var paymentInfo = new PaymentDetails
            {
                Medium = PaymentMedium.Nicepay,
                Method = method,
                Submethod = MappingNicepayPaymentSubMethod(notif.bankCd),
                Status = status,
                Time = status == PaymentStatus.Settled ? DateTime.UtcNow : (DateTime?) null,
                ExternalId = notif.tXid,
                TransferAccount = notif.vacctNo,
                FinalPriceIdr = notif.amt == null ? 0 : Decimal.Parse(notif.amt),
                LocalCurrency = new Currency("IDR")
            };

            if (paymentInfo.Status != PaymentStatus.Failed && paymentInfo.Status != PaymentStatus.Denied)
                new PaymentService().UpdatePayment(notif.referenceNo, paymentInfo);
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

        public static PaymentSubmethod MappingNicepayPaymentSubMethod(string bankCd)
        {
            switch (bankCd)
            {
                case "CENA":
                    return PaymentSubmethod.BCA;
                case "IBBK":
                    return PaymentSubmethod.Maybank;
                case "BNIN":
                    return PaymentSubmethod.BNI;
                case "BMRI":
                    return PaymentSubmethod.Mandiri;
                case "BBBA":
                    return PaymentSubmethod.Permata;
                case "BNIA":
                    return PaymentSubmethod.CIMB;
                case "BRIN":
                    return PaymentSubmethod.BRI;
                case "BDMN":
                    return PaymentSubmethod.Danamon;
                default:
                    return PaymentSubmethod.Undefined;
            }
        }

        public class NotificationResult
        {
            internal string tXid { get; set; }
            internal string referenceNo { get; set; }
            internal string amt { get; set; }
            internal string merchantToken { get; set; }
            internal string reqTm { get; set; }
            internal string goodsNm { get; set; }
            internal string resultCd { get; set; }
            internal string instmntType { get; set; }
            internal string iMid { get; set; }
            internal string billingNm { get; set; }
            internal string resultMsg { get; set; }
            internal string vacctValidDt { get; set; }
            internal string payMethod { get; set; }
            internal string reqDt { get; set; }
            internal string currency { get; set; }
            internal string instmntMon { get; set; }
            internal string vacctValidTm { get; set; }
            internal string status { get; set; }
            internal string vacctNo { get; set; }
            internal string bankCd { get; set; }
        }
    }
}