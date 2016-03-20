using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class ChangeProfileApiRequest
    {
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
        [JsonProperty("country")]
        public string CountryCd { get; set; }
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}