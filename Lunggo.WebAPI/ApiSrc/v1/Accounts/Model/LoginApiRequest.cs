using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class LoginApiRequest
    {
        [JsonProperty("em")]
        public string Email { get; set; }
        [JsonProperty("pw")]
        public string Password { get; set; }
        [JsonProperty("rtkn")]
        public string RefreshToken { get; set; }
        [JsonProperty("cid")]
        public string ClientId { get; set; }
        [JsonProperty("csc")]
        public string ClientSecret { get; set; }
    }
}