using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

using System;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservationForDisplay
    {
        [JsonProperty("iss")]
        public bool IsIssued { get; set; }
        [JsonProperty("itin")]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("pax")]
        public List<FlightPassenger> Passengers { get; set; }
        [JsonProperty("typ")]
        public TripType OverallTripType { get; set; }
    }

    public class FlightReservation : ReservationBase<FlightReservation>
    {
        protected override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        public List<FlightItinerary> Itineraries { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public TripType OverallTripType { get; set; }

        
    }
}
