using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetUserRequest
    {
        [JsonProperty("userFilter")]
        public UserFilterRequest Filter { get; set; }
        [JsonProperty("userSorting")]
        public string Sorting { get; set; }
    }

    public class UserFilterRequest
    {
        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("branch")]
        public string Branch { get; set; }
    }
}