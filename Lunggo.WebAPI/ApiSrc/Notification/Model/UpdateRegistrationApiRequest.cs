using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Notification.Model
{
    public class UpdateRegistrationApiRequest
    {
        [JsonProperty("registrationId")]
        public string RegistrationId { get; set; }
        [JsonProperty("handle")]
        public string Handle { get; set; }
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }
}