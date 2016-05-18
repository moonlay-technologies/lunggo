using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ForgotPasswordApiRequest
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}