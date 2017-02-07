using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightItineraryForDisplay
    {
        [JsonProperty("reqPassport", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequirePassport { get; set; }
        [JsonProperty("reqDob", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireBirthDate { get; set; }
        [JsonProperty("reqSameCheckIn", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireSameCheckIn { get; set; }
        [JsonProperty("reqNationality", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireNationality { get; set; }
        [JsonProperty("canBeHeld", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanHold { get; set; }
        
        
        [JsonProperty("adultCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? AdultCount { get; set; }
        [JsonProperty("childCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? ChildCount { get; set; }
        [JsonProperty("infantCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? InfantCount { get; set; }
        
        [JsonProperty("originalAdultFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalAdultFare { get; set; }
        [JsonProperty("originalChildFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalChildFare { get; set; }
        [JsonProperty("originalInfantFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalInfantFare { get; set; }

        [JsonProperty("netAdultFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetAdultFare { get; set; }
        [JsonProperty("netChildFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetChildFare { get; set; }
        [JsonProperty("netInfantFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetInfantFare { get; set; }

        [JsonProperty("originalTotalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? OriginalTotalFare { get; set; }
        [JsonProperty("netTotalFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NetTotalFare { get; set; }
                
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TripType? TripType { get; set; }
        [JsonProperty("reqCabin", NullValueHandling = NullValueHandling.Ignore)]
        public CabinClass? RequestedCabinClass { get; set; }
        [JsonProperty("reg", NullValueHandling = NullValueHandling.Ignore)]
        public int? RegisterNumber { get; set; }
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
        [JsonProperty("trips", NullValueHandling = NullValueHandling.Ignore)]
        public List<FlightTripForDisplay> Trips { get; set; }
        
        [JsonProperty("comboFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ComboFare { get; set; }
        [JsonProperty("tripBreakdown", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsTripFareBrokendown { get; set; }
    }

    public class FlightItinerary : OrderBase
    {
        public string SearchId { get; set; }
        public string FareId { get; set; }
        public string BookingId { get; set; }
        public bool RequirePassport { get; set; }
        public bool RequireBirthDate { get; set; }
        public bool RequireSameCheckIn { get; set; }
        public bool RequireNationality { get; set; }
        public bool CanHold { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public decimal AdultPricePortion { get; set; }
        public decimal ChildPricePortion { get; set; }
        public decimal InfantPricePortion { get; set; }
        public decimal NetAdultPricePortion { get; set; }
        public decimal NetChildPricePortion { get; set; }
        public decimal NetInfantPricePortion { get; set; }
        public TripType TripType { get; set; }
        public TripType RequestedTripType { get; set; }
        public CabinClass RequestedCabinClass { get; set; }
        public int RegisterNumber { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public List<FlightTrip> Trips { get; set; }
        public FareType FareType { get; set; }
        public Supplier Supplier { get; set; }

        internal override decimal GetApparentOriginalPrice()
        {
            if (Price == null)
                throw new Exception("Price not set");

            if (Price.OriginalIdr >= Price.FinalIdr)
            {
                return Price.OriginalIdr/Price.LocalCurrency.Rate;
            }
            else
            {
                var originalPrice = Price.OriginalIdr;
                var adultCount = AdultCount;
                var childCount = ChildCount;
                var infantCount = InfantCount;
                var roundingOrder = Price.LocalCurrency.RoundingOrder;

                var adultAdjustment = adultCount != 0
                    ? (originalPrice * AdultPricePortion / adultCount) % roundingOrder * adultCount
                    : 0M;
                var childAdjustment = childCount != 0
                    ? roundingOrder - (originalPrice * ChildPricePortion / childCount) % roundingOrder * childCount
                    : 0M;
                var infantAdjustment = infantCount != 0
                    ? roundingOrder - (originalPrice * InfantPricePortion / infantCount) % roundingOrder * infantCount
                    : 0M;
                var adjustment = -adultAdjustment + childAdjustment + infantAdjustment;

                return originalPrice + adjustment;
            }
        }

        public bool Identical(FlightItinerary otheritin)
        {
            return
                RequirePassport == otheritin.RequirePassport &&
                RequireBirthDate == otheritin.RequireBirthDate &&
                RequireSameCheckIn == otheritin.RequireSameCheckIn &&
                RequireNationality == otheritin.RequireNationality &&
                CanHold == otheritin.CanHold &&
                AdultCount == otheritin.AdultCount &&
                ChildCount == otheritin.ChildCount &&
                InfantCount == otheritin.InfantCount &&
                TripType == otheritin.TripType &&
                Trips.Count == otheritin.Trips.Count &&
                Trips.Zip(otheritin.Trips, (trip, otherTrip) => trip.Identical(otherTrip)).All(x => x);
        }
    }
}
