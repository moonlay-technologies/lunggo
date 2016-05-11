using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightTripForDisplay : FlightTripBase
    {
        [JsonProperty("transitCount")]
        public int TotalTransit { get; set; }
        [JsonProperty("transits")]
        public List<Transit> Transits { get; set; }
        [JsonProperty("airlines")]
        public List<Airline> Airlines { get; set; }
        [JsonProperty("totalDuration")]
        public TimeSpan TotalDuration { get; set; }
    }

    public class FlightTrip : FlightTripBase
    {
        public bool Identical(FlightTrip otherTrip)
        {
            return
                OriginAirport == otherTrip.OriginAirport &&
                DestinationAirport == otherTrip.DestinationAirport &&
                DepartureDate == otherTrip.DepartureDate &&
                Segments.Count == otherTrip.Segments.Count &&
                Segments.Zip(otherTrip.Segments, (segment, otherSegment) => segment.Identical(otherSegment)).All(x => x);
        }
    }

    public class FlightTripBase
    {
        [JsonProperty("origin")]
        public string OriginAirport { get; set; }
        [JsonProperty("originName")]
        public string OriginAirportName { get; set; }
        [JsonProperty("originCity")]
        public string OriginCity { get; set; }
        [JsonProperty("destination")]
        public string DestinationAirport { get; set; }
        [JsonProperty("destinationName")]
        public string DestinationAirportName { get; set; }
        [JsonProperty("destinationCity")]
        public string DestinationCity { get; set; }
        [JsonProperty("date")]
        public DateTime DepartureDate { get; set; }
        [JsonProperty("segments")]
        public List<FlightSegment> Segments { get; set; }
    }

    public class Airline
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logoUrl")]
        public string LogoUrl { get; set; }
    }

    public class Transit
    {
        [JsonProperty("stop")]
        public bool IsStop { get; set; }
        [JsonProperty("airport")]
        public string Airport { get; set; }
        [JsonProperty("arrivalTime")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }
    }


}
