using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class SetBookingDisabilityStatusApiRequest
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
    }
}