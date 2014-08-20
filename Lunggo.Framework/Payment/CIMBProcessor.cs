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
    public class CIMBProcessor : PaymentProcessor
    {
        public override PaymentResult PaymentResult(PaymentData paymentParamData)
        {
            try
            {
                CIMBPaymentData paymentData = (CIMBPaymentData)paymentParamData;
                PaymentResult result = new PaymentResult();
                string json = JsonConvert.SerializeObject(paymentData.ConvertToDummyObject());

                IRestResponse responses = RequestToVeritransByJson(json);
                dynamic data = JObject.Parse(responses.Content);

                result.Result = (string)data.status_code;
                if (result.Result == ((int)HttpStatusCode.Created).ToString())
                    result.RedirectUrl = (string)data.redirect_url;
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
