using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using System;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservationForDisplay
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
        [JsonProperty("rsvTime")]
        public DateTime RsvTime { get; set; }
        [JsonProperty("rsvStatus")]
        public RsvStatus RsvStatus { get; set; }
        [JsonProperty("cancelType")]
        public CancellationType CancellationType { get; set; }
        [JsonProperty("cancelTime")]
        public DateTime? CancellationTime { get; set; }
        [JsonProperty("payment")]
        public PaymentDetailsForDisplay Payment { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
        [JsonProperty("itin")]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("pax")]
        public List<Pax> Passengers { get; set; }
    }

    public class FlightReservation : ReservationBase<FlightReservation>
    {
        public override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        public List<FlightItinerary> Itineraries { get; set; }

        public FlightReservation()
        {
            State = new ReservationState();
        }
    }
}
