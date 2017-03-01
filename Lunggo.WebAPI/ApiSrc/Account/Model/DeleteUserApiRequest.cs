using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class DeleteUserApiRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}