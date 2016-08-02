using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Notification.Model
{
    public class RegisterDeviceApiRequest
    {
        [JsonProperty("handle")]
        public string Handle { get; set; }
    }
}