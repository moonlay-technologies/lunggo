using System.Collections;
using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Hotel.Query
{
    internal class GetRoomIdQuery : DbQueryBase<GetRoomIdQuery, long>
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
            clauseBuilder.Append("SELECT r.Id ");
            clauseBuilder.Append("FROM HotelRoom AS r ");
            clauseBuilder.Append("INNER JOIN HotelReservationDetails AS d ON r.DetailsId = d.Id ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE d.RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
