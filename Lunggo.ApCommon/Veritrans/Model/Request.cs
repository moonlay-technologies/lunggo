using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Payment.Data;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Veritrans.Model
{
    internal class Request
    {
        [JsonProperty("payment_type")]
        public string PaymentType { get; set; }
        [JsonProperty("vtweb")]
        public VtWeb VtWeb { get; set; }
        [JsonProperty("transaction_details")]
        public TransactionDetails TransactionDetail { get; set; }
        [JsonProperty("item_details")]
        public List<ItemDetails> ItemDetail { get; set; }
    }
}
