using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class EditCreditCardApiRequest
    {
        [JsonProperty("maskedCardNo")]
        public string MaskedCardNumber { get; set; }
        [JsonProperty("cardHolderName")]
        public string CardHolderName { get; set; }
        [JsonProperty("cardExpirymonth")]
        public int CardExpiryMonth { get; set; }
        [JsonProperty("cardExpiryYear")]
        public int CardExpiryYear { get; set; }
    }
}