using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public TransactionDetail TransactionDetail { get; set; }
        [JsonProperty("item_details")]
        public List<ItemDetail> ItemDetail { get; set; }
    }

    internal class VtWeb
    {
        [JsonProperty("enabled_payments")]
        public string EnabledPayments { get; set; }
        [JsonProperty("credit_card_3d_secure")]
        public bool CreditCard3DSecure { get; set; }
        [JsonProperty("payment_options")]
        public string PaymentOptions { get; set; }
        [JsonProperty("credit_card_bins")]
        public string CreditCardBins { get; set; }
    }
}
