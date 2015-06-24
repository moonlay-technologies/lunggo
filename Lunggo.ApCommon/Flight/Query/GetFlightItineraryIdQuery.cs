using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightItineraryIdQuery : QueryBase<GetFlightItineraryIdQuery, long>
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
            clauseBuilder.Append("SELECT ItineraryId ");
            clauseBuilder.Append("FROM FlightItinerary ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE BookingId = @BookingId");
            return clauseBuilder.ToString();
        }
    }
}
