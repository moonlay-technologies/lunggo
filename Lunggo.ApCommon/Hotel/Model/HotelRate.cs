using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Lunggo.ApCommon.Hotel.Constant;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelRateForDisplay
    {
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public Price Price { get; set; }

        [JsonProperty("timelimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TimeLimit { get; set; }

        [JsonProperty("rateKey", NullValueHandling = NullValueHandling.Ignore)]
        public string RateKey { get; set; }

        [JsonProperty("regsId", NullValueHandling = NullValueHandling.Ignore)]
        public string RegsId { get; set; }

        [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }

        [JsonProperty("classDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string ClassDescription { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("typeDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeDescription { get; set; }

        //[JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal Price { get; set; }
        [JsonProperty("paymentType", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentTypeEnum PaymentType { get; set; }

        [JsonProperty("boards", NullValueHandling = NullValueHandling.Ignore)]
        public string Boards { get; set; }

        [JsonProperty("boardDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string BoardDescription { get; set; }

        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public List<Cancellation> Cancellation { get; set; }

        [JsonProperty("roomCount", NullValueHandling = NullValueHandling.Ignore)]
        public int RoomCount { get; set; }

        [JsonProperty("adultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int AdultCount { get; set; }

        [JsonProperty("childCount", NullValueHandling = NullValueHandling.Ignore)]
        public int ChildCount { get; set; }

        [JsonProperty("allotment", NullValueHandling = NullValueHandling.Ignore)]
        public int Allotment { get; set; }

        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Offer> Offers { get; set; }

        [JsonProperty("originalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalPrice { get; set; }
        [JsonProperty("originalTotalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalTotalPrice { get; set; }

        [JsonProperty("netPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetPrice { get; set; }
        [JsonProperty("netTotalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetTotalPrice { get; set; }

        [JsonProperty("childrenAges", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ChildrenAges { get; set; }
        [JsonProperty("tnc", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> TermAndCondition { get; set; }
        [JsonProperty("isRefundable", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsRefundable { get; set; }
        [JsonProperty("isFreeCancel", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFreeCancel { get; set; }
        [JsonProperty("freeUntil", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FreeUntil { get; set; }

    }

    public class HotelRate : OrderBase
    {
        [JsonProperty("rateCount", NullValueHandling = NullValueHandling.Ignore)]
        public int RateCount { get; set; }
        [JsonProperty("rateKey", NullValueHandling = NullValueHandling.Ignore)]
        public string RateKey { get; set; }
        [JsonProperty("regsId", NullValueHandling = NullValueHandling.Ignore)]
        public string RegsId { get; set; }
        [JsonProperty("class", NullValueHandling = NullValueHandling.Ignore)]
        public string Class { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        //[JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal Price { get; set; }
        [JsonProperty("paymenType", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentTypeEnum PaymentType { get; set; }
        [JsonProperty("board", NullValueHandling = NullValueHandling.Ignore)]
        public string Boards { get; set; }
        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public List<Cancellation> Cancellation { get; set; }
        [JsonProperty("roomCount", NullValueHandling = NullValueHandling.Ignore)]
        public int RoomCount { get; set; }
        [JsonProperty("nightCount", NullValueHandling = NullValueHandling.Ignore)]
        public int NightCount { get; set; }
        [JsonProperty("adultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int AdultCount { get; set; }
        [JsonProperty("childCount", NullValueHandling = NullValueHandling.Ignore)]
        public int ChildCount { get; set; }
        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Offer> Offers { get; set; }
        [JsonProperty("childrenAges", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ChildrenAges { get; set; }
        [JsonProperty("hotelSellingRate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal HotelSellingRate{ get; set; }
        [JsonProperty("allotment", NullValueHandling = NullValueHandling.Ignore)]
        public int Allotment { get; set; }
        [JsonProperty("rateCommentsId", NullValueHandling = NullValueHandling.Ignore)]
        public string RateCommentsId { get; set; }
        public List<string> TermAndCondition { get; set; }
        
    }

    public class Offer
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public class Cancellation
    {
        [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Fee { get; set; }
        [JsonProperty("singleFee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal SingleFee { get; set; }
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
    }
}
