using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class Indomaret
    {
        [JsonProperty("storeName")]
        public string StoreName { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
    }
}