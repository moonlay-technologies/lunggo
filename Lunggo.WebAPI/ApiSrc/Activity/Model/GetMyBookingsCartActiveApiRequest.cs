using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetMyBookingsCartActiveApiRequest
    {
        [JsonProperty("lastUpdate")]
        public string LastUpdate { get; set; }
    }
}
