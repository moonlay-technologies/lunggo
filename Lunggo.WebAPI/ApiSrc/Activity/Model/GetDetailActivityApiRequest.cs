using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetDetailActivityApiRequest
    {
        [JsonProperty("id")]
        public string ActivityId { get; set; }
    }
}
