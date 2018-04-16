using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentDetailApiRequest
    {
        [JsonProperty("activityId")]
        public string ActivityId { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("session")]
        public string Session { get; set; }
    }
}
