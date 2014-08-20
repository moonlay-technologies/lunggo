using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class CIMBPaymentData : PaymentData
    {
        public CIMBClick CIMBClicks { get; set; }
        public CIMBPaymentData()
        {
            CIMBClicks = new CIMBClick();
        }
        
        public override PaymentDataDummy ConvertToDummyObject()
        {

            try
            {
                CIMBPaymentDataDummy dummy = new CIMBPaymentDataDummy();
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
        public string Description { get; set; }
    }
}
