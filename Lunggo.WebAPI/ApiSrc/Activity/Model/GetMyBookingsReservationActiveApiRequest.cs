using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetMyBookingsReservationActiveApiRequest
    {
        [JsonProperty("lastUpdate")]
        public string LastUpdate { get; set; }
    }
}
