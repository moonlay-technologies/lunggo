using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetProfileApiResponse : ApiResponseBase
    {
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("first", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("last", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("countryCallCd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCallingCd { get; set; }
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
    }
}