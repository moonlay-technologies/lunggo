using Newtonsoft.Json;

namespace Lunggo.CustomerWeb.Models
{
    public class GetBookingDisabilityStatusResponse
    {
        [JsonProperty("isPaymentDisabled")]
        public bool? IsPaymentDisabled { get; set; }
    }


}