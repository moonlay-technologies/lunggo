using Lunggo.ApCommon.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetReferralDetailApiResponse : ApiResponseBase
    {

        [JsonProperty("referralDetails", NullValueHandling = NullValueHandling.Ignore)]
        public List<ReferralDetail> ReferralDetails { get; set; }
    }
}