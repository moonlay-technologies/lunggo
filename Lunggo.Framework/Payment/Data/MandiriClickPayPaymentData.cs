using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class MandiriClickPayPaymentData : PaymentData
    {
        public MandiriClickpay MandiriClickpay { get; set; }
        public MandiriClickPayPaymentData()
        {
            this.MandiriClickpay = new MandiriClickpay();
        }
        public override PaymentDataDummy ConvertToDummyObject()
        {
            try
            {
                MandiriClickPayPaymentDataDummy dummy = new MandiriClickPayPaymentDataDummy();
                ConvertPaymentDataToPaymentDataDummy(dummy, this);

                dummy.mandiri_clickpay.card_number = this.MandiriClickpay.CardNumber;
                dummy.mandiri_clickpay.input1 = this.MandiriClickpay.Input1;
                dummy.mandiri_clickpay.input2 = this.MandiriClickpay.Input2;
                dummy.mandiri_clickpay.input3 = this.MandiriClickpay.Input3;
                dummy.mandiri_clickpay.token = this.MandiriClickpay.Token;
                return dummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class MandiriClickpay
    {
        public string CardNumber { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
        public string Token { get; set; }
    }
}
