using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ConfirmationStatusApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("cancellationReason")]
        public string CancellationReason { get; set; }
    }
}
