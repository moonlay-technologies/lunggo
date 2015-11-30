using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class ItemDetails
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("price")]
        public long Price { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
