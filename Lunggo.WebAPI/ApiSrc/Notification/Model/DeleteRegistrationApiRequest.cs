using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Notification.Model
{
    public class DeleteRegistrationApiRequest
    {
        [JsonProperty("registrationId")]
        public string RegistrationId { get; set; }
    }
}