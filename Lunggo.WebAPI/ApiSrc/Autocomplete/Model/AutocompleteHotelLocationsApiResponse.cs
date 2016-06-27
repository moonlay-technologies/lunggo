using System.Collections.Generic;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class AutocompleteHotelLocationsApiResponse : ApiResponseBase
    {
        [JsonProperty("hotelLocations", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelLocationApi> HotelLocations { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}