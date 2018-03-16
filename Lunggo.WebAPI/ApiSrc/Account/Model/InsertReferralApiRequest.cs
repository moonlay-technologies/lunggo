using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class InsertReferralApiRequest
    {
        [JsonProperty("referralCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ReferralCode { get; set; }
    }
}