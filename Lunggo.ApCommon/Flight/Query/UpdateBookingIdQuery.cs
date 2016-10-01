using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class UpdateBookingIdQuery : NoReturnDbQueryBase<UpdateBookingIdQuery>
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
            clauseBuilder.Append(@"BookingId = @NewBookingId ");
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
