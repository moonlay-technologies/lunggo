using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckBinDiscountApiRequest
    {
        [JsonProperty("voucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("bin")]
        public string CardNumber { get; set; }
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}