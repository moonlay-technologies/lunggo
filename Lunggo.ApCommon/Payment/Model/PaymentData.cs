using System;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentData
    {
        [JsonProperty("id")]
        public string ExternalId { get; set; }
        [JsonProperty("med")]
        public PaymentMedium Medium { get; set; }
        [JsonProperty("met")]
        public PaymentMethod Method { get; set; }
        [JsonProperty("st")]
        public PaymentStatus Status { get; set; }
        [JsonProperty("tm")]
        public DateTime? Time { get; set; }
        [JsonProperty("lim")]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public UniversalPaymentData Data { get; set; }
        [JsonProperty("account")]
        public string TransferAccount { get; set; }
        [JsonProperty("url")]
        public string RedirectionUrl { get; set; }
        [JsonProperty("curr")]
        public string Currency { get; set; }
        public decimal OriginalPrice { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountNominal { get; set; }
        public Discount Discount { get; set; }
        public decimal TransferFee { get; set; }
        public decimal FinalPrice { get; set; }
        [JsonProperty("paid")]
        public decimal PaidAmount { get; set; }
        [JsonProperty("ref", NullValueHandling = NullValueHandling.Ignore)]
        public Refund Refund { get; set; }
        public string InvoiceNo { get; set; }
    }
}
