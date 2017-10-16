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
using Lunggo.Framework.Extension;
using Newtonsoft.Json;

namespace Lunggo.CustomerWeb.Controllers
{
    public class E2PayController : Controller
    {
        [HttpPost]
        public string PaymentNotification()
        {
            var form = Request.Form;

            var signatureKey = CreateSignature(form["PaymentId"], form["RefNo"], form["Amount"], form["Currency"], form["Status"]);

            if (form["Signature"] != signatureKey)
                return null;

            if (form["Status"] == "1")
            {
                var paymentInfo = new PaymentDetails
                {
                    Medium = PaymentMedium.E2Pay,
                    Method = MapPaymentMethod(form["PaymentId"]),
                    Status = PaymentStatus.Settled,
                    Time = DateTime.UtcNow,
                    ExternalId = form["TransId"],
                    FinalPriceIdr = decimal.Parse(form["Amount"])/100,
                    LocalCurrency = new Currency("IDR")
                };
                PaymentService.GetInstance().UpdatePayment(form["RefNo"], paymentInfo);
            }
            return "OK";
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
                    return PaymentMethod.OnlineSbi;
                case "18":
                    return PaymentMethod.BcaKlikpay;
                case "19":
                    return PaymentMethod.Qnb;
                case "22":
                    return PaymentMethod.Btn;
                default:
                    return PaymentMethod.Undefined;
            }
        }

        private static string CreateSignature(string paymentId, string rsvNo, string amount, string currency, string status)
        {
            var merchantKey = ConfigManager.GetInstance().GetConfigValue("e2pay", "merchantKey");
            var merchantCode = ConfigManager.GetInstance().GetConfigValue("e2pay", "merchantCode");
            var plain = merchantKey + merchantCode + paymentId + rsvNo + amount + currency + status;
            var hashed = plain.Sha1Encode();
            return hashed;
        }
    }
}