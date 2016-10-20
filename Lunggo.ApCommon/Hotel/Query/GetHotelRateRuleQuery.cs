using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Hotel.Query
{
    internal class GetHotelRateRuleQuery : DbQueryBase<GetHotelRateRuleQuery, HotelRateRuleTableRecord>
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
            clauseBuilder.Append("SELECT ConstraintCount, Priority, BookingDays, BookingDates, StayDates" +
                                 "StayDurations, MaxAdult, MinAdult, MinChild, MaxChild, " +
                                 "Boards, Countries, Destinations, RoomTypes, HotelStars ");
            clauseBuilder.Append("FROM HotelRateRule ");
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
