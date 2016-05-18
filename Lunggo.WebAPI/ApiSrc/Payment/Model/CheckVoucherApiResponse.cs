using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckVoucherApiResponse : ApiResponseBase
    {
        [JsonProperty("discount")]
        public decimal Discount { get; set; }
        [JsonProperty("name")]
        public string DisplayName { get; set; }
    }
}