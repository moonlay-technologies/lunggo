using System.Net;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class GetReservationApiResponse : ApiResponseBase
    {
        [JsonProperty("typ", NullValueHandling = NullValueHandling.Ignore)]
        public ReservationType ReservationType { get; set; }
        [JsonProperty("fl", NullValueHandling = NullValueHandling.Ignore)]
        public FlightReservationForDisplay FlightReservation { get; set; }
    }
}