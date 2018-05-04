using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class IndosatDompetku
        {
            [JsonProperty("msisdn")] public string PhoneNumber { get; set; }
        }
    }
}