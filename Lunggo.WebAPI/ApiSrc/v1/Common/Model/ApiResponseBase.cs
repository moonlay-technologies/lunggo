using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Common.Model
{
    public class ApiResponseBase
    {
        [JsonProperty("st")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("err", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorCode { get; set; }
    }
}