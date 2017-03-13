using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckPaydayMadnessApiRequest
    {
        [JsonProperty("voucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}