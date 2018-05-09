using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class VeritransResponse
        {
            [JsonProperty("status_code")] internal string StatusCode { get; set; }
            [JsonProperty("status_message")] internal string StatusMessage { get; set; }
            [JsonProperty("redirect_url")] internal string RedirectUrl { get; set; }
            [JsonProperty("transaction_id")] internal string TransactionId { get; set; }
            [JsonProperty("order_id")] internal string OrderId { get; set; }
            [JsonProperty("gross_amount")] internal string Amount { get; set; }
            [JsonProperty("payment_type")] internal string PaymentType { get; set; }
            [JsonProperty("transaction_time")] internal string TransactionTime { get; set; }
            [JsonProperty("transaction_status")] internal string TransactionStatus { get; set; }
            [JsonProperty("fraud_status")] internal string FraudStatus { get; set; }
            [JsonProperty("masked_card")] internal string MaskedCard { get; set; }
            [JsonProperty("bank")] internal string Bank { get; set; }
            [JsonProperty("approval_code")] internal string ApprovalCode { get; set; }
            [JsonProperty("eci")] internal string Eci { get; set; }
            [JsonProperty("saved_token_id")] internal string SavedTokenId { get; set; }

            [JsonProperty("saved_token_id_expired_at")]
            internal DateTime TokenIdExpiry { get; set; }

            [JsonProperty("permata_va_number")] internal string PermataVANumber { get; set; }
        }
    }
}