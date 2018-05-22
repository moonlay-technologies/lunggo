using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentRequestApiRequest
    {
        [JsonProperty("lastUpdate")]
        public string LastUpdate { get; set; }
    }
}
