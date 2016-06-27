using System.Collections.Generic;
using System.Net;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Autocomplete.Model
{
    public class AutocompleteAirlinesApiResponse : ApiResponseBase
    {
        [JsonProperty("airlines", NullValueHandling = NullValueHandling.Ignore)]
        public List<AirlineApi> Airlines { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}