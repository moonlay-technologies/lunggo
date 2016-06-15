using System;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightItineraryApiResponse : ApiResponseBase
    {
        [JsonProperty("itin", NullValueHandling = NullValueHandling.Ignore)]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("expTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
    }
}