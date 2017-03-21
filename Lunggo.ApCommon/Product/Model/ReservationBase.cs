using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Product.Model
{
    public abstract class ReservationForDisplayBase
    {
        [JsonIgnore]
        public abstract ProductType Type { get; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("rsvTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RsvTime { get; set; }
        [JsonProperty("rsvType", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvType { get; set; }
        [JsonProperty("bookerMessageTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string BookerMessageTitle { get; set; }
        [JsonProperty("bookerMessageDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string BookerMessageDescription { get; set; }
        [JsonProperty("rejectionDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string RejectionDescription { get; set; }

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
        [JsonProperty("pax", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Pax { get; set; }
        [JsonProperty("booker", NullValueHandling = NullValueHandling.Ignore)]
        public UserForDisplay Booker { get; set; }
        [JsonProperty("approver", NullValueHandling = NullValueHandling.Ignore)]
        public UserForDisplay Approver { get; set; }
        [JsonIgnore]
        public string DeviceId { get; set; }
    }

    public abstract class ReservationBase
    {
        public abstract ProductType Type { get; }
        public string RsvNo { get; set; }
        public string RsvType { get; set; }
        public DateTime RsvTime { get; set; }
        public RsvStatus RsvStatus { get; set; }
        public CancellationType CancellationType { get; set; }
        public DateTime? CancellationTime { get; set; }
        public PaymentDetails Payment { get; set; }
        public Contact Contact { get; set; }
        public User User { get; set; }
        public ReservationState State { get; set; }
        public List<Pax> Pax { get; set; }
        public string BookerMessageTitle { get; set; }
        public string BookerMessageDescription { get; set; }
        public string RejectionDescription { get; set; }

        public abstract decimal GetTotalSupplierPrice();
    }
}
