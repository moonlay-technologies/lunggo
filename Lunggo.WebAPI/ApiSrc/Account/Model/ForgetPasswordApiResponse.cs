﻿using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ForgetPasswordApiResponse : ApiResponseBase
    {
        [JsonProperty("otp", NullValueHandling = NullValueHandling.Ignore)]
        public string Otp { get; set; }
        [JsonProperty("expireOtp", NullValueHandling = NullValueHandling.Ignore)]
        public string ExpireOtp { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }
}