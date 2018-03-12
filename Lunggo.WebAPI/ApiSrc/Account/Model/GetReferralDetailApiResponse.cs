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

        [JsonProperty("referralDetail", NullValueHandling = NullValueHandling.Ignore)]
        public List<ReferralHistoryModelForDisplay> ReferralDetail { get; set; }
    }
}