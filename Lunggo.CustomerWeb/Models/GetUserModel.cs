using System.Collections.Generic;
using Lunggo.ApCommon.Identity.Model;
using Newtonsoft.Json;

namespace Lunggo.CustomerWeb.Models
{
    public class GetUserModel
    {
        [JsonProperty("users", NullValueHandling = NullValueHandling.Ignore)]
        public List<UserData> Users { get; set; }
        [JsonProperty("roles", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Roles { get; set; }
        [JsonProperty("approvers", NullValueHandling = NullValueHandling.Ignore)]
        public List<ApproverDataModel> Approvers { get; set; }
    }

    public class ApproverDataModel
    {
        [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

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
        [JsonProperty("isLocked", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLocked { get; set; }
    }
}