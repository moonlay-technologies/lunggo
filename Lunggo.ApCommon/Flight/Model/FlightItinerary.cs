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
    public class FlightFareItinerary : FlightItineraryBase
    {
        public string FareId { get; set; }
        public List<FlightFareTrip> FlightTrips { get; set; }
        public int TotalTransit { get; set; }
        public List<Transit> Transits { get; set; }
        public List<Airline> Airlines { get; set; }
        public bool RequirePassport { get; set; }
        public bool RequireBirthDate { get; set; }
        public bool RequireSameCheckIn { get; set; }
        public bool CanHold { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public TripType TripType { get; set; }
        public decimal TotalFare { get; set; }
        public decimal AdultTotalFare { get; set; }
        public decimal ChildTotalFare { get; set; }
        public decimal InfantTotalFare { get; set; }
        public decimal PscFare { get; set; }
        public List<FlightRules> Rules { get; set; }
    }

    public class FlightItineraryDetails : FlightItineraryBase
    {
        public List<FlightTripDetails> FlightTrips { get; set; }
        public List<PassengerInfoDetails> PassengerInfo { get; set; }
    }

    public class FlightItineraryBase
    {
        public FlightSupplier Supplier { get; set; }
    }

    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Transit
    {
        public bool IsStop { get; set; }
        public string Location { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }
    }
}
