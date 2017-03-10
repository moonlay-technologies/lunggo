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
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }
        [JsonProperty("userName", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("countryCallCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCallCd { get; set; }
        [JsonProperty("phoneNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }
        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }
        [JsonProperty("department", NullValueHandling = NullValueHandling.Ignore)]
        public string Department { get; set; }
        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("roleName", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RoleName { get; set; }
        [JsonProperty("approverId", NullValueHandling = NullValueHandling.Ignore)]
        public string ApproverId { get; set; }
        [JsonProperty("approverName", NullValueHandling = NullValueHandling.Ignore)]
        public string ApproverName { get; set; }
    }
}
