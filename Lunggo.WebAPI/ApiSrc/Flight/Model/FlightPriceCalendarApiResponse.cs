using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightPriceCalendarApiResponse : ApiResponseBase
    {
        
        [JsonProperty("datesAndPrices")]
        public Dictionary<string, decimal> Prices { get; set; }
            
        [JsonProperty("cheapestPrice")]
        public decimal CheapestPrice { get; set; }

        [JsonProperty("cheapestDate")]
        public string CheapestDate { get; set; }
    }
}