using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class CheckOtpApiRequest
    {
        [JsonProperty("otp", NullValueHandling = NullValueHandling.Ignore)]
        public string Otp { get; set; }
        [JsonProperty("countryCallCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCallCd { get; set; }
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}