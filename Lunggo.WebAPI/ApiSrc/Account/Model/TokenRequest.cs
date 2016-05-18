using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class TokenRequest
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
    }
}