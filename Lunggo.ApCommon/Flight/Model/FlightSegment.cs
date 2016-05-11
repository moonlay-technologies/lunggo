using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSegment
    {
        [JsonProperty("departureTime")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("arrivalTime")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("departure")]
        public string DepartureAirport { get; set; }
        [JsonProperty("arrival")]
        public string ArrivalAirport { get; set; }
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }
        [JsonProperty("stopQty")]
        public int StopQuantity { get; set; }
        [JsonProperty("airlineCd")]
        public string AirlineCode { get; set; }
        [JsonProperty("flightNo")]
        public string FlightNumber { get; set; }
        [JsonProperty("opAirlineCd")]
        public string OperatingAirlineCode { get; set; }
        [JsonProperty("aircraft", NullValueHandling = NullValueHandling.Ignore)]
        public string AircraftCode { get; set; }
        [JsonProperty("rbd")]
        public string Rbd { get; set; }
        [JsonProperty("departureTerminal", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureTerminal { get; set; }
        [JsonProperty("departureCity")]
        public string DepartureCity { get; set; }
        [JsonProperty("departureName")]
        public string DepartureAirportName { get; set; }
        [JsonProperty("arrivalTerminal", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalTerminal { get; set; }
        [JsonProperty("arrivalCity")]
        public string ArrivalCity { get; set; }
        [JsonProperty("arrivalName")]
        public string ArrivalAirportName { get; set; }
        [JsonProperty("airlineName")]
        public string AirlineName { get; set; }
        [JsonProperty("airlineLogoUrl")]
        public string AirlineLogoUrl { get; set; }
        [JsonProperty("opAirlineName")]
        public string OperatingAirlineName { get; set; }
        [JsonProperty("opAirlineLogoUrl")]
        public string OperatingAirlineLogoUrl { get; set; }
        [JsonProperty("stops")]
        public List<FlightStop> Stops { get; set; }
        [JsonIgnore]
        public AirlineType AirlineType { get; set; }
        [JsonProperty("cabin")]
        public CabinClass CabinClass { get; set; }
        [JsonProperty("meal")]
        public bool Meal { get; set; }
        [JsonProperty("baggage", NullValueHandling = NullValueHandling.Ignore)]
        public string Baggage { get; set; }
        [JsonProperty("pnr", NullValueHandling = NullValueHandling.Ignore)]
        public string Pnr { get; set; }   
        [JsonProperty("remainingSeats", NullValueHandling = NullValueHandling.Ignore)]
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

    public class FlightStop
    {
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