using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class Contact
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("country")]
        public string CountryCode { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
