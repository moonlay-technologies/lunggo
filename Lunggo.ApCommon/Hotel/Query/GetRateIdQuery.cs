using System.Collections;
using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Hotel.Query
{
    internal class GetRateIdQuery : DbQueryBase<GetRateIdQuery, HotelRateTableRecord>
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
            clauseBuilder.Append("SELECT r.Id, r.RateKey, r.RoomCount, r.AdultCount, r.ChildCount, r.ChildrenAges ");
            clauseBuilder.Append("FROM HotelRate AS r ");
            clauseBuilder.Append("INNER JOIN HotelRoom AS d ON r.RoomId = d.Id ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE d.Id IN @Id");
            return clauseBuilder.ToString();
        }
    }
}
