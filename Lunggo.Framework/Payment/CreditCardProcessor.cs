using System.Globalization;
using Lunggo.Framework.Core;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;

namespace Lunggo.Framework.Payment
{
    public class CreditCardProcessor : PaymentProcessor
    {
        public override PaymentResult PaymentResult(PaymentData paymentParamData)
        {
            try
            {
                var paymentData = (CreditCardPaymentData)paymentParamData;
                var result = new PaymentResult();
                //var json = JsonConvert.SerializeObject(paymentData.ConvertToDummyObject());
                var json = JsonConvert.SerializeObject(paymentData);
                var responses = RequestToVeritransByJson(json);
                dynamic data = JObject.Parse(responses.Content);

                result.Result = (string)data.status_code;
                if (result.Result == ((int)HttpStatusCode.OK).ToString(CultureInfo.InvariantCulture))
                    result.OrderId = (string)data.order_id;
                else
                    result.StatusMessage = (string)data.status_message;

                return result;
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
