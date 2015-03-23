using System;
using System.ComponentModel.DataAnnotations;

namespace Lunggo.BackendWeb.Model
{
    public class FlightReservationSearch
    {
        public string RsvNo { get; set; }
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateSelection? RsvDateSelection { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RsvDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RsvDateStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RsvDateEnd { get; set; }
        public int? RsvMonth { get; set; }
        public int? RsvYear { get; set; }
        public string DepartureAirline { get; set; }
        public string ReturnAirline { get; set; }
        public DateSelection? DepartureDateSelection { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DepartureDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DepartureDateStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DepartureDateEnd { get; set; }
        public int? DepartureMonth { get; set; }
        public int? DepartureYear { get; set; }
        public DateSelection? ReturnDateSelection { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReturnDateStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReturnDateEnd { get; set; }
        public int? ReturnMonth { get; set; }
        public int? ReturnYear { get; set; }
        public string PassengerName { get; set; }
        public string ContactName { get; set; }

        public enum DateSelection
        {
            Specific = 0,
            Range,
            MonthYear
        }

        public FlightReservationSearch()
        {
            
        }

        public FlightReservationSearch(string rsvNo)
        {
            RsvNo = rsvNo;
        }
    }
    
}
