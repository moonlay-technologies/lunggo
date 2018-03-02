using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Activity.Model
{

    public class BookingDetailForDisplay
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("bookingStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingStatus { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("selectedSession", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedSession { get; set; }
        [JsonProperty("packageId", NullValueHandling = NullValueHandling.Ignore)]
        public long PackageId { get; set; }
        [JsonProperty("packageName", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCount { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
        [JsonProperty("paxsDetail", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }
    }
    public class BookingDetail
    {
        [JsonProperty("ticketNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string TicketNumber { get; set; }
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo  { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("bookingStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingStatus { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("selectedSession", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedSession { get; set; }
        [JsonProperty("packageId", NullValueHandling = NullValueHandling.Ignore)]
        public long PackageId { get; set; }
        [JsonProperty("packageName", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCount { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
        [JsonProperty("passengers", NullValueHandling = NullValueHandling.Ignore)]
        public List<Pax> Passengers { get; set; }
        [JsonProperty("area", NullValueHandling = NullValueHandling.Ignore)]
        public string Area { get; set; }
        [JsonProperty("zone", NullValueHandling = NullValueHandling.Ignore)]
        public string Zone { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Latitude { get; set; }
        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public double Longitude { get; set; }
        [JsonProperty("operatorName", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorName { get; set; }
        [JsonProperty("operatorEmail", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorEmail { get; set; }
        [JsonProperty("operatorPhone", NullValueHandling = NullValueHandling.Ignore)]
        public string OperatorPhone { get; set; }
        [JsonProperty("hasPdfVoucher", NullValueHandling = NullValueHandling.Ignore)]
        public bool HasPdfVoucher { get; set; }
        [JsonProperty("requestReview", NullValueHandling = NullValueHandling.Ignore)]
        public bool RequestReview { get; set; }
        [JsonProperty("requestRating", NullValueHandling = NullValueHandling.Ignore)]
        public bool RequestRating { get; set; }
        [JsonProperty("isPdfUploaded", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPdfUploaded { get; set; }
        [JsonProperty("pdfUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string PdfUrl { get; set; }        
    }

    public class ActivityPackageReservation
    {
        [JsonProperty("packageName", NullValueHandling = NullValueHandling.Ignore)]
        public string PackageName { get; set; }
        [JsonProperty("packageId", NullValueHandling = NullValueHandling.Ignore)]
        public long PackageId { get; set; }
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int Count { get; set; }
    }

    public class CartList
    {
        [JsonProperty("cartId", NullValueHandling = NullValueHandling.Ignore)]
        public string CartId { get; set; }
        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public List<BookingDetail> Activities { get; set; }
        [JsonProperty("totalOriginalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalOriginalPrice { get; set; }
        [JsonProperty("totalDiscount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalDiscount { get; set; }
        [JsonProperty("totalUniqueCode", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalUniqueCode { get; set; }
        [JsonProperty("totalFinalPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TotalFinalPrice { get; set; }
        [JsonProperty("paymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentStatus { get; set; }
    }
}
