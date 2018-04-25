using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentDetailsForDisplay
    {
        [JsonProperty("medium", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMedium Medium { get; set; }
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod Method { get; set; }
        [JsonProperty("submethod", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentSubmethod Submethod { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentStatus Status { get; set; }
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Time { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("transferAccount", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferAccount { get; set; }
        [JsonProperty("redirectionUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectionUrl { get; set; }
        [JsonProperty("originalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalPrice { get; set; }
        [JsonProperty("discountCode", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountCode { get; set; }
        [JsonProperty("discountNominal", NullValueHandling = NullValueHandling.Ignore)]
        public decimal DiscountNominal { get; set; }
        [JsonProperty("discountName", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountName { get; set; }
        [JsonProperty("uniqueCode", NullValueHandling = NullValueHandling.Ignore)]
        public decimal UniqueCode { get; set; }
        [JsonProperty("transferFee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TransferFee { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
        [JsonProperty("finalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal FinalPrice { get; set; }
        [JsonProperty("refund", NullValueHandling = NullValueHandling.Ignore)]
        public RefundForDisplay Refund { get; set; }
        [JsonProperty("invoice", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceNo { get; set; }
    }

    public class PaymentDetails
    {
        public string RsvNo { get; set; }
        public PaymentMedium Medium { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentSubmethod Submethod { get; set; }
        public PaymentStatus Status { get; set; }
        public FailureReason FailureReason { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? TimeLimit { get; set; }
        public PaymentData Data { get; set; }
        public string TransferAccount { get; set; }
        public string RedirectionUrl { get; set; }
        public string ExternalId { get; set; }
        public decimal OriginalPriceIdr { get; set; }
        public string DiscountCode { get; set; }
        public decimal DiscountNominal { get; set; }
        public UsedDiscount Discount { get; set; }
        public decimal Surcharge { get; set; }
        public decimal UniqueCode { get; set; }
        public decimal FinalPriceIdr { get; set; }
        public decimal PaidAmountIdr { get; set; }
        public Currency LocalCurrency { get; set; }
        public decimal LocalFinalPrice { get; set; }
        public decimal LocalPaidAmount { get; set; }
        public Refund Refund { get; set; }
        public string InvoiceNo { get; set; }
    }

    public class CartPaymentDetails : PaymentDetails
    {
        public List<PaymentDetails> RsvPaymentDetails { get; set; }
        public string CartId { get; set; }
    }
}
