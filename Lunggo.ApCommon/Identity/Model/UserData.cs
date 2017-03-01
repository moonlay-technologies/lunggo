using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Identity.Model
{
    public class UserData
    {
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("roleName", NullValueHandling = NullValueHandling.Ignore)]
        public string RoleName { get; set; }
    }
}
