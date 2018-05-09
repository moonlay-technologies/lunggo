using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class CimbClicks
        {
            [JsonProperty("description")] public string Description { get; set; }
            [JsonProperty("finish_redirect_url")] internal string FinishRedirectUrl { get; set; }

            [JsonProperty("unfinish_redirect_url")]
            internal string UnfinishRedirectUrl { get; set; }

            [JsonProperty("error_redirect_url")] internal string ErrorRedirectUrl { get; set; }
        }
    }
}