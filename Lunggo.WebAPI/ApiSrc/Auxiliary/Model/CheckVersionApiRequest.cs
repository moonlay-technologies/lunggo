using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class CheckVersionApiRequest
    {
        [JsonProperty("currentVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrentVersion { get; set; }
        [JsonProperty("platform", NullValueHandling = NullValueHandling.Ignore)]
        public string Platform { get; set; }
    }
}