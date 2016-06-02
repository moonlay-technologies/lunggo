using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightStopForDisplay
    {
        [JsonProperty("airport")]
        public string Airport { get; set; }
        [JsonProperty("arrivalTime")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
    }

    public class FlightStop
    {
        public string Airport { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}