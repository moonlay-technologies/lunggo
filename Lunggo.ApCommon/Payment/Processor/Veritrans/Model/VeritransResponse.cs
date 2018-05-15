using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class VeritransResponse
        {
            [JsonProperty("status_code", NullValueHandling = NullValueHandling.Ignore)]
            internal string StatusCode { get; set; }
            [JsonProperty("status_message", NullValueHandling = NullValueHandling.Ignore)]
            internal string StatusMessage { get; set; }
            [JsonProperty("redirect_url", NullValueHandling = NullValueHandling.Ignore)]
            internal string RedirectUrl { get; set; }
            [JsonProperty("transaction_id", NullValueHandling = NullValueHandling.Ignore)]
            internal string TransactionId { get; set; }
            [JsonProperty("order_id", NullValueHandling = NullValueHandling.Ignore)]
            internal string OrderId { get; set; }
            [JsonProperty("gross_amount", NullValueHandling = NullValueHandling.Ignore)]
            internal string Amount { get; set; }
            [JsonProperty("payment_type", NullValueHandling = NullValueHandling.Ignore)]
            internal string PaymentType { get; set; }
            [JsonProperty("transaction_time", NullValueHandling = NullValueHandling.Ignore)]
            internal string TransactionTime { get; set; }
            [JsonProperty("transaction_status", NullValueHandling = NullValueHandling.Ignore)]
            internal string TransactionStatus { get; set; }
            [JsonProperty("fraud_status", NullValueHandling = NullValueHandling.Ignore)]
            internal string FraudStatus { get; set; }
            [JsonProperty("masked_card", NullValueHandling = NullValueHandling.Ignore)]
            internal string MaskedCard { get; set; }
            [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
            internal string Bank { get; set; }
            [JsonProperty("approval_code", NullValueHandling = NullValueHandling.Ignore)]
            internal string ApprovalCode { get; set; }
            [JsonProperty("eci", NullValueHandling = NullValueHandling.Ignore)]
            internal string Eci { get; set; }
            [JsonProperty("saved_token_id", NullValueHandling = NullValueHandling.Ignore)]
            internal string SavedTokenId { get; set; }
            [JsonProperty("validation_messages", NullValueHandling = NullValueHandling.Ignore)]
            internal string ValidationMessages { get; set; }
            [JsonProperty("saved_token_id_expired_at", NullValueHandling = NullValueHandling.Ignore)]
            internal DateTime TokenIdExpiry { get; set; }
            [JsonProperty("permata_va_number", NullValueHandling = NullValueHandling.Ignore)]
            internal string PermataVANumber { get; set; }
        }
    }
}