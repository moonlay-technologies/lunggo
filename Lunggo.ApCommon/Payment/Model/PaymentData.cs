using System;
using Lunggo.ApCommon.Payment.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("med")]
        public PaymentMedium Medium { get; set; }
        [JsonProperty("met")]
        public PaymentMethod Method { get; set; }
        [JsonProperty("stat")]
        public PaymentStatus Status { get; set; }
        [JsonProperty("time")]
        public DateTime? Time { get; set; }
        [JsonProperty("time_limit")]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public UniversalPaymentData Data { get; set; }
        [JsonProperty("account")]
        public string TargetAccount { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("price")]
        public decimal FinalPrice { get; set; }
        [JsonProperty("paid")]
        public decimal PaidAmount { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("refund", NullValueHandling = NullValueHandling.Ignore)]
        public Refund Refund { get; set; }
    }
}
