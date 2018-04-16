using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model
{
    public class TransactionStatement
    {
        public string TrxNo { get; set; }
        public string Remarks { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
    }

    public class TransactionStatementForDisplay
    {
        [JsonProperty("trxNo", NullValueHandling = NullValueHandling.Ignore)]
        public string TrxNo { get; set; }
        [JsonProperty("remarks", NullValueHandling = NullValueHandling.Ignore)]
        public string Remarks { get; set; }
        [JsonProperty("dateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateTime { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
    }
}
