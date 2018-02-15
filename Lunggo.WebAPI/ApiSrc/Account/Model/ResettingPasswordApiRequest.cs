using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ResettingPasswordApiRequest
    {
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("otp", NullValueHandling = NullValueHandling.Ignore)]
        public string Otp { get; set; }
        [JsonProperty("newPassword", NullValueHandling = NullValueHandling.Ignore)]
        public string NewPassword { get; set; }
    }
}