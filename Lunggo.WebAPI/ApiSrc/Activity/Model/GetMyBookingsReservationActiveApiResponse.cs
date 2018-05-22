using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;
using System;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetMyBookingsReservationActiveApiResponse : ApiResponseBase
    {
        [JsonProperty("myReservations", NullValueHandling = NullValueHandling.Ignore)]
        public List<BookingDetail> MyReservations { get; set; }
        [JsonProperty("mustUpdate", NullValueHandling = NullValueHandling.Ignore)]
        public bool MustUpdate { get; set; }
        [JsonProperty("lastUpdate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime LastUpdate { get; set; }
    }
}