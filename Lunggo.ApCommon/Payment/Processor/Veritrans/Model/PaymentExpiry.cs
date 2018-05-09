using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        public class PaymentExpiry
        {
            [JsonProperty("order_time")] public string OrderTime { get; set; }
            [JsonProperty("expiry_duration")] public int Duration { get; set; }
            [JsonProperty("unit")] public string Unit { get; set; }
        }
    }

}