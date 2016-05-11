using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class IndosatDompetku
    {
        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }
    }
}