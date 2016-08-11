using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class ChangeProfileApiResponse : ApiResponseBase
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("countryCallCd")]
        public string CountryCallingCd { get; set; }
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}