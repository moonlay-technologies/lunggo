using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightTripForDisplay
    {
        [JsonProperty("transitCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalTransit { get; set; }
        [JsonProperty("transits", NullValueHandling = NullValueHandling.Ignore)]
        public List<Transit> Transits { get; set; }
        [JsonProperty("airlines", NullValueHandling = NullValueHandling.Ignore)]
        public List<Airline> Airlines { get; set; }
        [JsonProperty("totalDuration", NullValueHandling = NullValueHandling.Ignore)]
        public double? TotalDuration { get; set; }
        [JsonProperty("origin", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginAirport { get; set; }
        [JsonProperty("originName", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginAirportName { get; set; }
        [JsonProperty("originCity", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginCity { get; set; }
        [JsonProperty("destination", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationAirport { get; set; }
        [JsonProperty("destinationName", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationAirportName { get; set; }
        [JsonProperty("destinationCity", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationCity { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DepartureDate { get; set; }
        [JsonProperty("segments", NullValueHandling = NullValueHandling.Ignore)]

       public List<FlightSegmentForDisplay> Segments { get; set; }
        [JsonProperty("originalAdultFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalAdultFare { get; set; }
        [JsonProperty("originalChildFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalChildFare { get; set; }
        [JsonProperty("originalInfantFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalInfantFare { get; set; }

        [JsonProperty("netAdultFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetAdultFare { get; set; }
        [JsonProperty("netChildFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetChildFare { get; set; }
        [JsonProperty("netInfantFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetInfantFare { get; set; }

        [JsonProperty("originalTotalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalTotalFare { get; set; }
        [JsonProperty("netTotalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetTotalFare { get; set; }
    }

    public class FlightTrip
    {
        public string OriginAirport { get; set; }
        public string OriginAirportName { get; set; }
        public string OriginCity { get; set; }
        public string DestinationAirport { get; set; }
        public string DestinationAirportName { get; set; }
        public string DestinationCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<FlightSegment> Segments { get; set; }
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

    public class Airline
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }
        [JsonProperty("logoUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string LogoUrl { get; set; }
    }

    public class Transit
    {
        [JsonProperty("airport", NullValueHandling = NullValueHandling.Ignore)]
        public string Airport { get; set; }
        [JsonProperty("arrivalTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ArrivalTime { get; set; }
        [JsonProperty("departureTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DepartureTime { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public double Duration { get; set; }
    }


}
