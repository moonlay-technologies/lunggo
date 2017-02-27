using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelPriceCalendarApiResponse : ApiResponseBase
    {
        
        [JsonProperty("listDatesAndPrices")]
        public List<DailyPrice> Prices { get; set; }
            
        [JsonProperty("cheapestPrice")]
        public decimal CheapestPrice { get; set; }

        [JsonProperty("cheapestDate")]
        public string CheapestDate { get; set; }
    }

}