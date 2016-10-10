using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelRate
    {
        [JsonProperty("rateKey", NullValueHandling = NullValueHandling.Ignore)]
        public string RateKey { get; set; }
        [JsonProperty("regsId", NullValueHandling = NullValueHandling.Ignore)]
        public string RegsId { get; set; }
        [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("paymenType", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentType { get; set; }
        [JsonProperty("boards", NullValueHandling = NullValueHandling.Ignore)]
        public string Boards { get; set; }
        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public Cancellation Cancellation { get; set; }
        [JsonProperty("roomCount", NullValueHandling = NullValueHandling.Ignore)]
        public int RoomCount { get; set; }
        [JsonProperty("adultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int AdultCount { get; set; }
        [JsonProperty("childCount", NullValueHandling = NullValueHandling.Ignore)]
        public int ChildCount { get; set; }
        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Offer> Offers { get; set; }
        
    }

    public class Offer
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
    }

    public class Cancellation
    {
        [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Fee { get; set; }
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
    }
}
