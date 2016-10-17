using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Documents;

namespace Lunggo.ApCommon.Hotel.Query
{
    internal class GetRoomDetailFromSearchResult : DocQueryBase
    {
        public override string GetQueryString(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT r");
            clauseBuilder.Append(@"FROM Reservation AS r ");
            clauseBuilder.Append(@"JOIN h IN c.hotels ");
            clauseBuilder.Append(@"JOIN r IN h.room ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE c.id = @SearchId");
            clauseBuilder.Append(@"AND h.hotelCd = @HotelCode");
            clauseBuilder.Append(@"AND r.roomCode = @RoomCode");
            return clauseBuilder.ToString();
        }
    }
}
