using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Veritrans.Model;
using Lunggo.Framework.Payment.Data;

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
                ItemDetail = itemDetails
            };
            var webRequest = WebRequest.Create("https://api.sandbox.veritrans.co.id/v2/charge");
            webRequest.Headers.Add("Authorization", "Basic " + hashedServerKey);
            webRequest.Headers.Add("Content-type", "application/json");
            webRequest.Headers.Add("Accept", "application/json");
            var webResponse = webRequest.GetResponse();
            return null;
        }
    }
}
