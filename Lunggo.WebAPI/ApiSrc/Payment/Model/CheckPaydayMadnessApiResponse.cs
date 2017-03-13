using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class CheckPaydayMadnessResponse : ApiResponseBase
    {
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? DiscountAmount { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string DiscountName { get; set; }
        [JsonProperty("isAvailable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAvailable { get; set; }
        [JsonProperty("replaceOriginalDiscount", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReplaceOriginalDiscount { get; set; }
    }
}