using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ConfirmEmailApiResponse : ApiResponseBase
    {
        [JsonProperty("url")]
        public string RedirectionUrl { get; set; }
    }
}