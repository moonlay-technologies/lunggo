using System.Collections.Generic;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class AutocompleteAirportsApiResponse : ApiResponseBase
    {
        [JsonProperty("airports")]
        public List<AirportApi> Airports { get; set; }
        [JsonProperty("count")]
        public int? Count { get; set; }
    }
}