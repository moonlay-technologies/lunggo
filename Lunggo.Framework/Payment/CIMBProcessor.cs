using Lunggo.Framework.Config;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment
{
    public class CIMBProcessor : PaymentProcessor
    {
        public override PaymentResult PaymentResult(PaymentData paymentParamData)
        {
            CIMBPaymentData paymentData = (CIMBPaymentData)paymentParamData;
            PaymentResult result = new PaymentResult();
            string json = JsonConvert.SerializeObject(paymentData.ConvertToDummyObject());
            return null;
        }
    }
}
