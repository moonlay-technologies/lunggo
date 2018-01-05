using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class UniqueCodeApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
        [JsonProperty("trxId")]
        public string TrxId { get; set; }
        [JsonProperty("bin")]
        public string Bin { get; set; }
        [JsonProperty("discCd")]
        public string DiscountCode { get; set; }
    }
}