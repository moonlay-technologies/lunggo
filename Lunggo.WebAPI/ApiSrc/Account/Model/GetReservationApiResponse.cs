using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetReservationApiResponse : ApiResponseBase
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public ProductType ProductType { get; set; }
        [JsonProperty("flight", NullValueHandling = NullValueHandling.Ignore)]
        public FlightReservationForDisplay FlightReservation { get; set; }
    }
}