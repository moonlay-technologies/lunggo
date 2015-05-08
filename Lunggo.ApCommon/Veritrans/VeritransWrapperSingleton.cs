using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans.Model;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Veritrans
{
    internal partial class VeritransWrapper
    {
        private static readonly VeritransWrapper Instance = new VeritransWrapper();
        private bool _isInitialized;

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

        internal string VtWeb(TransactionDetail transactionDetail, List<ItemDetail> itemDetails)
        {
            var serverKey = "VT-server-NMbr8EGtsINe4Rsw9SjmWxsl" + ":";
            var plainServerKey = Encoding.UTF8.GetBytes(serverKey);
            var hashedServerKey = Convert.ToBase64String(plainServerKey);

            var request = new Request
            {
                PaymentType = "vtweb",
                TransactionDetail = transactionDetail,
                ItemDetail = itemDetails,
                VtWeb = new VtWeb
                {
                    EnabledPayments = @"[""credit_card""]",
                    CreditCard3DSecure = true
                }
            };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var webRequest = (HttpWebRequest) WebRequest.Create("https://api.sandbox.veritrans.co.id/v2/charge");
            webRequest.Headers.Add("Authorization", "Basic " + hashedServerKey);
            webRequest.Accept = "application/json";
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";
            var dataStream = webRequest.GetRequestStream();
            using (var streamWriter = new StreamWriter(dataStream))
            {
                streamWriter.Write(jsonRequest);
            }
            var webResponse = (HttpWebResponse) webRequest.GetResponse();
            var streamContent = webResponse.GetResponseStream();
            string jsonContent;
            using (var reader = new StreamReader(streamContent))
            {
                jsonContent = reader.ReadToEnd();
            }
            var content = JsonConvert.DeserializeObject<Response>(jsonContent);
            if (content.StatusCode == "201")
            {
                return content.RedirectUrl;
            }
            else
            {
                return null;
            }
        }
    }
}
