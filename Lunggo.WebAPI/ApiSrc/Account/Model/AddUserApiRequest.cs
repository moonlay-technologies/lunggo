using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class AddUserApiRequest
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("countryCallCd")]
        public string CountryCallCd { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("department")]
        public string Department { get; set; }
        [JsonProperty("branch")]
        public string Branch { get; set; }
        [JsonProperty("roles")]
        public List<string> Role { get; set; }
    }
}