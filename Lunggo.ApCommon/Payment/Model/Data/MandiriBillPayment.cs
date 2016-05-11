using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class MandiriBillPayment
    {
        [JsonProperty("label1")]
        public string Label1 { get; set; }
        [JsonProperty("value1")]
        public string Value1 { get; set; }
        [JsonProperty("label2")]
        public string Label2 { get; set; }
        [JsonProperty("value2")]
        public string Value2 { get; set; }
        [JsonProperty("label3")]
        public string Label3 { get; set; }
        [JsonProperty("value3")]
        public string Value3 { get; set; }
        [JsonProperty("label4")]
        public string Label4 { get; set; }
        [JsonProperty("value4")]
        public string Value4 { get; set; }
    }
}