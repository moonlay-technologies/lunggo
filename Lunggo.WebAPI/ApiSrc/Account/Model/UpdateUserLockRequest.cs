using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class UpdateUserLockRequest
    {
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }
        [JsonProperty("isLocked", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLocked { get; set; }
    }
}