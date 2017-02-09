using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class AddCreditCardRequest
    {
        [JsonProperty("maskedCardNumber")]
        public string MaskedCardNumber { get; set; }
        [JsonProperty("isPrimaryCard")]
        public Boolean? IsPrimaryCard { get; set; }
        [JsonProperty("cardHolderName")]
        public string CardHolderName { get; set; }
        [JsonProperty("cardExpiry")]
        public DateTime CardExpiry { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("tokenExpiry")]
        public DateTime TokenExpiry { get; set; }
    }
}