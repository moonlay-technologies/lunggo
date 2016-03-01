using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Refund
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("bank")]
        public string TargetBank { get; set; }
        [JsonProperty("account")]
        public string TargetAccount { get; set; }
    }
}