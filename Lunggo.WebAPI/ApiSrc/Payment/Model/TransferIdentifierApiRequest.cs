using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class TransferIdentifierApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}