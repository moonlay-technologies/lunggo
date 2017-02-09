using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class GetSavedCreditCardResponse : ApiResponseBase
    {
        [JsonProperty("creditCards")]
        public List<CreditCard> SavedCreditCard { get; set; }
    }

    public class CreditCard
    {
        [JsonProperty("maskedCardNumber")]
        public string MaskedCardNumber { get; set; }
        [JsonProperty("isPrimaryCard")]
        public Boolean? IsPrimaryCard { get; set; }
        [JsonProperty("cardHolderName")]
        public string CardHolderName { get; set; }
        [JsonProperty("cardExpiry")]
        public DateTime CardExpiry { get; set; }
    }
}