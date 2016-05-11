using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class TelkomselTcash
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("promo")]
        public bool PromoEnabled { get; set; }
        [JsonProperty("reversal")]
        public int Reversal { get; set; }
    }
}