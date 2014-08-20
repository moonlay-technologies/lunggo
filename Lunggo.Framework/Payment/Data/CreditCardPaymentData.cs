using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Payment.Data
{
    public class CreditCardPaymentData : PaymentData
    {
        public CreditCard CreditCard { get; set; }
        public CreditCardPaymentData()
        {
            CreditCard = new CreditCard();
        }
        public override PaymentDataDummy ConvertToDummyObject()
        {
            try
            {
                CreditCardPaymentDataDummy dummy = new CreditCardPaymentDataDummy();
                ConvertPaymentDataToPaymentDataDummy(dummy, this);

                dummy.credit_card.bank = this.CreditCard.Bank;
                dummy.credit_card.token_id = this.CreditCard.TokenId;

                return dummy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class CreditCard
    {
        public String TokenId { get; set; }
        public String Bank { get; set; }
    }
}
