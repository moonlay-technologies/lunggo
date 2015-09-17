using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class DeleteTripsPerItineraryQuery : NoReturnQueryBase<DeleteTripsPerItineraryQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateDeleteClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateDeleteClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"DELETE t ");
            clauseBuilder.Append(@"FROM FlightTrip AS t ");
            clauseBuilder.Append(@"INNER JOIN FlightItinerary AS i ON t.ItineraryId = i.ItineraryId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"i.BookingId = @BookingId");
            return clauseBuilder.ToString();
        }
    }
}
