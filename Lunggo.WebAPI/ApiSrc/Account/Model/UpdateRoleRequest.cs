using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class UpdateRoleRequest
    {
        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }
    }
}