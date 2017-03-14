using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class SetPaymentDisabilityStatusApiRequest
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
    }
}