using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class RegisterApiRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("countryCallCd")]
        public string CountryCallCd { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}