using System;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelPriceCalendarApiRequest
    {
        [JsonProperty("locationCd")]
        public string LocationCode { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
    }
}