using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightStopForDisplay
    {
        [JsonProperty("airport", NullValueHandling = NullValueHandling.Ignore)]
        public string Airport { get; set; }
        [JsonProperty("arrivalTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ArrivalTime { get; set; }
        [JsonProperty("departureTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DepartureTime { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }
    }

    public class FlightStop
    {
        public string Airport { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}