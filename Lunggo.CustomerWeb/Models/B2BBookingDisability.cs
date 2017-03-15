using Newtonsoft.Json;

namespace Lunggo.CustomerWeb.Models
{
    public class GetBookingDisabilityStatusResponse
    {
        [JsonProperty("isBookingDisabled")]
        public bool? IsBookingDisabled { get; set; }
    }


}