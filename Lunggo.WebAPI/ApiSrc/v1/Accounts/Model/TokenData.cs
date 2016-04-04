using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class TokenData
    {
        [JsonProperty(".expires")]
        public DateTime ExpiryTime { get; set; }
        [JsonProperty(".issued")]
        public DateTime IssuingTime { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("as:client_id")]
        public string ClientId { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiryMinutes { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}