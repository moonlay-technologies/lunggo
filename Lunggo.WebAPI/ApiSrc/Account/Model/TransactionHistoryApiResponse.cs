﻿using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class TransactionHistoryApiResponse : ApiResponseBase
    {
        [JsonProperty("flights", NullValueHandling = NullValueHandling.Ignore)]
        public List<FlightReservationForDisplay> FlightReservations { get; set; }

        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelReservationForDisplay> HotelReservations { get; set; }
    }
}