using Newtonsoft.Json;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public class Contact
    {
        [JsonProperty("nm")]
        public string Name { get; set; }
        [JsonProperty("ctycd")]
        public string CountryCode { get; set; }
        [JsonProperty("ph")]
        public string Phone { get; set; }
        [JsonProperty("em")]
        public string Email { get; set; }
        [JsonProperty("add")]
        public string Address { get; set; }
    }
}
