using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.Framework.Error;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightItineraryApi : FlightItineraryBase
    {
        public int SequenceNo { get; set; }
        public List<FlightTripApi> FlightTrips { get; set; }
        public bool RequirePassport { get; set; }
        public bool RequireBirthDate { get; set; }
        public bool RequireSameCheckIn { get; set; }
        public bool CanHold { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public TripType TripType { get; set; }
        public decimal TotalFare { get; set; }
        public string Currency { get; set; }
    }

    public class FlightItineraryFare : FlightItineraryBase
    {
        public string FareId { get; set; }
        public List<FlightTripFare> FlightTrips { get; set; }
        public bool RequirePassport { get; set; }
        public bool RequireBirthDate { get; set; }
        public bool RequireSameCheckIn { get; set; }
        public bool CanHold { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public TripType TripType { get; set; }
        public FareType FareType { get; set; }
        public decimal SupplierPrice { get; set; }
        public string SupplierCurrency { get; set; }
        public decimal SupplierRate { get; set; }
        public decimal LocalPrice { get; set; }
        public string LocalCurrency { get; set; }
        public decimal LocalRate { get; set; }
        public decimal IdrPrice { get; set; }
        public FlightSupplier Supplier { get; set; }
    }

    public class FlightItineraryDetails : FlightItineraryBase
    {
        public List<FlightTripDetails> FlightTrips { get; set; }
        public List<PassengerInfoDetails> PassengerInfo { get; set; }
        public TripType TripType { get; set; }
    }



    public class FlightItineraryBase
    {
    }
}
