using System.Collections.Generic;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class HotelAutocompleteApiResponse : ApiResponseBase
    {
        [JsonProperty("hotelAutocomplete")]
        public List<HotelAutocompleteApi> Autocompletes { get; set; }
        [JsonProperty("count")]
        public int? Count { get; set; }
    }
}