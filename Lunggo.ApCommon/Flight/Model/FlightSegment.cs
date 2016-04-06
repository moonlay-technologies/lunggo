using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSegment
    {
        [JsonProperty("deptm")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("arrtm")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("dep")]
        public string DepartureAirport { get; set; }
        [JsonProperty("arr")]
        public string ArrivalAirport { get; set; }
        [JsonProperty("dur")]
        public TimeSpan Duration { get; set; }
        [JsonProperty("stpqty")]
        public int StopQuantity { get; set; }
        [JsonProperty("aircd")]
        public string AirlineCode { get; set; }
        [JsonProperty("fno")]
        public string FlightNumber { get; set; }
        [JsonProperty("opaircd")]
        public string OperatingAirlineCode { get; set; }
        [JsonProperty("crft", NullValueHandling = NullValueHandling.Ignore)]
        public string AircraftCode { get; set; }
        [JsonProperty("rbd")]
        public string Rbd { get; set; }
        [JsonProperty("deptrm", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureTerminal { get; set; }
        [JsonProperty("depct")]
        public string DepartureCity { get; set; }
        [JsonProperty("depnm")]
        public string DepartureAirportName { get; set; }
        [JsonProperty("arrtrm", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalTerminal { get; set; }
        [JsonProperty("arrct")]
        public string ArrivalCity { get; set; }
        [JsonProperty("arrnm")]
        public string ArrivalAirportName { get; set; }
        [JsonProperty("airnm")]
        public string AirlineName { get; set; }
        [JsonProperty("airlg")]
        public string AirlineLogoUrl { get; set; }
        [JsonProperty("opairnm")]
        public string OperatingAirlineName { get; set; }
        [JsonProperty("opairlg")]
        public string OperatingAirlineLogoUrl { get; set; }
        [JsonProperty("stp")]
        public List<FlightStop> Stops { get; set; }
        [JsonProperty("cbn")]
        public CabinClass CabinClass { get; set; }
        [JsonProperty("meal")]
        public bool Meal { get; set; }
        [JsonProperty("bag", NullValueHandling = NullValueHandling.Ignore)]
        public string Baggage { get; set; }
        [JsonProperty("pnr", NullValueHandling = NullValueHandling.Ignore)]
        public string Pnr { get; set; }   
        [JsonProperty("rem", NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonProperty("air")]
        public string Airport { get; set; }
        [JsonProperty("arr")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("dep")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("dur")]
        public TimeSpan Duration { get; set; }
    }
}