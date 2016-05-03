using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class VirtualAccount
    {
        [JsonProperty("bank")]
        public string Bank { get; set; }
    }
}