using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetFlightBookingStatusQuery : QueryBase<GetFlightBookingStatusQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClauser());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT BookingStatus FROM FlightItinerary ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClauser()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE BookingId = @BookingId");
            return clauseBuilder.ToString();
        }
    }
}
