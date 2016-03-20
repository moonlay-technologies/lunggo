using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class GetProfileApiResponse : ApiResponseBase
    {
        [JsonProperty("first", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("last", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryCd { get; set; }
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
    }
}