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
        [JsonProperty("isValid", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsValid { get; set; }
        [JsonProperty("isPriceChanged", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPriceChanged { get; set; }
        [JsonProperty("newPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NewPrice { get; set; }

    }
}