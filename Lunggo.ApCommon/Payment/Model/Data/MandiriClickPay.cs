using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class MandiriClickPay
    {
        [JsonProperty("cardNo")]
        public string CardNumber { get; set; }
        [JsonProperty("cardLast10No")]
        public string CardNumberLast10 { get; set; }
        [JsonProperty("amount")]
        public long Amount { get; set; }
        [JsonProperty("randomNumber")]
        public string GivenRandomNumber { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}