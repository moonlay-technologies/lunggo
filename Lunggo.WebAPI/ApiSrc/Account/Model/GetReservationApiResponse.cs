using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetReservationApiResponse : ApiResponseBase
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public ProductType? ProductType { get; set; }
        [JsonProperty("flight", NullValueHandling = NullValueHandling.Ignore)]
        public FlightReservationForDisplay FlightReservation { get; set; }
        [JsonProperty("hotel", NullValueHandling = NullValueHandling.Ignore)]
        public HotelReservationForDisplay HotelReservation { get; set; }
    }
}