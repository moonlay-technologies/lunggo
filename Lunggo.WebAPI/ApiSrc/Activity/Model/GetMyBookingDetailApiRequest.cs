using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetMyBookingDetailApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}
