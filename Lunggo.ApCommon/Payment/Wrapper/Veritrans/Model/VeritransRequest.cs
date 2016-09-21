using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class VeritransRequest
    {
        [JsonProperty("payment_type")]
        internal string PaymentType { get; set; }
        [JsonProperty("vtweb")]
        internal VtWeb VtWeb { get; set; }
        [JsonProperty("credit_card")]
        internal CreditCard CreditCard { get; set; }
        [JsonProperty("bank_transfer")]
        internal BankTransfer BankTransfer { get; set; }
        [JsonProperty("echannel")]
        internal MandiriBillPayment MandiriBillPayment { get; set; }
        [JsonProperty("mandiri_clickpay")]
        internal MandiriClickPay MandiriClickPay { get; set; }
        [JsonProperty("cimb_clicks")]
        internal CimbClicks CimbClicks { get; set; }
        [JsonProperty("bca_klikpay")]
        internal BcaKlikPay BcaKlikPay{ get; set; }
        [JsonProperty("telkomsel_cash")]
        internal TelkomselTcash TelkomselTcash { get; set; }
        [JsonProperty("indosat_dompetku")]
        internal IndosatDompetku IndosatDompetku { get; set; }
        [JsonProperty("mandiri_ecash")]
        internal MandiriEcash MandiriEcash { get; set; }
        [JsonProperty("cstore")]
        internal Indomaret Indomaret { get; set; }
        [JsonProperty("custom_expiry")]
        internal PaymentExpiry PaymentExpiry { get; set; }
        [JsonProperty("transaction_details")]
        internal TransactionDetails TransactionDetail { get; set; }
        [JsonProperty("item_details")]
        internal List<ItemDetails> ItemDetail { get; set; }
        [JsonProperty("customer_details")]
        internal CustomerDetails CustomerDetail { get; set; }
    }
}
