using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckVoucherApiRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}