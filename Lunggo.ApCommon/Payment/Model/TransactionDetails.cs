using System;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class TransactionDetails
    {
        [JsonProperty("order_id")]
        public string TrxId { get; set; }
        [JsonProperty("order_time")]
        public DateTime OrderTime { get; set; }
        [JsonProperty("gross_amount")]
        public long Amount { get; set; }
        [JsonIgnore]
        public Contact Contact { get; set; }
    }
}
