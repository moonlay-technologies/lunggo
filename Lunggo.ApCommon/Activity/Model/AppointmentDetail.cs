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
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("requestTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RequestTime { get; set; }
        [JsonProperty("session", NullValueHandling = NullValueHandling.Ignore)]
        public string Session { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public string PaxCount { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
        [JsonProperty("paxGroups", NullValueHandling = NullValueHandling.Ignore)]
        public PaxGroup PaxGroup { get; set; }
        [JsonProperty("appointmentReservations", NullValueHandling = NullValueHandling.Ignore)]
        public List<AppointmentReservation> AppointmentReservations { get; set; }

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
    public class AppointmentDetail
    {
        public long ActivityId { get; set; }
        public long AppointmentId { get; set; }
        public string Name { get; set; }
        public long PackageId { get; set; }
        public string PaxCount { get; set; }
        public string RsvNo { get; set; }
        public string PackageName { get; set; }
        public DateTime Date { get; set; }
        public string RequestTime { get; set; }
        public string Session { get; set; }
        public string MediaSrc { get; set; }
        public PaxGroup PaxGroup { get; set; }
        public List<AppointmentReservation> AppointmentReservations { get; set; }
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
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("paxes", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPricePackageReservation> PaxCounts { get; set; }        
    }
    
}
