using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class AddUserApiResponse : ApiResponseBase
    {
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

    }
}