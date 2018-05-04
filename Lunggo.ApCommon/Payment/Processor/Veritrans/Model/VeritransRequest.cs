using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class VeritransRequest
        {
            [JsonProperty("payment_type", NullValueHandling = NullValueHandling.Ignore)]
            internal string PaymentType { get; set; }

            [JsonProperty("vtweb", NullValueHandling = NullValueHandling.Ignore)]
            internal VtWeb VtWeb { get; set; }

            [JsonProperty("credit_card", NullValueHandling = NullValueHandling.Ignore)]
            internal CreditCard CreditCard { get; set; }

            [JsonProperty("bank_transfer", NullValueHandling = NullValueHandling.Ignore)]
            internal BankTransfer BankTransfer { get; set; }

            [JsonProperty("echannel", NullValueHandling = NullValueHandling.Ignore)]
            internal MandiriBillPayment MandiriBillPayment { get; set; }

            [JsonProperty("mandiri_clickpay", NullValueHandling = NullValueHandling.Ignore)]
            internal MandiriClickPay MandiriClickPay { get; set; }

            [JsonProperty("cimb_clicks", NullValueHandling = NullValueHandling.Ignore)]
            internal CimbClicks CimbClicks { get; set; }

            [JsonProperty("bca_klikpay", NullValueHandling = NullValueHandling.Ignore)]
            internal BcaKlikPay BcaKlikPay { get; set; }

            [JsonProperty("telkomsel_cash", NullValueHandling = NullValueHandling.Ignore)]
            internal TelkomselTcash TelkomselTcash { get; set; }

            [JsonProperty("indosat_dompetku", NullValueHandling = NullValueHandling.Ignore)]
            internal IndosatDompetku IndosatDompetku { get; set; }

            [JsonProperty("mandiri_ecash", NullValueHandling = NullValueHandling.Ignore)]
            internal MandiriEcash MandiriEcash { get; set; }

            [JsonProperty("cstore", NullValueHandling = NullValueHandling.Ignore)]
            internal Indomaret Indomaret { get; set; }

            [JsonProperty("custom_expiry", NullValueHandling = NullValueHandling.Ignore)]
            internal PaymentExpiry PaymentExpiry { get; set; }

            [JsonProperty("transaction_details", NullValueHandling = NullValueHandling.Ignore)]
            internal TransactionDetails TransactionDetail { get; set; }

            [JsonProperty("item_details", NullValueHandling = NullValueHandling.Ignore)]
            internal List<ItemDetails> ItemDetail { get; set; }

            [JsonProperty("customer_details", NullValueHandling = NullValueHandling.Ignore)]
            internal CustomerDetails CustomerDetail { get; set; }
        }
    }
}