using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class GetProfileApiResponse : ApiResponseBase
    {
        [JsonProperty("em", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("fst", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("lst", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("ctycd", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCd { get; set; }
        [JsonProperty("ph", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("add", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }
}