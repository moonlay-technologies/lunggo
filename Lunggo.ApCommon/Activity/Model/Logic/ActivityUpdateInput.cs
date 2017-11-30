using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class ActivityUpdateInput
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortDesc { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Price { get; set; }
        public string PriceDetail { get; set; }
        public DurationActivity Duration { get; set; }
        public string OperationTime { get; set; }
        public List<string> MediaSrc { get; set; }
        public Content Contents { get; set; }
        public List<AdditionalContent> AdditionalContent { get; set; }
        public string Cancellation { get; set; }
        public DateTime Date { get; set; }
        public bool IsPassportNumberNeeded { get; set; }
        public bool IsPassportIssuedDateNeeded { get; set; }
        public bool IsPaxDoBNeeded { get; set; }
    }
}
