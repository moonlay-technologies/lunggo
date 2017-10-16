using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Wrapper.E2Pay;
using Lunggo.ApCommon.Payment.Wrapper.E2Pay.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using RestSharp;


namespace Lunggo.ApCommon.Payment.Wrapper.E2Pay
{
    internal partial class E2PayWrapper
    {
        private static readonly E2PayWrapper Instance = new E2PayWrapper();
        private bool _isInitialized;

        internal static string _endpoint;
        internal static string _requeryEndpoint;
        internal static string _merchantCode;
        internal static string _merchantKey;

        private E2PayWrapper()
        {

        }

        internal static E2PayWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                _endpoint = ConfigManager.GetInstance().GetConfigValue("e2pay", "endPoint");
                _requeryEndpoint = ConfigManager.GetInstance().GetConfigValue("e2pay", "requeryEndPoint");
                _merchantCode = ConfigManager.GetInstance().GetConfigValue("e2pay", "merchantCode");
                _merchantKey = ConfigManager.GetInstance().GetConfigValue("e2pay", "merchantKey");
                _isInitialized = true;
            }
        }

        internal PaymentDetails ProcessPayment(PaymentDetails payment, TransactionDetails transactionDetail)
        {
            if (payment.Method != PaymentMethod.CreditCard && payment.Method != PaymentMethod.VirtualAccount &&
                payment.Method != PaymentMethod.BankTransfer)
            {
                var client = new RestClient();
                var request = new RestRequest(_endpoint, Method.POST);
                var contact = transactionDetail.Contact;
                var rqParam = new PaymentRequest
                {
                    MerchantCode = _merchantCode,
                    PaymentId = MapPaymentMethod(payment.Method),
                    RefNo = transactionDetail.RsvNo,
                    Amount = (long)payment.FinalPriceIdr * 100,
                    Currency = "IDR",
                    ProdDesc = "", //TODO: payment description
                    UserName = contact.Name,
                    UserEmail = contact.Email,
                    UserContact = contact.CountryCallingCode + contact.Phone,
                    Signature = CreateRequestSignature(transactionDetail.RsvNo, (long)payment.FinalPriceIdr, "IDR"),
                    ResponseURL = "", //TODO: ResponseURL
                    BackendURL = "" //TODO: BackendURL
                };
                request.AddObject(rqParam);
                var response = client.Execute(request);
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = FailureReason.PaymentFailure;

                return payment;
            }
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
                case PaymentMethod.OnlineSbi:
                    return 16;
                case PaymentMethod.BcaKlikpay:
                    return 18;
                case PaymentMethod.Qnb:
                    return 19;
                case PaymentMethod.Btn:
                    return 22;
                default:
                    return 0;
            }
        }

        private static string CreateRequestSignature(string rsvNo, long amount, string currency)
        {
            var plain = _merchantKey + _merchantCode + rsvNo + amount + currency;
            var hashed = plain.Sha1Encode();
            return hashed;
        }
    }
}
