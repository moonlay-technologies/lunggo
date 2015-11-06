using System;
using System.IO;
using System.Security.Cryptography;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Util;
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
            var streamKey = new MemoryStream();
            streamKey.WriteIntoStream(rawKey);
            var signatureKey = SHA512.Create().ComputeHash(streamKey).ToString();

            if (notif.signature_key != signatureKey)
                return null;

            if ((notif.status_code == "200") || (notif.status_code == "201") || (notif.status_code == "202"))
            {
                DateTime? time;
                if (notif.transaction_time != null)
                    time = DateTime.Parse(notif.transaction_time);
                else
                    time = null;

                var paymentInfo = new PaymentInfo
                {
                    Medium = PaymentMedium.Veritrans,
                    Method = MapPaymentMethod(notif),
                    Status = MapPaymentStatus(notif),
                    Time = time,
                    Id = notif.approval_code,
                    TargetAccount = notif.permata_va_number,
                    FinalPrice = notif.gross_amount
                };

                if (notif.order_id.IsFlightRsvNo())
                {
                    var flight = FlightService.GetInstance();
                    flight.UpdateFlightPayment(notif.order_id, paymentInfo);
                }
            }
            return null;
        }

        public ActionResult PaymentFinish(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            if (rsvNo.IsFlightRsvNo())
            {
                var flight = FlightService.GetInstance();
                flight.UpdateFlightPayment(rsvNo, new PaymentInfo {Status = PaymentStatus.Verifying});
                return RedirectToAction("Thankyou", "Flight", new {RsvNo = rsvNo});
            }
            else
                return Redirect("/");
        }

        public ActionResult PaymentUnfinish(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            if (rsvNo.IsFlightRsvNo())
            {
                var flight = FlightService.GetInstance();
                flight.UpdateFlightPayment(rsvNo, new PaymentInfo { Status = PaymentStatus.Expired });
                return RedirectToAction("Thankyou", "Flight", new { RsvNo = rsvNo });
            }
            else
                return Redirect("/");
        }

        public ActionResult PaymentError(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            if (rsvNo.IsFlightRsvNo())
            {
                var flight = FlightService.GetInstance();
                flight.UpdateFlightPayment(rsvNo, new PaymentInfo { Status = PaymentStatus.Expired });
                return RedirectToAction("Thankyou", "Flight", new { RsvNo = rsvNo });
            }
            else
                return Redirect("/");
        }

        private static PaymentMethod MapPaymentMethod(VeritransNotification notif)
        {
            switch (notif.payment_type.ToLower())
            {
                case "credit_card":
                    return PaymentMethod.CreditCard;
                case "bank_transfer":
                    return PaymentMethod.BankTransfer;
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
                        case "capture":
                            return PaymentStatus.Settled;
                        case "challenge":
                        case "deny":
                            return PaymentStatus.Denied;
                        default:
                            return PaymentStatus.Settled;
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
                    return PaymentStatus.Undefined;
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