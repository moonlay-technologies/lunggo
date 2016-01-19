using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class TelkomselTcash
    {
        [JsonProperty("customer")]
        public string Token { get; set; }
        [JsonProperty("promo")]
        public bool PromoEnabled { get; set; }
        [JsonProperty("Is_Reversal")]
        public int Reversal { get; set; }
    }
}