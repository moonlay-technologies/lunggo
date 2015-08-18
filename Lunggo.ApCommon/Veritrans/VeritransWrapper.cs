using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lunggo.ApCommon.Veritrans
{
    internal class VeritransWrapper
    {
        private static readonly VeritransWrapper Instance = new VeritransWrapper();
        private bool _isInitialized;

        private static string _endPoint;
        private static string _serverKey;
        private static string _rootUrl;
        private static string _finishedRedirectUrl;
        private static string _unfinishedRedirectUrl;
        private static string _errorRedirectUrl;

        private const string FinishRedirectPath = @"/Veritrans/PaymentFinish";
        private const string UnfinishRedirectPath = @"/Veritrans/PaymentUnfinish";
        private const string ErrorRedirectPath = @"/Veritrans/PaymentError";

        private VeritransWrapper()
        {
            
        }

        internal static VeritransWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                var langCode = @"/" + OnlineContext.GetActiveLanguageCode();
                _endPoint = ConfigManager.GetInstance().GetConfigValue("veritrans", "endPoint");
                _serverKey = ConfigManager.GetInstance().GetConfigValue("veritrans", "serverKey");
                _rootUrl = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
                _finishedRedirectUrl = _rootUrl + langCode + FinishRedirectPath;
                _unfinishedRedirectUrl = _rootUrl + langCode + UnfinishRedirectPath;
                _errorRedirectUrl = _rootUrl + langCode + ErrorRedirectPath;
                _isInitialized = true;
            }
        }

        internal string GetPaymentUrl(TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var authorizationKey = ProcessAuthorizationKey(_serverKey);
            var request = CreateRequest(authorizationKey, transactionDetail, itemDetails, method);
            var response = SubmitRequest(request);
            var url = GetUrlFromResponse(response);
            return url;
        }

        private static string ProcessAuthorizationKey(string serverKey)
        {
            var plainAuthorizationKey = Encoding.UTF8.GetBytes(serverKey);
            var hashedAuthorizationKey = Convert.ToBase64String(plainAuthorizationKey);
            return hashedAuthorizationKey;
        }

        private static WebRequest CreateRequest(string authorizationKey, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var request = (HttpWebRequest) WebRequest.Create(_endPoint);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + authorizationKey);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            ProcessRequestParams(request, transactionDetail, itemDetails, method);
            return request;
        }

        private static void ProcessRequestParams(WebRequest request, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout"));
            var requestParams = new Request
            {
                PaymentType = "vtweb",
                TransactionDetail = transactionDetail,
                ItemDetail = itemDetails,
                PaymentExpiry = new PaymentExpiry
                {
                    Duration = timeout,
                    Unit = "minute"
                },
                VtWeb = new VtWeb
                {
                    EnabledPayments = new List<string> {MapEnabledPayment(method)},
                    CreditCard3DSecure = true,
                    FinishRedirectUrl = _finishedRedirectUrl,
                    UnfinishRedirectUrl = _unfinishedRedirectUrl,
                    ErrorRedirectUrl = _errorRedirectUrl
                }
            };
            var jsonRequestParams = JsonConvert.SerializeObject(requestParams);
            var dataStream = request.GetRequestStream();
            using (var streamWriter = new StreamWriter(dataStream))
            {
                streamWriter.Write(jsonRequestParams);
            }
        }

        private static string MapEnabledPayment(PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CreditCard:
                    return "credit_card";
                case PaymentMethod.MandiriClickPay:
                    return "mandiri_clickpay";
                case PaymentMethod.CimbClicks:
                    return "cimb_clicks";
                case PaymentMethod.VirtualAccount:
                    return "bank_transfer";
                default:
                    return null;
            }
        }

        private static WebResponse SubmitRequest(WebRequest request)
        {
            var response = (HttpWebResponse) request.GetResponse();
            return response;
        }

        private static string GetUrlFromResponse(WebResponse response)
        {
            var streamContent = response.GetResponseStream();
            if (streamContent != null)
            {
                string jsonContent;
                using (var reader = new StreamReader(streamContent))
                {
                    jsonContent = reader.ReadToEnd();
                }
                var content = JsonConvert.DeserializeObject<Response>(jsonContent);
                return content.StatusCode == "201" ? content.RedirectUrl : null;
            }
            else
            {
                return null;
            }
        }
    }
}
