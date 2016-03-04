using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSegment
    {
        [JsonProperty("dep_time")]
        public DateTime DepartureTime { get; set; }
        [JsonProperty("arr_time")]
        public DateTime ArrivalTime { get; set; }
        [JsonProperty("dep")]
        public string DepartureAirport { get; set; }
        [JsonProperty("arr")]
        public string ArrivalAirport { get; set; }
        [JsonProperty("dur")]
        public TimeSpan Duration { get; set; }
        [JsonProperty("stop_qty")]
        public int StopQuantity { get; set; }
        [JsonProperty("air_code")]
        public string AirlineCode { get; set; }
        [JsonProperty("f_no")]
        public string FlightNumber { get; set; }
        [JsonProperty("op_air_code")]
        public string OperatingAirlineCode { get; set; }
        [JsonProperty("craft", NullValueHandling = NullValueHandling.Ignore)]
        public string AircraftCode { get; set; }
        [JsonProperty("rbd")]
        public string Rbd { get; set; }
        [JsonProperty("dep_term", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureTerminal { get; set; }
        [JsonProperty("dep_city")]
        public string DepartureCity { get; set; }
        [JsonProperty("dep_name")]
        public string DepartureAirportName { get; set; }
        [JsonProperty("arr_term", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalTerminal { get; set; }
        [JsonProperty("arr_city")]
        public string ArrivalCity { get; set; }
        [JsonProperty("arr_name")]
        public string ArrivalAirportName { get; set; }
        [JsonProperty("air_name")]
        public string AirlineName { get; set; }
        [JsonProperty("air_logo_url")]
        public string AirlineLogoUrl { get; set; }
        [JsonProperty("op_air_name")]
        public string OperatingAirlineName { get; set; }
        [JsonProperty("op_air_logo_url")]
        public string OperatingAirlineLogoUrl { get; set; }
        [JsonProperty("stops")]
        public List<FlightStop> Stops { get; set; }
        [JsonProperty("cabin")]
        public CabinClass CabinClass { get; set; }
        [JsonProperty("meal")]
        public bool Meal { get; set; }
        [JsonProperty("bagg", NullValueHandling = NullValueHandling.Ignore)]
        public string Baggage { get; set; }
        [JsonProperty("pnr", NullValueHandling = NullValueHandling.Ignore)]
        public string Pnr { get; set; }
        [JsonProperty("rem_seats", NullValueHandling = NullValueHandling.Ignore)]
        public int RemainingSeats { get; set; }

    }

    public class FlightStop
    {
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