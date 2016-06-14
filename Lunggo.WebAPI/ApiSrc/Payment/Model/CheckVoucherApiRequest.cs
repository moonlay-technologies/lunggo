using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckVoucherApiRequest
    {
        [JsonProperty("code")]
        public string DiscountCode { get; set; }
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}