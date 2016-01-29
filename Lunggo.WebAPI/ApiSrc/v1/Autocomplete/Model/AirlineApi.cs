using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model
{
    public class AirlineApi
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}