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
using System.Web.Helpers;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans.Model;
using Lunggo.Framework.Config;
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
        private static string _password;
        private static string _rootRedirectUrl;
        private static string _finishRedirectUrl;
        private static string _unfinishRedirectUrl;
        private static string _errorRedirectUrl;

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
                // TODO flight replace password override
                _endPoint = ConfigManager.GetInstance().GetConfigValue("veritrans", "endPoint");
                _serverKey = ConfigManager.GetInstance().GetConfigValue("veritrans", "serverKey");
                _password = "";
                //_password = ConfigManager.GetInstance().GetConfigValue("veritrans", "password");
                _rootRedirectUrl = ConfigManager.GetInstance().GetConfigValue("veritrans", "rootRedirectUrl");
                _finishRedirectUrl = _rootRedirectUrl + ConfigManager.GetInstance().GetConfigValue("veritrans", "finishRedirectUrl");
                _unfinishRedirectUrl = _rootRedirectUrl + ConfigManager.GetInstance().GetConfigValue("veritrans", "unfinishRedirectUrl");
                _errorRedirectUrl = _rootRedirectUrl + ConfigManager.GetInstance().GetConfigValue("veritrans", "errorRedirectUrl");
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("VeritransWrapper is already initialized");
            }
        }

        internal string GetPaymentUrl(TransactionDetails transactionDetail, List<ItemDetails> itemDetails)
        {
            var authorizationKey = ProcessAuthorizationKey(_serverKey, _password);
            var request = CreateRequest(authorizationKey, transactionDetail, itemDetails);
            var response = SubmitRequest(request);
            var url = GetUrlFromResponse(response);
            return url;
        }

        private static string ProcessAuthorizationKey(string serverKey, string password)
        {
            var rawAuthorizationKey = serverKey + ":" + password;
            var plainAuthorizationKey = Encoding.UTF8.GetBytes(rawAuthorizationKey);
            var hashedAuthorizationKey = Convert.ToBase64String(plainAuthorizationKey);
            return hashedAuthorizationKey;
        }

        private static WebRequest CreateRequest(string authorizationKey, TransactionDetails transactionDetail, List<ItemDetails> itemDetails)
        {
            var request = (HttpWebRequest) WebRequest.Create(_endPoint);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + authorizationKey);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            ProcessRequestParams(request, transactionDetail, itemDetails);
            return request;
        }

        private static void ProcessRequestParams(WebRequest request, TransactionDetails transactionDetail, List<ItemDetails> itemDetails)
        {
            var requestParams = new Request
            {
                PaymentType = "vtweb",
                TransactionDetail = transactionDetail,
                ItemDetail = itemDetails,
                VtWeb = new VtWeb
                {
                    CreditCard3DSecure = true,
                    FinishRedirectUrl = _finishRedirectUrl,
                    UnfinishRedirectUrl = _unfinishRedirectUrl,
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
