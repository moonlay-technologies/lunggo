using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class CheckVersionApiResponse : ApiResponseBase
    {
        [JsonProperty("latestVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string LatestVersion { get; set; }
        [JsonProperty("mustUpdate", NullValueHandling = NullValueHandling.Ignore)]
        public bool MustUpdate { get; set; }
    }
}