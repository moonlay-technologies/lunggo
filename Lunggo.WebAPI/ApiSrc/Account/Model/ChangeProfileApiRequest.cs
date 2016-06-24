using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ChangeProfileApiRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("countryCallCd")]
        public string CountryCallingCd { get; set; }
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}