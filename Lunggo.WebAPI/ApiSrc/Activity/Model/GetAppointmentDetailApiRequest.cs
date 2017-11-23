using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAppointmentDetailApiRequest
    {
        [JsonProperty("appointmentId")]
        public string AppointmentId { get; set; }
    }
}
