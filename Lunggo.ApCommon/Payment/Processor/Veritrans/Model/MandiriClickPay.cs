using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class MandiriClickPay
        {
            [JsonProperty("card_number")] public string CardNumber { get; set; }
            [JsonProperty("input1")] public string CardNumberLast10 { get; set; }
            [JsonProperty("input2")] public long Amount { get; set; }
            [JsonProperty("input3")] public string GivenRandomNumber { get; set; }
            [JsonProperty("token")] public string Token { get; set; }
        }
    }
}