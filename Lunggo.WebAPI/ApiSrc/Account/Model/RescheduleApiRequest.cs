using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class RescheduleApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}