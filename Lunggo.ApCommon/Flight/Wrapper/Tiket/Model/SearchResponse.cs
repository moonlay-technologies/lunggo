using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    public class SearchResponse : TiketBaseResponse
    {
        [JsonProperty("departures", NullValueHandling = NullValueHandling.Ignore)]
        public Departure Departures { get; set; }
    }

    public class Departure
    {
        [JsonProperty("result", NullValueHandling = NullValueHandling.Ignore)]
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("flight_id", NullValueHandling = NullValueHandling.Ignore)]
        public string FlightId { get; set; }
        [JsonProperty("airlines_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AirlinesName { get; set; }
        [JsonProperty("flight_number", NullValueHandling = NullValueHandling.Ignore)]
        public string FlighNumber { get; set; }
        [JsonProperty("departure_city", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureCity { get; set; }
        [JsonProperty("arrival_city", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalCity { get; set; }
        [JsonProperty("stop", NullValueHandling = NullValueHandling.Ignore)]
        public string Stop { get; set; }
        [JsonProperty("price_value", NullValueHandling = NullValueHandling.Ignore)]
        public decimal PriceValue { get; set; }
        [JsonProperty("price_adult", NullValueHandling = NullValueHandling.Ignore)]
        public decimal PriceAdult { get; set; }
        [JsonProperty("price_child", NullValueHandling = NullValueHandling.Ignore)]
        public decimal PriceChild { get; set; }
        [JsonProperty("price_infant", NullValueHandling = NullValueHandling.Ignore)]
        public decimal PriceInfant { get; set; }

        [JsonProperty("has_food", NullValueHandling = NullValueHandling.Ignore)]
        public string HasFood { get; set; }
        [JsonProperty("multiplier", NullValueHandling = NullValueHandling.Ignore)]
        public string Multiplier { get; set; }
        [JsonProperty("check_in_baggage", NullValueHandling = NullValueHandling.Ignore)]
        public int Baggage { get; set; }
        [JsonProperty("airport_tax", NullValueHandling = NullValueHandling.Ignore)]
        public bool AirportTax { get; set; }

        [JsonProperty("simple_departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureTime { get; set; }
        [JsonProperty("simple_arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalTime { get; set; }

        [JsonProperty("departure_city_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureCityName { get; set; }
        [JsonProperty("arrival_city_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalCityName { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public string Duration { get; set; }
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

        [JsonProperty("departure_flight_date", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureDate { get; set; }
        [JsonProperty("arrival_flight_date", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalDate { get; set; }

        [JsonProperty("flight_infos", NullValueHandling = NullValueHandling.Ignore)]
        public FlightInfos FlightInfos { get; set; }
        
    }

    public class FlightInfos
    {
        [JsonProperty("flight_info", NullValueHandling = NullValueHandling.Ignore)]
        public List<FlightInfo> FlightInfo { get; set; }
    }

    public class FlightInfo
    {
        [JsonProperty("flight_number", NullValueHandling = NullValueHandling.Ignore)]
        public string Flight_Number { get; set; }

        [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }

        [JsonProperty("departure_city", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureCity { get; set; }
        [JsonProperty("departure_city_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureCityName { get; set; }
        [JsonProperty("arrival_city", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalCity { get; set; }

        [JsonProperty("arrival_city_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalCityName { get; set; }
        [JsonProperty("airlines_name", NullValueHandling = NullValueHandling.Ignore)]
        public string AirlinesName { get; set; }
        [JsonProperty("departure_date_time", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureDate { get; set; }
        [JsonProperty("arrival_date_time", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalDate { get; set; }

        [JsonProperty("simple_departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public string DepartureTime { get; set; }
        [JsonProperty("simple_arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public string ArrivalTime { get; set; }

        [JsonProperty("img_src", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }
        [JsonProperty("duration_time", NullValueHandling = NullValueHandling.Ignore)]
        public int DurationTime { get; set; }
        [JsonProperty("duration_hour", NullValueHandling = NullValueHandling.Ignore)]
        public string DurationHour { get; set; }
        [JsonProperty("duration_minute", NullValueHandling = NullValueHandling.Ignore)]
        public string DurationMinute { get; set; }
        [JsonProperty("check_in_baggaege", NullValueHandling = NullValueHandling.Ignore)]
        public int CheckInBaggage { get; set; }
        [JsonProperty("transit_duration_hour", NullValueHandling = NullValueHandling.Ignore)]
        public int TransitDurationHour { get; set; }
        [JsonProperty("transit_duration_minute", NullValueHandling = NullValueHandling.Ignore)]
        public int TransitDurationMinute { get; set; }
        [JsonProperty("transit_arrival_text_city", NullValueHandling = NullValueHandling.Ignore)]
        public string TransitCity { get; set; }
        [JsonProperty("transit_arrival_text_time", NullValueHandling = NullValueHandling.Ignore)]
        public string TransitTime { get; set; }

    }

}
