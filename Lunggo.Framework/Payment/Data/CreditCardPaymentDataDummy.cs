using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class CreditCardPaymentDataDummy : PaymentDataDummy
    {
        public CreditCardDummy credit_card { get; set; }
        public CreditCardPaymentDataDummy()
        {
            credit_card = new CreditCardDummy();
        }
    }

    public class CreditCardDummy
    {
        public String token_id { get; set; }
        public String bank { get; set; }
    }
}
