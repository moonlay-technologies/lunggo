using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.Framework.Payment.Data
{
    public class CreditCardPaymentData : PaymentData
    {
        [JsonProperty("credit_card")]
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
        [JsonProperty("token_id")]
        public String TokenId { get; set; }
        [JsonProperty("bank")]
        public String Bank { get; set; }
    }
}
