using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySelectApiRequest
    {
        [JsonProperty("id")]
        public string ActivityId { get; set; }
    }
}
