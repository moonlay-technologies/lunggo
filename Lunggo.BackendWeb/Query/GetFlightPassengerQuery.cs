using System.Text;
using Lunggo.BackendWeb.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.BackendWeb.Query
{
    public class GetFlightPassengerQuery : QueryBase<GetFlightPassengerQuery, FlightPassengerTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            if (condition.QueryType == QueryType.PrimKeys)
                queryBuilder.Append(
                    @"SELECT DISTINCT PassengerId ");
            else if (condition.QueryType == QueryType.Overview)
                queryBuilder.Append(
                    @"SELECT DISTINCT PassengerId, FirstName, LastName ");
            else
                queryBuilder.Append(
                    @"SELECT DISTINCT * ");
            queryBuilder.Append(
                @"FROM FlightPassenger ");
            queryBuilder.Append(
                @"WHERE TripId = @TripId ");
            return queryBuilder.ToString();
        }
    }
}
