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
        [JsonProperty("fare")]
        public decimal TotalFare { get; set; }
        [JsonProperty("curr")]
        public string Currency { get; set; }
        [JsonProperty("trips")]
        public List<FlightTripForDisplay> Trips { get; set; }
        [JsonProperty("ofare")]
        public decimal OriginalFare { get; set; }
        [JsonProperty("cfare", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ComboFare { get; set; }
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
        public TripType RequestedTripType { get; set; }
    }

    public class FlightItineraryBase
    {
        [JsonProperty("sid")]
        public string SearchId { get; set; }
        [JsonProperty("rqpass")]
        public bool RequirePassport { get; set; }
        [JsonProperty("rqdob")]
        public bool RequireBirthDate { get; set; }
        [JsonProperty("rqsci")]
        public bool RequireSameCheckIn { get; set; }
        [JsonProperty("rqnat")]
        public bool RequireNationality { get; set; }
        [JsonProperty("hld")]
        public bool CanHold { get; set; }
        [JsonProperty("adt")]
        public int AdultCount { get; set; }
        [JsonProperty("chd")]
        public int ChildCount { get; set; }
        [JsonProperty("inf")]
        public int InfantCount { get; set; }
        [JsonProperty("type")]
        public TripType TripType { get; set; }
        [JsonProperty("rqcbn")]
        public CabinClass RequestedCabinClass { get; set; }
        [JsonProperty("reg")]
        public int RegisterNumber { get; set; }
    }
}
