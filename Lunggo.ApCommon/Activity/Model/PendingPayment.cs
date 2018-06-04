using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model
{
    public class PendingPayment
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string PaymentStatus { get; set; }
        public string RsvNo { get; set; }
    }

    public class PendingPaymentForDisplay
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("accountNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }
        [JsonProperty("bankName", NullValueHandling = NullValueHandling.Ignore)]
        public string BankName { get; set; }
        [JsonProperty("paymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentStatus { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
    }
}
