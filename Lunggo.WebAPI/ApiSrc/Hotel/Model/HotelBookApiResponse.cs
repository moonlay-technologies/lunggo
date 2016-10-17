using System;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelBookApiResponse : ApiResponseBase
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("isValid")]
        public bool IsValid { get; set; }
        [JsonProperty("isPriceChanged")]
        public bool IsPriceChanged { get; set; }
        [JsonProperty("newPrice")]
        public decimal? NewPrice { get; set; }

    }
}