using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Notification.Model
{
    public class DeleteRegistrationApiRequest
    {
        [JsonProperty("handle")]
        public string Handle { get; set; }
    }
}