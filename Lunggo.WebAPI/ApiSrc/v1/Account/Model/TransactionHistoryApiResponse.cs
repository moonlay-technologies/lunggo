using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Account.Model
{
    public class TransactionHistoryApiResponse
    {
        [JsonProperty("trx", NullValueHandling = NullValueHandling.Ignore)]
        public List<FlightReservationForDisplay> Reservations { get; set; }
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
    }
}