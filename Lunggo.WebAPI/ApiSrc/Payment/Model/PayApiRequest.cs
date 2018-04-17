using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Constant;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class PayApiRequest
    {
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod Method { get; set; }
        [JsonProperty("submethod", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentSubmethod? Submethod { get; set; }
        [JsonProperty("discCd", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountCode { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("creditCard", NullValueHandling = NullValueHandling.Ignore)]
        public CreditCard CreditCard { get; set; }
        [JsonProperty("mandiriClickPay", NullValueHandling = NullValueHandling.Ignore)]
        public MandiriClickPay MandiriClickPay { get; set; }
    }

    public class CreditCard
    {
        [JsonProperty("tokenId")]
        internal string TokenId { get; set; }
    }

    public class MandiriClickPay
    {
        [JsonProperty("cardNo")]
        public string CardNumber { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}