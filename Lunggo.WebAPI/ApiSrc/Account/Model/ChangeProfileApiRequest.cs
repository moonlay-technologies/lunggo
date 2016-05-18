using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ChangeProfileApiRequest
    {
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
        [JsonProperty("countryCallCd")]
        public string CountryCallingCd { get; set; }
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}