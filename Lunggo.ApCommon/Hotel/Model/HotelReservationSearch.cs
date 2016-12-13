using System;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelReservationSearch
    {
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string PaxName { get; set; }
        public string RsvNo { get; set; }
        public DateSelectionType? RsvDateSelection { get; set; }
        public DateTime? RsvDateStart { get; set; }
        public DateTime? RsvDateEnd { get; set; }
        public DateTime? RsvDate { get; set; }
        public int? RsvDateMonth { get; set; }
        public int? RsvDateYear { get; set; }

        public enum DateSelectionType
        {
            Span,
            Specific,
            MonthYear
        }
    }

}