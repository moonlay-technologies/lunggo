﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class BookingDetailForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
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
        [JsonProperty("paxCount", NullValueHandling = NullValueHandling.Ignore)]
        public string PaxCount { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
        
    }
    public class BookingDetail
    {
        public long ActivityId { get; set; }
        public string RsvNo  { get; set; }
        public string Name { get; set; }
        public string BookingStatus { get; set; }
        public DateTime? TimeLimit { get; set; }
        public DateTime Date { get; set; }
        public string SelectedSession { get; set; }
        public string PaxCount { get; set; }
        public decimal Price { get; set; }
        public string MediaSrc { get; set; }
        
    }
    
}
