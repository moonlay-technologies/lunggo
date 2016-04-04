using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class ChangeProfileApiRequest
    {
        [JsonProperty("fst")]
        public string FirstName { get; set; }
        [JsonProperty("lst")]
        public string LastName { get; set; }
        [JsonProperty("ctycd")]
        public string CountryCd { get; set; }
        [JsonProperty("ph")]
        public string PhoneNumber { get; set; }
        [JsonProperty("add")]
        public string Address { get; set; }
    }
}