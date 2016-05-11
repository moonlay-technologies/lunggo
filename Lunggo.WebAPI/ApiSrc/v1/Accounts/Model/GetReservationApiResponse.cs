using System.Net;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class GetReservationApiResponse : ApiResponseBase
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public ProductType ProductType { get; set; }
        [JsonProperty("flight", NullValueHandling = NullValueHandling.Ignore)]
        public FlightReservationForDisplay FlightReservation { get; set; }
    }
}