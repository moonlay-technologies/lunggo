using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ForgetPasswordApiResponse : ApiResponseBase
    {
        [JsonProperty("countryCallCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCallCd { get; set; }
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("resendCooldown", NullValueHandling = NullValueHandling.Ignore)]
        public int? ResendCooldown { get; set; }
    }
}