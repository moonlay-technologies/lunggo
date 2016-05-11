using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class BcaKlikPay
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("miscFee")]
        public int MiscFee { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}