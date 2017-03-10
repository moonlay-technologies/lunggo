using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetUserResponse : ApiResponseBase
    {
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserData> Users { get; set; }
        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Roles { get; set; }
        [JsonProperty("approvers", NullValueHandling = NullValueHandling.Ignore)]
        public List<ApproverData> Approvers { get; set; } 
    }
}