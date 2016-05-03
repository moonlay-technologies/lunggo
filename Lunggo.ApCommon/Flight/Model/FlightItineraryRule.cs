using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightItineraryRule : OrderRuleBase
    {
        protected override ProductType Type
        {
            get { return ProductType.Flight; }
        }

        public List<DateSpanRule> BookingDateSpans { get; set; }
        public List<DayOfWeek> BookingDays { get; set; }
        public List<DateTime> BookingDates { get; set; }
        public List<FareType> FareTypes { get; set; }
        public List<AirlineType> AirlineTypes { get; set; }
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

        public FlightItineraryRule()
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
            MinPassengers = 0;
            MaxPassengers = 10;
        }

        internal static FlightItineraryRule GetFromDb(long ruleId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetItinRuleQuery.GetInstance().Execute(conn, new { RuleId = ruleId }).Single();
                return new FlightItineraryRule
                {
                    RuleId = record.Id.GetValueOrDefault(),
                    BookingDateSpans = record.BookingDateSpans.Deserialize<List<DateSpanRule>>(),
                    BookingDays = record.BookingDays.Deserialize<List<DayOfWeek>>(),
                    BookingDates = record.BookingDates.Deserialize<List<DateTime>>(),
                    FareTypes = record.FareTypes.Deserialize<List<FareType>>(),
                    CabinClasses = record.CabinClasses.Deserialize<List<CabinClass>>(),
                    TripTypes = record.RequestedTripTypes.Deserialize<List<TripType>>(),
                    DepartureDateSpans = record.DepartureDateSpans.Deserialize<List<DateSpanRule>>(),
                    DepartureDays = record.DepartureDays.Deserialize<List<DayOfWeek>>(),
                    DepartureDates = record.DepartureDates.Deserialize<List<DateTime>>(),
                    DepartureTimeSpans = record.DepartureTimeSpans.Deserialize<List<TimeSpanRule>>(),
                    ReturnDateSpans = record.ReturnDateSpans.Deserialize<List<DateSpanRule>>(),
                    ReturnDays = record.ReturnDays.Deserialize<List<DayOfWeek>>(),
                    ReturnDates = record.ReturnDates.Deserialize<List<DateTime>>(),
                    ReturnTimeSpans = record.ReturnTimeSpans.Deserialize<List<TimeSpanRule>>(),
                    MaxPassengers = record.MaxPassengers.GetValueOrDefault(),
                    MinPassengers = record.MinPassengers.GetValueOrDefault(),
                    Airlines = record.Airlines.Deserialize<List<string>>(),
                    AirlinesIsExclusion = record.AirlinesIsExclusion.GetValueOrDefault(),
                    AirportPairs = record.AirportPairs.Deserialize<List<AirportPairRule>>(),
                    AirportPairsIsExclusion = record.AirportPairsIsExclusion.GetValueOrDefault(),
                    CityPairs = record.CityPairs.Deserialize<List<AirportPairRule>>(),
                    CityPairsIsExclusion = record.CityPairsIsExclusion.GetValueOrDefault(),
                    CountryPairs = record.CountryPairs.Deserialize<List<AirportPairRule>>(),
                    CountryPairsIsExclusion = record.CountryPairsIsExclusion.GetValueOrDefault(),
                    ConstraintCount = record.ConstraintCount.GetValueOrDefault(),
                    Priority = record.Priority.GetValueOrDefault(),
                };
            }
        }
    }

    public class TimeSpanRule
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public bool Contains(TimeSpan time)
        {
            return time >= Start && time <= End;
        }
    }

    public class DateSpanRule
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public bool Contains(DateTime date)
        {
            return date >= Start && date <= End;
        }
    }

    public class AirportPairRule
    {
        public string Origin { get; set; }
        public string Destination { get; set; }

        public static bool operator ==(AirportPairRule rule1, AirportPairRule rule2)
        {
            if (ReferenceEquals(rule1, rule2))
            {
                return true;
            }

            if (((object)rule1 == null) || ((object)rule2 == null))
            {
                return false;
            }

            return rule1.Origin == rule2.Origin && rule1.Destination == rule2.Destination;
        }

        public static bool operator !=(AirportPairRule rule1, AirportPairRule rule2)
        {
            return !(rule1 == rule2);
        }

        public override bool Equals(object obj)
        {
            return this == (AirportPairRule)obj;
        }
    }
}
