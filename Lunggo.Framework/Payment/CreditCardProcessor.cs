using Lunggo.Framework.Config;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment
{
    public class CreditCardProcessor : PaymentProcessor
    {
        public override PaymentResult PaymentResult(PaymentData paymentParamData)
        {
            CreditCardPaymentData paymentData = (CreditCardPaymentData)paymentParamData;
            PaymentResult result = new PaymentResult();
            string json = JsonConvert.SerializeObject(paymentData.ConvertToDummyObject());
            var client = new RestClient(VeritransClientUrl);

            var request = new RestRequest(VeritransRequestUrl, Method.POST);
            request.RequestFormat = DataFormat.Json;

            request.AddBody(json);

            //RestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            return null;
        }
    }
}
