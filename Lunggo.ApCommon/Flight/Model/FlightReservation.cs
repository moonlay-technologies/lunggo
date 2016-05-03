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
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public RsvStatus RsvStatus { get; set; }
        public CancellationType CancellationType { get; set; }
        public DateTime? CancellationTime { get; set; }
        public PaymentDetails Payment { get; set; }
        public Contact Contact { get; set; }
        public User User { get; set; }
        [JsonProperty("itin")]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("pax")]
        public List<FlightPassenger> Passengers { get; set; }
        [JsonProperty("typ")]
        public TripType OverallTripType { get; set; }
    }

    public class FlightReservation : ReservationBase<FlightReservation, FlightItinerary, FlightRsvRule, FlightItineraryRule>
    {
        public override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        public List<FlightPassenger> Passengers { get; set; }
        public TripType OverallTripType { get; set; }

        
    }
}
