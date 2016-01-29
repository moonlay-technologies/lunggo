using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Autocomplete;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Autocomplete.Model
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