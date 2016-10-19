using System.Text;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Hotel.Query
{
    public class GetActiveHotelPriceMarginRuleQuery : DbQueryBase<GetActiveHotelPriceMarginRuleQuery, HotelMarginRule, MarginTableRecord, HotelRateRuleTableRecord>
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
                                 "r.ConstraintCount, r.Priority, r.BookingDays, r.BookingDates, " +
                                 "r.StayDates, r.StayDurations, r.MaxAdult, r.MaxChild, r.MinAdult, " +
                                 "r.MinChild, r.Boards, r.Countries, r.Destinations, r.RoomTypes, " +
                                 "r.HotelStars, r.HotelChains ");
            clauseBuilder.Append("FROM Margin AS m " +
                                 "INNER JOIN HotelRateRule AS r ON m.OrderRuleId = r.Id ");
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
