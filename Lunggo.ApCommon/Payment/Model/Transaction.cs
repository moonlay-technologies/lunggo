using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Transaction
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Time { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
        [JsonProperty("balanceAfter", NullValueHandling = NullValueHandling.Ignore)]
        public decimal BalanceAfter { get; set; }
        [JsonProperty("remark", NullValueHandling = NullValueHandling.Ignore)]
        public string Remark { get; set; }
    }
}
