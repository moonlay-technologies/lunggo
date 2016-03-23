using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

using System;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservationForDisplay
    {
        [JsonProperty("rsvno")]
        public string RsvNo { get; set; }
        [JsonProperty("rsvtm")]
        public DateTime RsvTime { get; set; }
        [JsonProperty("iss")]
        public bool IsIssued { get; set; }
        [JsonProperty("itin")]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("pax")]
        public List<FlightPassenger> Passengers { get; set; }
        [JsonProperty("pay")]
        public PaymentData Payment { get; set; }
        [JsonProperty("con")]
        public Contact Contact { get; set; }
        [JsonProperty("inv", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceNo { get; set; }
        [JsonProperty("disc")]
        public decimal Discount { get; set; }
        [JsonProperty("discnm")]
        public string DiscountName { get; set; }
        [JsonProperty("cd")]
        public string VoucherCode { get; set; }
        [JsonProperty("typ")]
        public TripType TripType { get; set; }
    }

    public class FlightReservation
    {
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public List<FlightItinerary> Itineraries { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public Payment.Model.PaymentData Payment { get; set; }
        public Contact Contact { get; set; }
        public string InvoiceNo { get; set; }
        public TripType TripType { get; set; }
        public Discount Discount { get; set; }
    }
}
