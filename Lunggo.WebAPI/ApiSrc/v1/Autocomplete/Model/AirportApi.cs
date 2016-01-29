using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model
{
    public class AirportApi
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}