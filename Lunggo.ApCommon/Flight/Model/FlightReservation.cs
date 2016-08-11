using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using System;

using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservationForDisplay
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("rsvTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RsvTime { get; set; }
        [JsonProperty("rsvStatus", NullValueHandling = NullValueHandling.Ignore)]
        public RsvDisplayStatus RsvDisplayStatus { get; set; }
        [JsonProperty("cancelType", NullValueHandling = NullValueHandling.Ignore)]
        public CancellationType CancellationType { get; set; }
        [JsonProperty("cancelTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CancellationTime { get; set; }
        [JsonProperty("payment", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentDetailsForDisplay Payment { get; set; }
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("itin", NullValueHandling = NullValueHandling.Ignore)]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("pax", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public string DeviceId { get; set; }
    }

    public class FlightReservation : ReservationBase<FlightReservation>
    {
        public override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        public List<FlightItinerary> Itineraries { get; set; }
    }
}
