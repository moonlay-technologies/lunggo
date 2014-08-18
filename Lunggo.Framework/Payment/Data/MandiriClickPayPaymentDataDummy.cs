using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class MandiriClickPayPaymentDataDummy : PaymentDataDummy
    {
        public MandiriClickpayDummy mandiri_clickpay { get; set; }
        public MandiriClickPayPaymentDataDummy()
        {
            this.mandiri_clickpay = new MandiriClickpayDummy();
        }
    }

    public class MandiriClickpayDummy
    {
        public string card_number { get; set; }
        public string input1 { get; set; }
        public string input2 { get; set; }
        public string input3 { get; set; }
        public string token { get; set; }
    }
}
