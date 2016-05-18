using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class AutocompleteAirportsApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("airports")]
        public IEnumerable<AirportApi> Airports { get; set; }
    }
}