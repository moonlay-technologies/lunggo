using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckBinDiscountApiRequest
    {
        [JsonProperty("voucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("bin")]
        public string Bin { get; set; }
        [JsonProperty("hashedPan")]
        public string HashedPan { get; set; }
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}