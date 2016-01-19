using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class MandiriBillPayment
    {
        [JsonProperty("bill_info1")]
        public string Label1 { get; set; }
        [JsonProperty("bill_info2")]
        public string Value1 { get; set; }
        [JsonProperty("bill_info3")]
        public string Label2 { get; set; }
        [JsonProperty("bill_info4")]
        public string Value2 { get; set; }
        [JsonProperty("bill_info5")]
        public string Label3 { get; set; }
        [JsonProperty("bill_info6")]
        public string Value3 { get; set; }
        [JsonProperty("bill_info7")]
        public string Label4 { get; set; }
        [JsonProperty("bill_info8")]
        public string Value4 { get; set; }
    }
}