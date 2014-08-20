using Lunggo.Framework.Config;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment
{
    public class MandiriClickPayProcessor : PaymentProcessor
    {
        public override PaymentResult PaymentResult(PaymentData paymentParamData)
        {
            try
            {
                MandiriClickPayPaymentData paymentData = (MandiriClickPayPaymentData)paymentParamData;
                PaymentResult result = new PaymentResult();
                string json = JsonConvert.SerializeObject(paymentData.ConvertToDummyObject());

                IRestResponse responses = RequestToVeritransByJson(json);
                dynamic data = JObject.Parse(responses.Content);

                result.Result = (string)data.status_code;
                if (result.Result == ((int)HttpStatusCode.OK).ToString())
                    result.OrderId = (string)data.order_id;
                else
                    result.StatusMessage = (string)data.status_message;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
