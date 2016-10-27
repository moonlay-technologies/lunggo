using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRateApiRequest
    {
        [JsonProperty("hotelCode")]
        public int HotelCode { get; set; }
        [JsonProperty("searchId")]
        public string SearchId { get; set; }       
    } 
}
