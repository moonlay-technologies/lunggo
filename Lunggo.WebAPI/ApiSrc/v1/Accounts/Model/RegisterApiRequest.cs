using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class RegisterApiRequest
    {
        [Required]
        [EmailAddress]
        [JsonProperty("em")]
        public string Email { get; set; }
    }
}