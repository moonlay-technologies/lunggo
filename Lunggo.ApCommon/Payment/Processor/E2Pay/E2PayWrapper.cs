using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Environment;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.Framework.Redis;
using RestSharp;


namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class E2PayWrapper
        {
            private static readonly E2PayWrapper Instance = new E2PayWrapper();
            private bool _isInitialized;

            private static readonly string PaymentEndpoint = EnvVariables.Get("e2pay", "paymentEndPoint");
            private static readonly string RequeryEndpoint = EnvVariables.Get("e2pay", "requeryEndPoint");
            private static readonly string MerchantCode = EnvVariables.Get("e2pay", "merchantCode");
            private static readonly string MerchantKey = EnvVariables.Get("e2pay", "merchantKey");
            private const string _responsePath = @"/id/E2Pay/ResponsePage";
            private const string _backendPath = @"/id/E2Pay/BackendPost";
            private const string _paymentPagePath = @"/id/Payment/OnlineDebit";

            internal PaymentDetails ProcessPayment(PaymentDetails payment, TransactionDetails transactionDetail)
            {
                var timeout = int.Parse(EnvVariables.Get("flight", "paymentTimeout"));
                var requestHtml = GenerateRequestPageHtml(payment, transactionDetail);
                var paymentUrl = SavePaymentHtmlToCache(requestHtml, transactionDetail.Id, timeout);
                payment.Status = PaymentStatus.Pending;
                payment.RedirectionUrl = paymentUrl;
                return payment;

                //var rootUrl = EnvVariables.Get("general", "rootUrl");
                //if (payment.Method != PaymentMethod.CreditCard && payment.Method != PaymentMethod.VirtualAccount &&
                //    payment.Method != PaymentMethod.BankTransfer)
                //{
                //    var client = new RestClient(_endpointBase);
                //    var request = new RestRequest(_paymentEndpointPath, Method.POST);
                //    if (EnvVariables.Get("general", "environment") == "production")
                //        request.AddHeader("Referer", rootUrl);
                //    else
                //        request.AddHeader("Referer", "qa.travorama.com");
                //    var contact = transactionDetail.Contact;
                //    var rqParam = new PaymentRequest
                //    {
                //        MerchantCode = _merchantCode,
                //        PaymentId = MapPaymentMethod(payment.Method),
                //        RefNo = transactionDetail.RsvNo,
                //        Amount = (long)payment.FinalPriceIdr * 100,
                //        Currency = "IDR",
                //        ProdDesc = "Pembayaran Travorama No. Pesanan " + transactionDetail.RsvNo,
                //        UserName = contact.Name,
                //        UserEmail = contact.Email,
                //        UserContact = contact.CountryCallingCode + contact.Phone,
                //        Signature = CreateRequestSignature(transactionDetail.RsvNo, (long)payment.FinalPriceIdr, "IDR"),
                //        ResponseURL = rootUrl + _responsePath,
                //        BackendURL = rootUrl + _backendPath
                //    };
                //    request.AddObject(rqParam);
                //    var response = client.Execute(request);
                //    if (response.StatusCode != HttpStatusCode.OK)
                //    {
                //        payment.Status = PaymentStatus.Failed;
                //        payment.FailureReason = FailureReason.PaymentFailure;

                //        var log = LogService.GetInstance();
                //        var env = EnvVariables.Get("general", "environment");
                //        log.Post(
                //            "```Payment Log```"
                //            + "\n`*Environment :* " + env.ToUpper()
                //            + "\n*PAYMENT DETAILS :*\n"
                //            + payment.Serialize()
                //            + "\n*TRANSAC DETAILS :*\n"
                //            + transactionDetail.Serialize()
                //            + "\n*REQUEST :*\n"
                //            + rqParam.Serialize()
                //            + "\n*RESPONSE :*\n"
                //            + response.ErrorMessage
                //            + "\n*Error Location Code :*\n"
                //            + "9985081b-4448-47e9-9220-ec0f9627f968"
                //            + "\n*Platform :* "
                //            + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId()),
                //            env == "production" ? "#logging-prod" : "#logging-dev");

                //        return payment;
                //    }

                //    var responseContent = response.Content;
                //    if (!responseContent.Contains("PaymentCancel"))
                //    {
                //        payment.Status = PaymentStatus.Failed;
                //        payment.FailureReason = FailureReason.PaymentFailure;

                //        var log = LogService.GetInstance();
                //        var env = EnvVariables.Get("general", "environment");
                //        log.Post(
                //            "```Payment Log```"
                //            + "\n`*Environment :* " + env.ToUpper()
                //            + "\n*PAYMENT DETAILS :*\n"
                //            + payment.Serialize()
                //            + "\n*TRANSAC DETAILS :*\n"
                //            + transactionDetail.Serialize()
                //            + "\n*REQUEST :*\n"
                //            + rqParam.Serialize()
                //            + "\n*RESPONSE CONTENT :*\n"
                //            + responseContent
                //            + "\n*Error Location Code :*\n"
                //            + "3ea07625-7a2a-405e-bb8c-5f55cc56d21c"
                //            + "\n*Platform :* "
                //            + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId()),
                //            env == "production" ? "#logging-prod" : "#logging-dev");

                //        return payment;
                //    }

                //    var timeout = int.Parse(EnvVariables.Get("flight", "paymentTimeout"));
                //    var csHtml = (CQ) responseContent;
                //    var externalId = csHtml["input[name='billReferenceNo']"].Attr("value");
                //    var paymentUrl = SavePaymentHtmlToCache(responseContent, transactionDetail.RsvNo, timeout);
                //    payment.Status = PaymentStatus.Pending;
                //    payment.RedirectionUrl = paymentUrl;
                //    payment.ExternalId = externalId;
                //    return payment;
                //}
                //else
                //{
                //    payment.Status = PaymentStatus.Failed;
                //    payment.FailureReason = FailureReason.PaymentFailure;

                //    var log = LogService.GetInstance();
                //    var env = EnvVariables.Get("general", "environment");
                //    log.Post(
                //        "```Payment Log```"
                //        + "\n`*Environment :* " + env.ToUpper()
                //        + "\n*PAYMENT DETAILS :*\n"
                //        + payment.Serialize()
                //        + "\n*TRANSAC DETAILS :*\n"
                //        + transactionDetail.Serialize()
                //        + "\n*METHOD :*\n"
                //        + payment.Method
                //        + "\n*Error Location Code :*\n"
                //        + "b871fd57-747b-427a-b7c9-7c17a88b1024"
                //        + "\n*Platform :* "
                //        + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId()),
                //        env == "production" ? "#logging-prod" : "#logging-dev");

                //    return payment;
            }

            private static string GenerateRequestPageHtml(PaymentDetails payment, TransactionDetails transactionDetail)
            {
                var rootUrl = EnvVariables.Get("general", "rootUrl");
                var contact = transactionDetail.Contact;
                var html =
                    string.Format(
                        "<form id=\"post-redirector\" action=\"" + PaymentEndpoint + "\" method=\"post\">\n" +
                        "<input type=\"hidden\" name=\"Amount\" value=\"" + (long)payment.FinalPriceIdr * 100 +
                        "\" />\n" +
                        "<input type=\"hidden\" name=\"RefNo\" value=\"" + transactionDetail.Id + "\" />\n" +
                        "<input type=\"hidden\" name=\"ProdDesc\" value=\"" + "Pembayaran Travorama No. Pesanan " +
                        transactionDetail.Id + "\" />\n" +
                        "<input type=\"hidden\" name=\"UserName\" value=\"" + contact.Name + "\" />\n" +
                        "<input type=\"hidden\" name=\"UserEmail\" value=\"" + contact.Email + "\" />\n" +
                        "<input type=\"hidden\" name=\"UserContact\" value=\"" + contact.CountryCallingCode +
                        contact.Phone + "\" />\n" +
                        "<input type=\"hidden\" name=\"ResponseURL\" value=\"" + rootUrl + _responsePath + "\" />\n" +
                        "<input type=\"hidden\" name=\"BackendURL\" value=\"" + rootUrl + _backendPath + "\" />\n" +
                        "<input type=\"hidden\" name=\"PaymentId\" value=\"" + MapPaymentMethod(payment.Method) +
                        "\" />\n" +
                        "<input type=\"hidden\" name=\"MerchantCode\" value=\"" + MerchantCode + "\" />\n" +
                        "<input type=\"hidden\" name=\"Currency\" value=\"" + "IDR" + "\" />\n" +
                        "<input type=\"hidden\" name=\"Signature\" value=\"" +
                        CreateRequestSignature(transactionDetail.Id, (long)payment.FinalPriceIdr, "IDR") +
                        "\" />\n" +
                        "</form>\n" +
                        "<script type=\"text/javascript\">\n" +
                        "document.getElementById('post-redirector').submit();\n" +
                        "</script>");
                return html;
            }

            private string SavePaymentHtmlToCache(string responseContent, string rsvNo, int timeout)
            {
                var redisService = RedisService.GetInstance();
                var guid = Guid.NewGuid().ToString("N");
                var redisKey = "e2pay:paymentPage:" + guid;
                var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
                for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
                {
                    try
                    {
                        redisDb.StringSet(redisKey, responseContent,
                            DateTime.UtcNow.AddMinutes(timeout) - DateTime.UtcNow);
                    }
                    catch
                    {
                        if (i >= ApConstant.RedisMaxRetry)
                            throw;
                    }
                }

                var rootUrl = EnvVariables.Get("general", "rootUrl");
                var paymentUrl = rootUrl + _paymentPagePath + "?rsvNo=" + rsvNo + "&id=" + guid + "&medium=" +
                                 (int)PaymentMedium.E2Pay;
                return paymentUrl;
            }

            private static int MapPaymentMethod(PaymentMethod method)
            {
                switch (method)
                {
                    case PaymentMethod.CimbClicks:
                        return 7;
                    case PaymentMethod.MandiriClickPay:
                        return 4;
                    case PaymentMethod.XlTunai:
                        return 9;
                    case PaymentMethod.TelkomselTcash:
                        return 10;
                    case PaymentMethod.IbMuamalat:
                        return 11;
                    case PaymentMethod.EpayBri:
                        return 12;
                    case PaymentMethod.DanamonOnlineBanking:
                        return 13;
                    case PaymentMethod.IndosatDompetku:
                        return 14;
                    case PaymentMethod.SbiiOnlineShopping:
                        return 16;
                    case PaymentMethod.BcaKlikpay:
                        return 18;
                    case PaymentMethod.DooEtQnb:
                        return 19;
                    case PaymentMethod.BtnMobileBanking:
                        return 22;
                    default:
                        return 0;
                }
            }

            private static string CreateRequestSignature(string rsvNo, long amount, string currency)
            {
                var plain = MerchantKey + MerchantCode + rsvNo + (amount * 100) + currency;
                var hashed = plain.Sha1Base64Encode();
                return hashed;
            }
        }
    }
}