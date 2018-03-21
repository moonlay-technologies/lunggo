using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetReferralApiResponse : ApiResponseBase
    {
        [JsonProperty("referralCredit", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ReferralCredit { get; set; }
        [JsonProperty("referralCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ReferralCode { get; set; }
        [JsonProperty("expDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpDate { get; set; }
        [JsonProperty("shareableLink", NullValueHandling = NullValueHandling.Ignore)]
        public string ShareableLink { get; set; }
        [JsonProperty("referralContent", NullValueHandling = NullValueHandling.Ignore)]
        public string ReferralContent { get; set; }
        [JsonProperty("shareDialog", NullValueHandling = NullValueHandling.Ignore)]
        public string ShareDialog { get; set; }
    }
}