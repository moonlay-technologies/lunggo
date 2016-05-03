using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class IndosatDompetku
    {
        [JsonProperty("msisdn")]
        public string PhoneNumber { get; set; }
    }
}