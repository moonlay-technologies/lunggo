using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetItinRuleQuery : QueryBase<GetItinRuleQuery, FlightItineraryRuleTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT ConstraintCount, Priority, BookingDateSpans, BookingDays, BookingDates, " +
                                 "FareTypes, AirlineTypes, CabinClasses, RequestedTripTypes, DepartureDateSpanse, " +
                                 "DepartureDays, DepartureDates, DepartureTimeSpans, ReturnDateSpans, ReturnDays, " +
                                 "ReturnDates, ReturnTimeSpans, MaxPassengers, MinPassengers, Airlines, AirlinesIsExclusion, " +
                                 "AirportPairs, AirportPairsIsExclusion, CityPairs, CityPairsIsExclusion, CountryPairs, " +
                                 "CountryPairsIsExclusion ");
            clauseBuilder.Append("FROM FlightItineraryRule ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Id = @RuleId");
            return clauseBuilder.ToString();
        }
    }
}
