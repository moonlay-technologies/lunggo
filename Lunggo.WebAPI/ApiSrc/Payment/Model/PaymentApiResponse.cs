using Lunggo.ApCommon.Payment.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class PaymentApiResponse : ApiResponseBase
    {
        [JsonProperty("paymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentStatus? PaymentStatus { get; set; }
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod? Method { get; set; }
        [JsonProperty("redirectionUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectionUrl { get; set; }
        [JsonProperty("transferAccount", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferAccount { get; set; }
    }
}