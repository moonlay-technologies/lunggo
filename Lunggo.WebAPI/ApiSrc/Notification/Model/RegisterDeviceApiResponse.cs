using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Notification.Model
{
    public class RegisterDeviceApiResponse : ApiResponseBase
    {
        [JsonProperty("registrationId", NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationId { get; set; }
    }
}