using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class MandiriEcash
    {
        [JsonProperty("bank")]
        public string Bank { get; set; }
    }
}