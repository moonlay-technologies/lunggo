using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class DiscountRule
    {
        public long RuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<DateSpanRule> BookingDateSpans { get; set; }
        public List<DayOfWeek> BookingDays { get; set; }
        public List<DateTime> BookingDates { get; set; }
        public List<FareType> FareTypes { get; set; }
        public List<CabinClass> CabinClasses { get; set; }
        public List<TripType> TripTypes { get; set; }
        public List<DateSpanRule> DepartureDateSpans { get; set; }
        public List<DayOfWeek> DepartureDays { get; set; }
        public List<DateTime> DepartureDates { get; set; }
        public List<TimeSpanRule> DepartureTimeSpans { get; set; }
        public List<DateSpanRule> ReturnDateSpans { get; set; }
        public List<DayOfWeek> ReturnDays { get; set; }
        public List<DateTime> ReturnDates { get; set; }
        public List<TimeSpanRule> ReturnTimeSpans { get; set; }
        public int MaxPassengers { get; set; }
        public int MinPassengers { get; set; }
        public List<string> Airlines { get; set; }
        public bool AirlinesIsExclusion { get; set; }
        public List<AirportPairRule> AirportPairs { get; set; }
        public bool AirportPairsIsExclusion { get; set; }
        public List<AirportPairRule> CityPairs { get; set; }
        public bool CityPairsIsExclusion { get; set; }
        public List<AirportPairRule> CountryPairs { get; set; }
        public bool CountryPairsIsExclusion { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Constant { get; set; }
        public int ConstraintCount { get; set; }
        public int Priority { get; set; }

        public DiscountRule()
        {
            BookingDateSpans = new List<DateSpanRule>();
            BookingDays = new List<DayOfWeek>();
            BookingDates = new List<DateTime>();
            FareTypes = new List<FareType>();
            CabinClasses = new List<CabinClass>();
            TripTypes = new List<TripType>();
            DepartureDateSpans = new List<DateSpanRule>();
            DepartureDays = new List<DayOfWeek>();
            DepartureDates = new List<DateTime>();
            DepartureTimeSpans = new List<TimeSpanRule>();
            ReturnDateSpans = new List<DateSpanRule>();
            ReturnDays = new List<DayOfWeek>();
            ReturnDates = new List<DateTime>();
            ReturnTimeSpans = new List<TimeSpanRule>();
            Airlines = new List<string>();
            AirportPairs = new List<AirportPairRule>();
            CityPairs = new List<AirportPairRule>();
            CountryPairs = new List<AirportPairRule>();
            MinPassengers = int.MinValue;
            MaxPassengers = int.MaxValue;
            ConstraintCount = 0;
            Priority = int.MaxValue;
        }
    }
}
