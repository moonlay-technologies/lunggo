using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class NewsletterSubscribeApiRequest
    {
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}