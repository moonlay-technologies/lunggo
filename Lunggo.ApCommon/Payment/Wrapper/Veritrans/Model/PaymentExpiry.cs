using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    public class PaymentExpiry
    {
        [JsonProperty("order_time")]
        public string OrderTime { get; set; }
        [JsonProperty("expiry_duration")]
        public int Duration { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}
