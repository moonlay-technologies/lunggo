using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.Repository.TableRecord;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.CustomerWeb.Controllers
{
    public class VeritransController : Controller
    {
        public PaymentStatus HandleNotification(string rsvNoOrExternalId)
        {
            var path = string.Join("/", "v2", rsvNoOrExternalId, "status");
            var client = new RestClient(ConfigManager.GetInstance().GetConfigValue("veritrans", "endPoint"));
            var rq = new RestRequest(path, Method.GET);
            var serverKey = ConfigManager.GetInstance().GetConfigValue("veritrans", "serverKey");
            var authorizationKey = ProcessAuthorizationKey(serverKey + ":");
            rq.AddHeader("Authorization", "Basic " + authorizationKey);
            rq.AddHeader("ContentType", "application/json");
            rq.AddHeader("Accept", "application/json");
            var rs = client.Execute(rq);
            if (rs.StatusCode != HttpStatusCode.OK)
            {
                return ReturnAndLogFailure(rs.Content);
            }

            var notif = JsonConvert.DeserializeObject<VeritransNotification>(rs.Content);

            
            var rawKey = notif.order_id + notif.status_code + notif.gross_amount + serverKey;
            var signatureKey = rawKey.Sha512Encode();

            if (notif.signature_key != signatureKey)
            {
                return ReturnAndLogFailure(rs.Content);
            }

            if ((notif.status_code == "200") || (notif.status_code == "201") || (notif.status_code == "202") || (notif.status_code == "407"))
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
                    ExternalId = notif.transaction_id,
                    TransferAccount = notif.permata_va_number,
                    FinalPriceIdr = notif.gross_amount,
                    LocalCurrency = new Currency("IDR")
                };

                if (paymentInfo.Status != PaymentStatus.Failed && paymentInfo.Status != PaymentStatus.Denied)
                    PaymentService.GetInstance().UpdatePayment(notif.order_id, paymentInfo);

                return status;
            }

            return ReturnAndLogFailure(rs.Content);
        }

        private static string ProcessAuthorizationKey(string serverKey)
        {
            var plainAuthorizationKey = Encoding.UTF8.GetBytes(serverKey);
            var hashedAuthorizationKey = Convert.ToBase64String(plainAuthorizationKey);
            return hashedAuthorizationKey;
        }

        private PaymentStatus ReturnAndLogFailure(string content)
        {
            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            log.Post(
                "```Veritrans Payment Notif Log```"
                + "\n`*Environment :* " + env.ToUpper()
                + "\n*NOTIF DETAILS :*\n"
                + content
                //+ "\n*ITEM DETAILS :*\n"
                //+ itemDetails.Serialize()
                + "\n*RESPONSE :*\n"
                + content.Serialize()
                + "\n*Platform :* "
                + Client.GetPlatformType(User.Identity.GetClientId()),
                env == "production" ? "#logging-prod" : "#logging-dev");

            return PaymentStatus.Undefined;
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult PaymentNotification()
        {
            string notifJson;
            using (var rqStream = new StreamReader(Request.InputStream))
                notifJson = rqStream.ReadToEnd();
            var notif = JsonConvert.DeserializeObject<VeritransNotification>(notifJson);

            HandleNotification(notif.order_id);

            return new EmptyResult();
        }


        public ActionResult PaymentFinish(VeritransResponse response = null, string id = null)
        {
            if (id != null)
                return PaymentFinishPost(null, id);

            var rsvNo = response.order_id;
            if (response.status_code == "202")
            {
                return RedirectToAction("Payment", "Payment", new { RsvNo = rsvNo, regId = new PaymentController().GenerateId(rsvNo) });
            }
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Verifying });
            return RedirectToAction("Thankyou", "Payment", new { RsvNo = rsvNo, regId = new PaymentController().GenerateId(rsvNo) });
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("PaymentFinish")]
        public ActionResult PaymentFinishPost(string response, [FromUri] string id = null)
        {
            string rsvNoOrExternalId;
            string rsvNo;

            if (!string.IsNullOrEmpty(id))
            {
                rsvNoOrExternalId = id;
                using (var conn = DbService.GetInstance().GetOpenConnection())
                    rsvNo = GetRsvNoFromExternalIdQuery.GetInstance().Execute(conn, new { id }).Single();
            }
            else
            {
                var response2 = response.Deserialize<VeritransNotification>();
                rsvNoOrExternalId = response2.order_id;
                rsvNo = response2.order_id;
            }
            var paymentStatus = HandleNotification(rsvNoOrExternalId);
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = paymentStatus });
            if (paymentStatus == PaymentStatus.Denied || paymentStatus == PaymentStatus.Expired || paymentStatus == PaymentStatus.Failed)
            {
                TempData["PaymentFailed"] = true;
                return RedirectToAction("Payment", "Payment", new { RsvNo = rsvNo, regId = new PaymentController().GenerateId(rsvNo) });
            }
            
            return RedirectToAction("Thankyou", "Payment", new { RsvNo = rsvNo, regId = new PaymentController().GenerateId(rsvNo) });
        }


        public ActionResult PaymentUnfinish(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Expired });
            return RedirectToAction("Thankyou", "Payment", new { RsvNo = rsvNo, regId = new PaymentController().GenerateId(rsvNo) });
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("PaymentUnfinish")]
        public ActionResult PaymentUnfinishPost(string response)
        {
            return PaymentFinishPost(response);
        }

        public ActionResult PaymentError(VeritransResponse response)
        {
            var rsvNo = response.order_id;
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Expired });
            return RedirectToAction("Thankyou", "Payment", new { RsvNo = rsvNo, regId = new PaymentController().GenerateId(rsvNo) });
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("PaymentError")]
        public ActionResult PaymentErrorPost(string response)
        {
            return PaymentFinishPost(response);
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.ActionName("PaymentComplete")]
        public ActionResult PaymentCompletePost(string response)
        {
            return PaymentFinishPost(response);
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
                case "bca_klikpay":
                    return PaymentMethod.BcaKlikpay;
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
                    return MapPaymentMethod(notif) == PaymentMethod.BcaKlikpay 
                        ? PaymentStatus.Failed 
                        : PaymentStatus.Pending;
                case "expire":
                    return PaymentStatus.Expired;
                case "deny":
                    return PaymentStatus.Denied;
                case "authorize":
                case "cancel":
                default:
                    return PaymentStatus.Failed;
            }
        }
    }

    public class VeritransNotification
    {
        public string status_code { get; set; }
        public string status_message { get; set; }
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_time { get; set; }
        public string transaction_status { get; set; }
        public string transaction_id { get; set; }
        public string fraud_status { get; set; }
        public string approval_code { get; set; }
        public string bank { get; set; }
        public string permata_va_number { get; set; }
        public string masked_card { get; set; }
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

    internal class GetRsvNoFromExternalIdQuery : DbQueryBase<GetRsvNoFromExternalIdQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT RsvNo FROM Payment WHERE MediumCd = 'VERI' AND ExternalId = @id";
        }
    }
}