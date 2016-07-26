using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightPriceCalendarApiResponse : ApiResponseBase
    {
        
        [JsonProperty("datesAndPrices")]
        public DataByYear Prices { get; set; }
            
        [JsonProperty("cheapestPrice")]
        public decimal CheapestPrice { get; set; }

        [JsonProperty("cheapestDate")]
        public string CheapestDate { get; set; }
    }

    public class DataByYear
    {
        [JsonProperty("years")]
        public List<Year> Years { get; set; }

    }

    public class Year
    {
        [JsonProperty("months")]
        public List<Month> Months { get; set; }
        [JsonProperty("yearName")]
        public int Name { get; set; }
    }

    public class Month
    {
        [JsonProperty("prices")]
        public Dictionary<string, decimal> DailyPrice { get; set; }
        [JsonProperty("monthName")]
        public string Name { get; set; }
    }
}