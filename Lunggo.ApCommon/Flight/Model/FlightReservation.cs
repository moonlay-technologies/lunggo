using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

using System;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservationForDisplay
    {
        [JsonProperty("rsv_no")]
        public string RsvNo { get; set; }
        [JsonProperty("rsv_time")]
        public DateTime RsvTime { get; set; }
        [JsonProperty("issued")]
        public bool IsIssued { get; set; }
        [JsonProperty("itin")]
        public FlightItineraryForDisplay Itinerary { get; set; }
        [JsonProperty("pax")]
        public List<FlightPassenger> Passengers { get; set; }
        [JsonProperty("payment")]
        public PaymentData Payment { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
        [JsonProperty("invoice", NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceNo { get; set; }
        [JsonProperty("disc")]
        public decimal Discount { get; set; }
        [JsonProperty("disc_name")]
        public string DiscountName { get; set; }
        [JsonProperty("code")]
        public string VoucherCode { get; set; }
        [JsonProperty("type")]
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
