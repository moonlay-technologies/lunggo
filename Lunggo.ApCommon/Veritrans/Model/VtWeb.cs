using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Veritrans.Model
{
    internal class VtWeb
    {
        [JsonProperty("enabled_payments")]
        internal List<string> EnabledPayments { get; set; }
        [JsonProperty("credit_card_3d_secure")]
        internal bool CreditCard3DSecure { get; set; }
        [JsonProperty("payment_options")]
        internal string PaymentOptions { get; set; }
        [JsonProperty("credit_card_bins")]
        internal string CreditCardBins { get; set; }
        [JsonProperty("finish_redirect_url")]
        internal string FinishRedirectUrl { get; set; }
        [JsonProperty("unfinish_redirect_url")]
        internal string UnfinishRedirectUrl { get; set; }
        [JsonProperty("error_redirect_url")]
        internal string ErrorRedirectUrl { get; set; }
    }
}