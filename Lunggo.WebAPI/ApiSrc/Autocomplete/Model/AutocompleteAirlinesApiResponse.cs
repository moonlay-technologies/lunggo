using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class AutocompleteAirlinesApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("airlines")]
        public IEnumerable<AirlineApi> Airlines { get; set; }
    }
}