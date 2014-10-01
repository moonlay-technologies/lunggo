using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
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
        public abstract PaymentResult PaymentResult(PaymentData paymentData);
        public string GetVeriTransHeaderKey()
        {
            try
            {
                var configManager = ConfigManager.GetInstance();
                var veritransServerKey = configManager.GetConfigValue("veritrans", "Authorization");
                var generatedServerKey = GenerateServerKey(veritransServerKey);
                return generatedServerKey;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        string GenerateServerKey(string serverKey)
        {
            var base64Key = Base64Encode(serverKey + ":");
            var resultGenerated = "Basic " + base64Key;
            return resultGenerated;
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
                var configManager = ConfigManager.GetInstance();
                var veritransRestSharpClientUrl = configManager.GetConfigValue("veritrans", "RestSharpClientUrl");
                var veritransRestSharpRequestUrl = configManager.GetConfigValue("veritrans", "RestSharpRequestUrl");
                var client = new RestClient(veritransRestSharpClientUrl);

                var request = new RestRequest(veritransRestSharpRequestUrl, Method.POST);

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
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
