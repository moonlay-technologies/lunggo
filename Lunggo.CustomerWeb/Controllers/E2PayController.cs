using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Newtonsoft.Json;
using Lunggo.ApCommon.Log;

namespace Lunggo.CustomerWeb.Controllers
{
    public class E2PayController : Controller
    {

        [HttpPost]
        public ActionResult ResponsePage()
        {
            var TableLog = new GlobalLog();
            
            TableLog.PartitionKey = "E2PAY RESPONSE PAGE LOG";
            

            var form = Request.Form;

            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            TableLog.Log = "```E2Pay Response Page```"
                + "\n`*Environment :* " + env.ToUpper()
                + "\n*FORM :*\n"
                + form
                + "\n*Platform :* "
                + Client.GetPlatformType(System.Web.HttpContext.Current.User.Identity.GetClientId());
            log.Post(TableLog.Log
                ,
                env == "production" ? "#logging-prod" : "#logging-dev");
            TableLog.Logging();

            var isSuccess = ProcessResponse(form);
            var param = new { rsvNo = form["RefNo"], regId = new PaymentController().GenerateId(form["RefNo"]) };
            return RedirectToAction(isSuccess ? "Thankyou" : "Payment", "Payment", param);
        }

        [HttpPost]
        public string BackendPost()
        {
            var TableLog = new GlobalLog();
            
            TableLog.PartitionKey = "E2PAY RESPONSE PAGE LOG";
            
            var form = Request.Form;

            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");

            TableLog.Log = "```E2Pay Backend Post```"
                + "\n`*Environment :* " + env.ToUpper()
                + "\n*FORM :*\n"
                + form
                + "\n*Platform :* "
                + Client.GetPlatformType(System.Web.HttpContext.Current.User.Identity.GetClientId());
            log.Post(TableLog.Log
                ,
                env == "production" ? "#logging-prod" : "#logging-dev");
            TableLog.Logging();

            var isSuccess = ProcessResponse(form);
            return isSuccess ? "OK" : "Failed";
        }

        private static bool ProcessResponse(NameValueCollection form)
        {
            var signatureKey = CreateSignature(form["PaymentId"], form["RefNo"], form["Amount"], form["Currency"], form["Status"]);

            if (!string.IsNullOrEmpty(form["Signature"]) && form["Signature"] != signatureKey)
                return false;

            if (form["Status"] != "1")
            {
                var payment = new PaymentDetails
                {
                    Medium = PaymentMedium.E2Pay,
                    Method = MapPaymentMethod(form["PaymentId"]),
                    Status = PaymentStatus.Failed,
                    Time = DateTime.UtcNow,
                    ExternalId = form["TransId"],
                    RedirectionUrl = null,
                    FinalPriceIdr = decimal.Parse(form["Amount"]) / 100,
                    LocalCurrency = new Currency("IDR")
                };
                PaymentService.GetInstance().UpdatePayment(form["RefNo"], payment);
                return false;
            }

            var paymentInfo = new PaymentDetails
            {
                Medium = PaymentMedium.E2Pay,
                Method = MapPaymentMethod(form["PaymentId"]),
                Status = PaymentStatus.Settled,
                Time = DateTime.UtcNow,
                ExternalId = form["TransId"],
                FinalPriceIdr = decimal.Parse(form["Amount"]) / 100,
                LocalCurrency = new Currency("IDR")
            };
            PaymentService.GetInstance().UpdatePayment(form["RefNo"], paymentInfo);
            return true;
        }

        private static PaymentMethod MapPaymentMethod(string id)
        {
            switch (id)
            {
                case "7":
                    return PaymentMethod.CimbClicks;
                case "4":
                    return PaymentMethod.MandiriClickPay;
                case "9":
                    return PaymentMethod.XlTunai;
                case "10":
                    return PaymentMethod.TelkomselTcash;
                case "11":
                    return PaymentMethod.IbMuamalat;
                case "12":
                    return PaymentMethod.EpayBri;
                case "13":
                    return PaymentMethod.DanamonOnlineBanking;
                case "14":
                    return PaymentMethod.Indomaret;
                case "16":
                    return PaymentMethod.SbiiOnlineShopping;
                case "18":
                    return PaymentMethod.BcaKlikpay;
                case "19":
                    return PaymentMethod.DooEtQnb;
                case "22":
                    return PaymentMethod.BtnMobileBanking;
                default:
                    return PaymentMethod.Undefined;
            }
        }

        private static string CreateSignature(string paymentId, string rsvNo, string amount, string currency, string status)
        {
            var merchantKey = ConfigManager.GetInstance().GetConfigValue("e2pay", "merchantKey");
            var merchantCode = ConfigManager.GetInstance().GetConfigValue("e2pay", "merchantCode");
            var plain = merchantKey + merchantCode + paymentId + rsvNo + amount + currency + status;
            var hashed = plain.Sha1Base64Encode();
            return hashed;
        }
    }
}