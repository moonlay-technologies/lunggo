using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ChangePasswordApiRequest
    {
        [JsonProperty("oldPassword", NullValueHandling = NullValueHandling.Ignore)]
        public string OldPassword { get; set; }
        [JsonProperty("newPassword", NullValueHandling = NullValueHandling.Ignore)]
        public string NewPassword { get; set; }
    }
}