using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Dictionary;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model
{
    public class AutocompleteHotelLocationsApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("hotel_locations")]
        public IEnumerable<HotelLocationApi> HotelLocations { get; set; }
    }
}