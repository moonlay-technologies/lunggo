using System;
using Newtonsoft.Json;

namespace Lunggo.Framework.Payment.Data
{
    public class CIMBPaymentData : PaymentData
    {

        [JsonProperty("cimb_clicks")]
        public CIMBClick CIMBClicks { get; set; }
        public CIMBPaymentData()
        {
            CIMBClicks = new CIMBClick();
        }
        
        public override PaymentDataDummy ConvertToDummyObject()
        {

            try
            {
                var dummy = new CIMBPaymentDataDummy();
                ConvertPaymentDataToPaymentDataDummy(dummy, this);

                dummy.cimb_clicks.description = this.CIMBClicks.Description;

                return dummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public override static PaymentDataDummy ConvertPaymentDataToPaymentDataDummy(PaymentDataDummy Dummy, PaymentData Origin)
        //{
        //    return paren
        //}
    }
    public class CIMBClick 
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
