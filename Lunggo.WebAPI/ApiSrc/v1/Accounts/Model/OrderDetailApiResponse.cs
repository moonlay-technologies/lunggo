using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class OrderDetailApiResponse : ApiResponseBase
    {
        [JsonProperty("rsv", NullValueHandling = NullValueHandling.Ignore)]
        public FlightReservationForDisplay Reservation { get; set; }
    }
}