using Lunggo.Framework.Config;
using Lunggo.Framework.Payment.Data;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment
{
    public abstract class PaymentProcessor
    {
        protected string VeritransClientUrl = "https://api.sandbox.veritrans.co.id/";
        protected string VeritransRequestUrl = "v2/charge";
        public abstract PaymentResult PaymentResult(PaymentData paymentData);
        public string GetVeriTransHeaderKey()
        {
            try
            {
                var configManager = ConfigManager.GetInstance();
                string VeritransServerKey = configManager.GetConfigValue("veritrans", "Authorization");
                string generatedServerKey = GenerateServerKey(VeritransServerKey);
                return generatedServerKey;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        string GenerateServerKey(string serverKey)
        {
            string base64Key = Base64Encode(serverKey + ":");
            string ResultGenerated = "Basic " + base64Key;
            return ResultGenerated;
        }
        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public IRestResponse RequestToVeritransByJson(string json)
        {
            try
            {
                var client = new RestClient(VeritransClientUrl);

                var request = new RestRequest(VeritransRequestUrl, Method.POST);

                request.AddHeader("Authorization", GetVeriTransHeaderKey());
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                request.XmlSerializer.ContentType = "application/json";
                var responses = client.Execute(request);
                return responses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
