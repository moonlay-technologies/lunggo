using Lunggo.ApCommon.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetReservationListApiResponse : ApiResponseBase
    {
        [JsonProperty("reservations", NullValueHandling = NullValueHandling.Ignore)]
        public List<ReservationListForDisplay> Reservations { get; set; }
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
        [JsonProperty("perPage", NullValueHandling = NullValueHandling.Ignore)]
        public int? PerPage { get; set; }
    }
}