using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class UpdateBookingStatusQuery : QueryBase<UpdateBookingStatusQuery, BookingStatusInfo>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateSetClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE FlightItinerary ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"BookingStatusCd = CASE WHEN @BookingStatus > BookingStatusCd THEN @BookingStatus ELSE BookingStatusCd END, ");
            clauseBuilder.Append(@"TicketTimeLimit = CASE WHEN @BookingStatus = 2 THEN @TimeLimit ELSE NULL END ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"BookingId = @BookingId");
            return clauseBuilder.ToString();
        }
    }
}
