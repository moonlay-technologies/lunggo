using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class GetBookingDisabilityStatusResponse : ApiResponseBase
    {
        [JsonProperty("isPaymentDisabled")]
        public bool? IsPaymentDisabled { get; set; }
    }
}