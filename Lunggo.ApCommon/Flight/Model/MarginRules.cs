using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class MarginRules
    {
        internal int RuleId { get; set; }
        internal string RuleName { get; set; }
        internal List<string> Airlines { get; set; }
        internal List<AirportPairRule> AirportPairs { get; set; }
        internal List<CabinClass> CabinClasses { get; set; }
        internal List<DateSpanRule> BookingDateSpans { get; set; }
        internal List<DayOfWeek> BookingDays { get; set; }
        internal List<DateTime> BookingDates { get; set; }
        internal List<DateSpanRule> DepartureDateSpans { get; set; }
        internal List<DayOfWeek> DepartureDays { get; set; }
        internal List<DateTime> DepartureDates { get; set; }
        internal List<DateSpanRule> ReturnDateSpans { get; set; }
        internal List<DayOfWeek> ReturnDays { get; set; }
        internal List<DateTime> ReturnDates { get; set; }
        internal List<TripType> TripTypes { get; set; }
        internal List<TimeSpanRule> DepartureTimeSpans { get; set; }
        internal List<TimeSpanRule> ArrivalTimeSpans { get; set; }
        internal List<FareType> FareTypes { get; set; }
        internal int MaxPassengers { get; set; }
        internal int MinPassengers { get; set; }
        internal int ConstraintCount { get; set; }
        internal int Priority { get; set; }
    }

    internal class TimeSpanRule
    {
        internal TimeSpan Start { get; set; }
        internal TimeSpan End { get; set; }
    }

    internal class DateSpanRule
    {
        internal DateTime Start { get; set; }
        internal DateTime End { get; set; }
    }

    internal class AirportPairRule
    {
        internal string Departure { get; set; }
        internal string Arrival { get; set; }
    }
}
