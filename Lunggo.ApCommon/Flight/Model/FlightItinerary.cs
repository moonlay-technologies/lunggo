using System;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightItineraryForDisplay
    {
        [JsonProperty("reqPassport")]
        public bool RequirePassport { get; set; }
        [JsonProperty("reqDob")]
        public bool RequireBirthDate { get; set; }
        [JsonProperty("reqSameCheckIn")]
        public bool RequireSameCheckIn { get; set; }
        [JsonProperty("reqNationality")]
        public bool RequireNationality { get; set; }
        [JsonProperty("holdable")]
        public bool CanHold { get; set; }
        [JsonProperty("adult")]
        public int AdultCount { get; set; }
        [JsonProperty("child")]
        public int ChildCount { get; set; }
        [JsonProperty("infant")]
        public int InfantCount { get; set; }
        [JsonProperty("type")]
        public TripType TripType { get; set; }
        [JsonProperty("reqCabin")]
        public CabinClass RequestedCabinClass { get; set; }
        [JsonProperty("reg")]
        public int RegisterNumber { get; set; }
        [JsonProperty("fare")]
        public decimal TotalFare { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("trips")]
        public List<FlightTripForDisplay> Trips { get; set; }
        [JsonProperty("originalFare")]
        public decimal OriginalFare { get; set; }
        [JsonProperty("comboFare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ComboFare { get; set; }
    }

    public class FlightItinerary : OrderBase<FlightItineraryRule>
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
        public TripType TripType { get; set; }
        public CabinClass RequestedCabinClass { get; set; }
        public int RegisterNumber { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public List<FlightTrip> Trips { get; set; }
        public FareType FareType { get; set; }
        public Supplier Supplier { get; set; }
        public TripType RequestedTripType { get; set; }

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
