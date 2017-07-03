using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class HotelLocationApi
    {
        [JsonProperty("locationId", NullValueHandling = NullValueHandling.Ignore)]
        public long LocationId { get; set; }
        [JsonProperty("locationId", NullValueHandling = NullValueHandling.Ignore)]
        public long LocationTiketId { get; set; }
        [JsonProperty("locationName", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationName { get; set; }
        [JsonProperty("regionName", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionName { get; set; }
        [JsonProperty("countryName", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryName { get; set; }
        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public int? Priority { get; set; }
    }
}