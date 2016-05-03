using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class CimbClicks
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}