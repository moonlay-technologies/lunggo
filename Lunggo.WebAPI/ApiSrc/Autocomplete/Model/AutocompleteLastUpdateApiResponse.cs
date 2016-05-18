using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class AutocompleteLastUpdateApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("update_sets")]
        public IEnumerable<UpdateSet> UpdateSets { get; set; }
    }
}