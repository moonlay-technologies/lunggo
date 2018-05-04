using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class BcaKlikPay
        {
            [JsonProperty("type")] public int Type { get; set; }
            [JsonProperty("misc_fee")] public int MiscFee { get; set; }
            [JsonProperty("description")] public string Description { get; set; }
        }
    }
}