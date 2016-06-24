using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSegmentForDisplay
    {
        [JsonProperty("departureTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DepartureTime { get; set; }
        [JsonProperty("arrivalTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ArrivalTime { get; set; }
        [JsonProperty("departure", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureAirport { get; set; }
        [JsonProperty("arrival", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalAirport { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public double? Duration { get; set; }
        [JsonProperty("stopQty", NullValueHandling = NullValueHandling.Ignore)]
        public int? StopQuantity { get; set; }
        [JsonProperty("airlineCd", NullValueHandling = NullValueHandling.Ignore)]
        public string AirlineCode { get; set; }
        [JsonProperty("flightNo", NullValueHandling = NullValueHandling.Ignore)]
        public string FlightNumber { get; set; }
        [JsonProperty("opAirlineCd", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatingAirlineCode { get; set; }
        [JsonProperty("aircraft", NullValueHandling = NullValueHandling.Ignore)]
        public string AircraftCode { get; set; }
        [JsonProperty("rbd", NullValueHandling = NullValueHandling.Ignore)]
        public string Rbd { get; set; }
        [JsonProperty("departureTerminal", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureTerminal { get; set; }
        [JsonProperty("departureCity", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureCity { get; set; }
        [JsonProperty("departureName", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureAirportName { get; set; }
        [JsonProperty("arrivalTerminal", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalTerminal { get; set; }
        [JsonProperty("arrivalCity", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalCity { get; set; }
        [JsonProperty("arrivalName", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalAirportName { get; set; }
        [JsonProperty("airlineName", NullValueHandling = NullValueHandling.Ignore)]
        public string AirlineName { get; set; }
        [JsonProperty("airlineLogoUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string AirlineLogoUrl { get; set; }
        [JsonProperty("opAirlineName", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatingAirlineName { get; set; }
        [JsonProperty("opAirlineLogoUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatingAirlineLogoUrl { get; set; }
        [JsonProperty("stops", NullValueHandling = NullValueHandling.Ignore)]
        public List<FlightStopForDisplay> Stops { get; set; }
        [JsonProperty("cabin", NullValueHandling = NullValueHandling.Ignore)]
        public CabinClass? CabinClass { get; set; }
        [JsonProperty("hasMeal", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMealIncluded { get; set; }
        [JsonProperty("includingPsc", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPscIncluded { get; set; }
        [JsonProperty("baggage", NullValueHandling = NullValueHandling.Ignore)]
        public string Baggage { get; set; }
        [JsonProperty("pnr", NullValueHandling = NullValueHandling.Ignore)]
        public string Pnr { get; set; }
        [JsonProperty("remainingSeats", NullValueHandling = NullValueHandling.Ignore)]
        public int? RemainingSeats { get; set; }
    }

    public class FlightSegment
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public TimeSpan Duration { get; set; }
        public int StopQuantity { get; set; }
        public string AirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public string OperatingAirlineCode { get; set; }
        public string AircraftCode { get; set; }
        public string Rbd { get; set; }
        public string DepartureTerminal { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalTerminal { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalAirportName { get; set; }
        public string AirlineName { get; set; }
        public string AirlineLogoUrl { get; set; }
        public string OperatingAirlineName { get; set; }
        public string OperatingAirlineLogoUrl { get; set; }
        public List<FlightStop> Stops { get; set; }
        public AirlineType AirlineType { get; set; }
        public CabinClass CabinClass { get; set; }
        public bool IsMealIncluded { get; set; }
        public bool IsPscIncluded { get; set; }
        public string Baggage { get; set; }
        public string Pnr { get; set; }   
        public int RemainingSeats { get; set; }
        
        public bool Identical(FlightSegment otherSegment)
        {
            return
                DepartureAirport == otherSegment.DepartureAirport &&
                ArrivalAirport == otherSegment.ArrivalAirport &&
                DepartureTime == otherSegment.DepartureTime &&
                ArrivalTime == otherSegment.ArrivalTime &&
                Duration == otherSegment.Duration &&
                AirlineCode == otherSegment.AirlineCode &&
                FlightNumber == otherSegment.FlightNumber &&
                CabinClass == otherSegment.CabinClass;
        }
    }
}