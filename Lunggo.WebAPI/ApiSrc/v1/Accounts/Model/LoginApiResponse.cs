using System;
using System.ComponentModel.DataAnnotations;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class LoginApiResponse : ApiResponseBase
    {
        [JsonProperty("atkn")]
        public string AccessToken { get; set; }
        [JsonProperty("rtkn")]
        public string RefreshToken { get; set; }
        [JsonProperty("exp")]
        public DateTime ExpiryTime { get; set; }
    }
}