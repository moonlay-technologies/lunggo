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
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}