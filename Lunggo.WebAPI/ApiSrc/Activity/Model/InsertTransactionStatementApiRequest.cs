using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class InsertTransactionStatementApiRequest
    {
        [JsonProperty("remarks", NullValueHandling = NullValueHandling.Ignore)]
        public string Remarks { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public Decimal Amount { get; set; }
        [JsonProperty("operatorId", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorId { get; set; }
        [JsonProperty("dateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateTime { get; set; }
    }
}