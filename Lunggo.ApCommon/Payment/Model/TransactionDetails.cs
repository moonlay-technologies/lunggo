using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class TransactionDetails
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("order_time")]
        public DateTime OrderTime { get; set; }
        [JsonProperty("gross_amount")]
        public long Amount { get; set; }
    }
}
