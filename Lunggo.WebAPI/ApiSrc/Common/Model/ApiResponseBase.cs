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

        public static ApiResponseBase Return500()
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorCode = "ERRGEN99"
            };
        }

        public static ApiResponseBase ReturnInvalidJson()
        {
            return new ApiResponseBase
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERRGEN98"
            };
        }
    }
}