using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightTripForDisplay : FlightTripBase
    {
        [JsonProperty("tot_transit")]
        public int TotalTransit { get; set; }
        [JsonProperty("transits")]
        public List<Transit> Transits { get; set; }
        [JsonProperty("airs")]
        public List<Airline> Airlines { get; set; }
        [JsonProperty("tot_dur")]
        public TimeSpan TotalDuration { get; set; }
    }

    public class FlightTrip : FlightTripBase
    {

    }

    public class FlightTripBase
    {
        [JsonProperty("ori")]
        public string OriginAirport { get; set; }
        [JsonProperty("des")]
        public string DestinationAirport { get; set; }
        [JsonProperty("des_city")]
        public string DestinationCity { get; set; }
        [JsonProperty("des_name")]
        public string DestinationAirportName { get; set; }
        [JsonProperty("ori_city")]
        public string OriginCity { get; set; }
        [JsonProperty("ori_name")]
        public string OriginAirportName { get; set; }
        [JsonProperty("date")]
        public DateTime DepartureDate { get; set; }
        [JsonProperty("segments")]
        public List<FlightSegment> Segments { get; set; }

        public bool Identical(FlightTrip otherTrip)
        {
            if (OriginAirport != otherTrip.OriginAirport ||
                DestinationAirport != otherTrip.DestinationAirport ||
                DepartureDate != otherTrip.DepartureDate ||
                Segments == null ||
                otherTrip.Segments == null ||
                Segments.Count != otherTrip.Segments.Count)
                return false;
            for (var i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];
                var otherSegment = otherTrip.Segments[i];
                if (segment.DepartureAirport != otherSegment.DepartureAirport ||
                    segment.ArrivalAirport != otherSegment.ArrivalAirport ||
                    segment.DepartureTime != otherSegment.DepartureTime ||
                    segment.ArrivalTime != otherSegment.ArrivalTime ||
                    segment.Duration != otherSegment.Duration ||
                    segment.AirlineCode != otherSegment.AirlineCode ||
                    segment.FlightNumber != otherSegment.FlightNumber ||
                    segment.CabinClass != otherSegment.CabinClass)
                    return false;
            }
            return true;
        }
    }

    public class Airline
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }
    }

    public class Transit
    {
        [JsonProperty("stop")]
        public bool IsStop { get; set; }
        [JsonProperty("airport")]
        public string Airport { get; set; }
        [JsonProperty("arr_time")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("dep_time")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("dur")]
        public TimeSpan Duration { get; set; }
    }


}
