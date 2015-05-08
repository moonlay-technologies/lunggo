using Newtonsoft.Json;

namespace Lunggo.ApCommon.Veritrans.Model
{
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