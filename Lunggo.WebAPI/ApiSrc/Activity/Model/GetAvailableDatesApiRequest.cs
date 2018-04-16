using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAvailableDatesApiRequest
    {
        [JsonProperty("id")]
        public string ActivityId { get; set; }
    }
}
