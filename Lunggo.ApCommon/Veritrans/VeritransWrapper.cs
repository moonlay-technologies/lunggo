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
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lunggo.ApCommon.Veritrans
{
    internal class VeritransWrapper
    {
        private static readonly VeritransWrapper Instance = new VeritransWrapper();
        private bool _isInitialized;

        private readonly static string VeritransEndPoint = "https://api.sandbox.veritrans.co.id/v2/charge";
        private readonly static string ServerKey = "VT-server-NMbr8EGtsINe4Rsw9SjmWxsl";
        private readonly static string Password = "";

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
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("VeritransWrapper is already initialized");
            }
        }

        internal string GetPaymentUrl(TransactionDetails transactionDetail, List<ItemDetails> itemDetails)
        {
            var authorizationKey = ProcessAuthorizationKey(ServerKey, Password);
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
            var request = (HttpWebRequest) WebRequest.Create(VeritransEndPoint);
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
                    CreditCard3DSecure = true
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
