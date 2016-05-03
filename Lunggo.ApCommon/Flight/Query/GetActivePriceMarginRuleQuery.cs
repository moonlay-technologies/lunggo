using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    public class GetActivePriceMarginRuleQuery : QueryBase<GetActivePriceMarginRuleQuery, FlightMarginRule, MarginTableRecord, FlightItineraryRuleTableRecord>
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
            clauseBuilder.Append("SELECT m.OrderRuleId, m.Name, m.Description, m.Percentage, m.Constant, m.CurrencyCd, " +
                                 "m.IsFlat, m.IsActive," +
                                 "r.ConstraintCount, r.Priority, r.BookingDateSpans, r.BookingDays, r.BookingDates, " +
                                 "r.FareTypes, r.AirlineTypes, r.CabinClasses, r.RequestedTripTypes, r.DepartureDateSpans, " +
                                 "r.DepartureDays, r.DepartureDates, r.DepartureTimeSpans, r.ReturnDateSpans, r.ReturnDays, " +
                                 "r.ReturnDates, r.ReturnTimeSpans, r.MaxPassengers, r.MinPassengers, r.Airlines, " +
                                 "r.AirlinesIsExclusion, r.AirportPairs, r.AirportPairsIsExclusion, r.CityPairs, " +
                                 "r.CityPairsIsExclusion, r.CountryPairs, r.CountryPairsIsExclusion ");
            clauseBuilder.Append("FROM Margin AS m " +
                                 "INNER JOIN FlightItineraryRule AS r ON m.OrderRuleId = r.Id ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE m.IsActive = 1 AND CAST(m.OrderRuleId AS NVARCHAR) LIKE '1%'");
            clauseBuilder.Append("ORDER BY r.ConstraintCount DESC, r.Priority ASC");
            return clauseBuilder.ToString();
        }
    }
}
