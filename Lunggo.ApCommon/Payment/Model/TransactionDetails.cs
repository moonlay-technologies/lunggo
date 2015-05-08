using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class TransactionDetails
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }
        [JsonProperty("gross_amount")]
        public decimal Amount { get; set; }
    }
}
