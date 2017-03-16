using System.Collections.Generic;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelBookApiRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
        [JsonProperty("pax")]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("lang")]
        public string LanguageCode { get; set; }
        [JsonProperty("specialRequest")]
        public string SpecialRequest { get; set; }
        [JsonProperty("bookerMessageTitle")]
        public string BookerMessageTitle { get; set; }
        [JsonProperty("bookerMessageDescription")]
        public string BookerMessageDescription { get; set; }
    }
}