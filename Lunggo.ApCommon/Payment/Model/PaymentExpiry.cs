using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentExpiry
    {
        [JsonProperty("expiry_duration")]
        public int Duration { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}
