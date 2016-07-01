using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ForgotPasswordApiRequest
    {
        [Required]
        [JsonProperty("userName")]
        public string UserName { get; set; }
    }
}