using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ConfirmEmailApiRequest
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}