using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Activity.Model
{

    public class AppointmentDetailForDisplay
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long? ActivityId { get; set; }
        [JsonProperty("appointmentId", NullValueHandling = NullValueHandling.Ignore)]
        public long? AppointmentId { get; set; }
        [JsonProperty("activityName", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("requestTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RequestTime { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime TimeLimit { get; set; }
        [JsonProperty("session", NullValueHandling = NullValueHandling.Ignore)]
        public string Session { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCount { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
        [JsonProperty("paxGroups", NullValueHandling = NullValueHandling.Ignore)]
        public PaxGroup PaxGroup { get; set; }
        [JsonProperty("appointmentReservations", NullValueHandling = NullValueHandling.Ignore)]
        public List<AppointmentReservation> AppointmentReservations { get; set; }
        [JsonProperty("contactName", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactName { get; set; }

    }

    public class AppointmentListForDisplay
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("session", NullValueHandling = NullValueHandling.Ignore)]
        public string Session { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
        [JsonProperty("reservations", NullValueHandling = NullValueHandling.Ignore)]
        public List<AppointmentReservation> AppointmentReservations { get; set; }
    }

    public class ReservationListForDisplay
    {
        [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
        public ReservationActivityDetail Activity { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("rsvTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RsvTime { get; set; }
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("paxes", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCounts { get; set; }
        [JsonProperty("paymentSteps", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentStep> PaymentSteps { get; set; }
    }

    public class AppointmentDetail
    {
        public long ActivityId { get; set; }
        public long AppointmentId { get; set; }
        public string Name { get; set; }
        public long PackageId { get; set; }
        public List<ActivityPricePackageReservation> PaxCount { get; set; }
        public string RsvNo { get; set; }
        public string PackageName { get; set; }
        public DateTime Date { get; set; }
        public DateTime RequestTime { get; set; }
        public string Session { get; set; }
        public string MediaSrc { get; set; }
        public PaxGroup PaxGroup { get; set; }
        public List<AppointmentReservation> AppointmentReservations { get; set; }
        public string ContactName { get; set; }
        public DateTime TimeLimit { get; set; }
        public List<PaymentStep> PaymentSteps { get; set; }
        public string RsvStatus { get; set; }
    }

    public class AppointmentList
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Session { get; set; }
        public string MediaSrc { get; set; }
        public List<AppointmentReservation> AppointmentReservations { get; set; }
    }

    public class Reservation
    {
        public ReservationActivityDetail Activity { get; set; }
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public Contact Contact { get; set; }
        public List<PaxForDisplay> Passengers { get; set; }
        public List<ActivityPricePackageReservation> PaxCounts { get; set; }
        public List<PaymentStep> PaymentSteps { get; set; }
    }
    public class PaxGroup
    {
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("paxes", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCounts { get; set; }
    }

    public class AppointmentReservation
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("rsvTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? RsvTime { get; set; }
        [JsonProperty("rsvStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvStatus { get; set; }
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("paxes", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCounts { get; set; }
        [JsonProperty("paymentSteps", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaymentStep> PaymentSteps { get; set; }

    }

    public class PaymentStep
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string StepDescription { get; set; }
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal StepAmount { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StepDate { get; set; }
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string StepStatus { get; set; }
    }
    
    public class ReservationActivityDetail
    {
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Session { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
    }
}
