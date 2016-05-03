using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class TelkomselTcash
    {
        [JsonProperty("customer")]
        public string Token { get; set; }
        [JsonProperty("promo")]
        public bool PromoEnabled { get; set; }
        [JsonProperty("Is_Reversal")]
        public int Reversal { get; set; }
    }
}