using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Newtonsoft.Json;

namespace Lunggo.CustomerWeb.Controllers
{
    public class VeritransController : Controller
    {
        [HttpPost]
        public ActionResult PaymentNotification()
        {
            string notifJson;
            using (var rqStream = new StreamReader(Request.InputStream))
                notifJson = rqStream.ReadToEnd();
            var notif = JsonConvert.DeserializeObject<VeritransNotification>(notifJson);

            var serverKey = ConfigManager.GetInstance().GetConfigValue("veritrans", "serverKey");
            var rawKey = notif.order_id + notif.status_code + notif.gross_amount + serverKey;
            var signatureKey = rawKey.Sha512Encode();

            if (notif.signature_key != signatureKey)
                return null;

            if ((notif.status_code == "200") || (notif.status_code == "201") || (notif.status_code == "202"))
            {
                DateTime? time;
                if (notif.transaction_time != null)
                    time = DateTime.Parse(notif.transaction_time).AddHours(-7).ToUniversalTime();
                else
                    time = null;

                var status = MapPaymentStatus(notif);
                var paymentInfo = new PaymentDetails
                {
                    Medium = PaymentMedium.Veritrans,
                    Method = MapPaymentMethod(notif),
                    Status = status,
                    Time = status == PaymentStatus.Settled ? time : null,
                    ExternalId = notif.approval_code,
                    TransferAccount = notif.permata_va_number,
                    FinalPriceIdr = notif.gross_amount,
                    LocalCurrency = new Currency("IDR")
                };

                if (paymentInfo.Status != PaymentStatus.Failed && paymentInfo.Status != PaymentStatus.Denied)
                    PaymentService.GetInstance().UpdatePayment(notif.order_id, paymentInfo);
            }
            return new EmptyResult();
        }

        public ActionResult PaymentFinish(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            var flight = FlightService.GetInstance();
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Verifying });
            TempData["AllowThisThankyouPage"] = rsvNo;
            return RedirectToAction("Thankyou", "Flight", new { RsvNo = rsvNo });
        }

        public ActionResult PaymentUnfinish(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            var flight = FlightService.GetInstance();
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Expired });
            TempData["AllowThisThankyouPage"] = rsvNo;
            return RedirectToAction("Thankyou", "Flight", new { RsvNo = rsvNo });
        }

        public ActionResult PaymentError(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            var flight = FlightService.GetInstance();
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Expired });
            TempData["AllowThisThankyouPage"] = rsvNo;
            return RedirectToAction("Thankyou", "Flight", new { RsvNo = rsvNo });
        }

        private static PaymentMethod MapPaymentMethod(VeritransNotification notif)
        {
            switch (notif.payment_type.ToLower())
            {
                case "credit_card":
                    return PaymentMethod.CreditCard;
                case "bank_transfer":
                    return PaymentMethod.VirtualAccount;
                case "mandiri_clickpay":
                    return PaymentMethod.MandiriClickPay;
                case "cimb_clicks":
                    return PaymentMethod.CimbClicks;
                default:
                    return PaymentMethod.Undefined;
            }
        }

        private static PaymentStatus MapPaymentStatus(VeritransNotification notif)
        {
            switch (notif.transaction_status.ToLower())
            {
                case "capture":
                    switch (notif.fraud_status.ToLower())
                    {
                        case "accept":
                            return PaymentStatus.Settled;
                        case "challenge":
                        case "deny":
                            return PaymentStatus.Denied;
                        default:
                            return PaymentStatus.Denied;
                    }
                case "settlement":
                    return PaymentStatus.Settled;
                case "pending":
                    return PaymentStatus.Pending;
                case "authorize":
                case "cancel":
                case "expire":
                    return PaymentStatus.Expired;
                case "deny":
                    return PaymentStatus.Denied;
                default:
                    return PaymentStatus.Denied;
            }
        }
    }

    public class VeritransNotification
    {
        public string status_code { get; set; }
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_time { get; set; }
        public string transaction_status { get; set; }
        public string fraud_status { get; set; }
        public string approval_code { get; set; }
        public string bank { get; set; }
        public string permata_va_number { get; set; }
        public decimal gross_amount { get; set; }
        public string signature_key { get; set; }
    }

    public class VeritransResponse
    {
        public string merchant_id { get; set; }
        public string order_id { get; set; }
        public string status_code { get; set; }
        public string transaction_status { get; set; }
    }
}