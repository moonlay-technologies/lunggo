using Lunggo.Framework.Config;
using Lunggo.Framework.Payment.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment
{
    public abstract class PaymentProcessor
    {
        protected string VeritransClientUrl = "https://api.sandbox.veritrans.co.id";
        protected string VeritransRequestUrl = "v2/charge";
        public abstract PaymentResult PaymentResult(PaymentData paymentData);
        public string GenerateServerKey(string serverKey)
        {
            string base64Key = EncodeTo64(serverKey+":");
            string ResultGenerated = "Basic " + base64Key;
            return ResultGenerated;
        }
        public string GetVeriTransHeaderKey()
        {
            var configManager = ConfigManager.GetInstance();
            string VeritransServerKey = configManager.GetConfigValue("veritrans", "Authorization");
            string generatedServerKey = GenerateServerKey(VeritransServerKey);
            return generatedServerKey;
        }
        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
