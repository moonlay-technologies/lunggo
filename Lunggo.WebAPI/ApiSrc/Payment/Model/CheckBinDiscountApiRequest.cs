using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckBinDiscountApiRequest
    {
        [JsonProperty("discountCode")]
        public string DiscountCode { get; set; }
        [JsonProperty("bin")]
        public string CardNumber { get; set; }
    }
}