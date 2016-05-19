using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Common.Model
{
    public class ApiResponseBase
    {
        [JsonProperty("status")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
        [JsonProperty("ver")]
        public string Version { get; set; }

        public ApiResponseBase()
        {
            Version = "1.0";
        }
    }
}