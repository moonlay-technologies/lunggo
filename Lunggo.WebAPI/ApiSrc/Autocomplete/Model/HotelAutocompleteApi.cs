using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class HotelAutocompleteApi
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("numOfHotels", NullValueHandling = NullValueHandling.Ignore)]
        public int? NumOfHotels { get; set; }
    }
}