using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightItineraryForDisplay : FlightItineraryBase
    {
        public string SearchId { get; set; }
        public decimal TotalFare { get; set; }
        public string Currency { get; set; }
        public List<FlightTripForDisplay> Trips { get; set; }
        public string IssueProgress { get; set; }
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
    }
}
