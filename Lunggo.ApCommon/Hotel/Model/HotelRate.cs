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
        [JsonProperty("timelimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TimeLimit { get; set; }
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
        [JsonProperty("breakdowns", NullValueHandling = NullValueHandling.Ignore)]
        public List<RateBreakdown> Breakdowns { get; set; }
        [JsonProperty("paymentType", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentTypeEnum PaymentType { get; set; }
        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public List<Cancellation> Cancellation { get; set; }
        [JsonProperty("allotment", NullValueHandling = NullValueHandling.Ignore)]
        public int Allotment { get; set; }
        [JsonProperty("offers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Offer> Offers { get; set; }
        [JsonProperty("tnc", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> TermAndCondition { get; set; }
        [JsonProperty("isRefundable", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsRefundable { get; set; }
        [JsonProperty("isFreeCancel", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFreeCancel { get; set; }
        [JsonProperty("freeUntil", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? FreeUntil { get; set; }

    }

    public class RateBreakdown
    {
        [JsonProperty("rateCount", NullValueHandling = NullValueHandling.Ignore)]
        public int RateCount { get; set; }
        [JsonProperty("adultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int AdultCount { get; set; }
        [JsonProperty("childCount", NullValueHandling = NullValueHandling.Ignore)]
        public int ChildCount { get; set; }
        [JsonProperty("childrenAges", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ChildrenAges { get; set; }
        [JsonProperty("board", NullValueHandling = NullValueHandling.Ignore)]
        public string Board { get; set; }
        [JsonProperty("boardDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string BoardDescription { get; set; }
        [JsonProperty("originalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalFare { get; set; }
        [JsonProperty("originalTotalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal OriginalTotalFare { get; set; }
        [JsonProperty("netFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetFare { get; set; }
        [JsonProperty("netTotalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal NetTotalFare { get; set; }
    }

    public class HotelRate : OrderBase
    {
        public int RateCount { get; set; }
        public string RateKey { get; set; }
        public string RegsId { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public string Board { get; set; }
        public List<Cancellation> Cancellation { get; set; }
        public int NightCount { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public List<Offer> Offers { get; set; }
        public List<int> ChildrenAges { get; set; }
        public decimal HotelSellingRate{ get; set; }
        public int Allotment { get; set; }
        public string RateCommentsId { get; set; }
        public List<string> TermAndCondition { get; set; }

        internal override decimal GetApparentOriginalPrice()
        {

            if (Price.OriginalIdr >= Price.FinalIdr)
            {
                return Price.OriginalIdr / Price.LocalCurrency.Rate;
            }
            else
            {
                var markups = new[] {1.25M, 1.275M, 1.30M, 1.325M, 1.35M, 1.375M, 1.40M};
                var markup = Price.OriginalIdr < 2000000
                    ? markups[(int) (Price.OriginalIdr/100%3)]
                    : markups[(int) (Price.OriginalIdr/100%7)];
                var original = Price.OriginalIdr * markup;
                var originalLocal = original / Price.LocalCurrency.Rate;
                var roundedOriginal = Math.Round(originalLocal / Price.LocalCurrency.RoundingOrder) * Price.LocalCurrency.RoundingOrder;
                var adjuster = Price.LocalCurrency.RoundingOrder*RateCount*NightCount;
                var adjustedOriginal = Math.Ceiling(roundedOriginal/adjuster)*adjuster;
                return adjustedOriginal;
            }
            
        }
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
        [JsonProperty("feePercentage", NullValueHandling = NullValueHandling.Ignore)]
        public decimal FeePercentage { get; set; }
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }
}
