using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class GetProfileApiRequest
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}