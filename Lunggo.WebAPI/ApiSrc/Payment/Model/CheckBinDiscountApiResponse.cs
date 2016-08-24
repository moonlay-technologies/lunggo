using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckBinDiscountResponse : ApiResponseBase
    {
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Include)]
        public decimal? DiscountAmount { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Include)]
        public string DiscountName { get; set; }
    }
}