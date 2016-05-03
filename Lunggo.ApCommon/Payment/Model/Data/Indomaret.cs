using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class Indomaret
    {
        [JsonProperty("Store")]
        public string StoreName { get; set; }
        [JsonProperty("Message")]
        public string Label { get; set; }
    }
}