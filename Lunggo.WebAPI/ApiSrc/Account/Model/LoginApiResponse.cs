using System;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class LoginApiResponse : ApiResponseBase
    {
        [JsonProperty("accessToken", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }
        [JsonProperty("refreshToken", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
        [JsonProperty("expTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpiryTime { get; set; }
    }
}