using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Accounts.Model
{
    public class TransactionHistoryApiResponse : ApiResponseBase
    {
        [JsonProperty("trx", NullValueHandling = NullValueHandling.Ignore)]
        public List<FlightReservationForDisplay> Reservations { get; set; }
    }
}