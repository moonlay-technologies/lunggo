using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model
{
    public class HotelLocationApi
    {
        [JsonProperty("location_id")]
        public long LocationId { get; set; }
        [JsonProperty("location_name")]
        public string LocationName { get; set; }
        [JsonProperty("region_name")]
        public string RegionName { get; set; }
        [JsonProperty("country_name")]
        public string CountryName { get; set; }
        [JsonProperty("priority")]
        public int? Priority { get; set; }
    }
}