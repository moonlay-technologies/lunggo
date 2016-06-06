﻿using System;
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
        [JsonProperty("adultCount")]
        public int AdultCount { get; set; }
        [JsonProperty("childCount")]
        public int ChildCount { get; set; }
        [JsonProperty("infantCount")]
        public int InfantCount { get; set; }
        [JsonProperty("adultFare")]
        public decimal AdultFare { get; set; }
        [JsonProperty("childFare")]
        public decimal ChildFare { get; set; }
        [JsonProperty("infantFare")]
        public decimal InfantFare { get; set; }
        [JsonProperty("type")]
        public TripType TripType { get; set; }
        [JsonProperty("reqCabin")]
        public CabinClass RequestedCabinClass { get; set; }
        [JsonProperty("reg")]
        public int RegisterNumber { get; set; }
        [JsonProperty("totalFare")]
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
        public TripType TripType { get; set; }
        public TripType RequestedTripType { get; set; }
        public CabinClass RequestedCabinClass { get; set; }
        public int RegisterNumber { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public List<FlightTrip> Trips { get; set; }
        public FareType FareType { get; set; }
        public Supplier Supplier { get; set; }        

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
