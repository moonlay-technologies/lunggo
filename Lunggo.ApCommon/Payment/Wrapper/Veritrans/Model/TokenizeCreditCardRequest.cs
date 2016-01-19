using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class TokenizeCreditCardRequest
    {
        [JsonProperty("card_number")]
        public string CardNumber { get; set; }
        [JsonProperty("card_cvv")]
        public string CardCvv { get; set; }
        [JsonProperty("card_exp_month")]
        public int CardExpiryMonth { get; set; }
        [JsonProperty("card_exp_year")]
        public int CardExpiryYear { get; set; }
        [JsonProperty("bank")]
        public string Bank { get; set; }
        [JsonProperty("secure")]
        public bool ThreeDSecureEnabled { get; set; }
        [JsonProperty("gross_amount")]
        public long Amount { get; set; }
        [JsonProperty("installment")]
        public bool InstallmentEnabled { get; set; }
        [JsonProperty("installment_term")]
        public int InstallmentTerm { get; set; }
        [JsonProperty("token_id")]
        public string TokenId { get; set; }
        [JsonProperty("two_click")]
        public string TwoClicksEnabled { get; set; }
        [JsonProperty("type")]
        public string TransactionType { get; set; }
    }
}
