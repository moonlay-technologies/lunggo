using System;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelReservationSearch
    {
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string PassengerName { get; set; }
        public string Airline { get; set; }
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public string RsvNo { get; set; }
        public DateSelectionType? RsvDateSelection { get; set; }
        public DateTime? RsvDateStart { get; set; }
        public DateTime? RsvDateEnd { get; set; }
        public DateTime? RsvDate { get; set; }
        public int? RsvDateMonth { get; set; }
        public int? RsvDateYear { get; set; }
        public DateSelectionType? DepartureDateSelection { get; set; }
        public DateTime? DepartureDateStart { get; set; }
        public DateTime? DepartureDateEnd { get; set; }
        public DateTime? DepartureDate { get; set; }
        public int? DepartureDateMonth { get; set; }
        public int? DepartureDateYear { get; set; }
        public DateSelectionType? ReturnDateSelection { get; set; }
        public DateTime? ReturnDateStart { get; set; }
        public DateTime? ReturnDateEnd { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int? ReturnDateMonth { get; set; }
        public int? ReturnDateYear { get; set; }

        public enum DateSelectionType
        {
            Span,
            Specific,
            MonthYear
        }
    }

}