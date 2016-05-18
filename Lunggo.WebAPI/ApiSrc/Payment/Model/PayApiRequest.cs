using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class PayApiRequest
    {
        [JsonProperty("method")]
        public PaymentMethod Method { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentData Data { get; set; }
        [JsonProperty("token")]
        public string DiscountCode { get; set; }
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}