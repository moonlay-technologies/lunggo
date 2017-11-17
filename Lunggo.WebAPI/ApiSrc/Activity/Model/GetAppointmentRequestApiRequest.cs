using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentRequestApiRequest
    {
        [JsonProperty("page")]
        public string Page { get; set; }
        [JsonProperty("perPage")]
        public string PerPage { get; set; }
    }
}
