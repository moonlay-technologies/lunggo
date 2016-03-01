using System;
using System.Collections.Generic;
using System.Web.Helpers;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightItineraryForDisplay : FlightItineraryBase
    {
        [JsonProperty("search_id")]
        public string SearchId { get; set; }
        [JsonProperty("total_fare")]
        public decimal TotalFare { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("trips")]
        public List<FlightTripForDisplay> Trips { get; set; }
        [JsonProperty("original_fare")]
        public decimal OriginalFare { get; set; }
    }

    public class FlightItinerary : FlightItineraryBase
    {
        public string FareId { get; set; }
        public string BookingId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public DateTime? TicketTimeLimit { get; set; }
        public List<FlightTrip> Trips { get; set; }
        public decimal SupplierPrice { get; set; }
        public string SupplierCurrency { get; set; }
        public decimal SupplierRate { get; set; }
        public decimal OriginalIdrPrice { get; set; }
        public long MarginId { get; set; }
        public decimal MarginCoefficient { get; set; }
        public decimal MarginConstant { get; set; }
        public decimal MarginNominal { get; set; }
        public bool MarginIsFlat { get; set; }
        public decimal FinalIdrPrice { get; set; }
        public decimal LocalPrice { get; set; }
        public string LocalCurrency { get; set; }
        public decimal LocalRate { get; set; }
        public FareType FareType { get; set; }
        public Supplier Supplier { get; set; }
        public bool AsReturn { get; set; }
        public TripType RequestedTripType { get; set; }
    }

    public class FlightItineraryBase
    {
        [JsonProperty("req_passport")]
        public bool RequirePassport { get; set; }
        [JsonProperty("req_bd")]
        public bool RequireBirthDate { get; set; }
        [JsonProperty("req_same_check_in")]
        public bool RequireSameCheckIn { get; set; }
        [JsonProperty("req_nat")]
        public bool RequireNationality { get; set; }
        [JsonProperty("hold")]
        public bool CanHold { get; set; }
        [JsonProperty("adt")]
        public int AdultCount { get; set; }
        [JsonProperty("chd")]
        public int ChildCount { get; set; }
        [JsonProperty("inf")]
        public int InfantCount { get; set; }
        [JsonProperty("trip")]
        public TripType TripType { get; set; }
        [JsonProperty("req_cabin")]
        public CabinClass RequestedCabinClass { get; set; }
        [JsonProperty("reg")]
        public int RegisterNumber { get; set; }
        [JsonProperty("combo", NullValueHandling = NullValueHandling.Ignore)]
        public ComboSet ComboSet { get; set; }
    }

    public class ComboSet
    {
        [JsonProperty("fare")]
        public decimal ComboFare { get; set; }
        [JsonProperty("regs")]
        public List<int> PairRegisterNumber { get; set; }
        [JsonProperty("fares")]
        public List<decimal> TotalComboFare { get; set; }
        [JsonProperty("bun_regs", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> BundledRegisterNumber { get; set; }
    }
}
