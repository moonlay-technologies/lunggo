using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.Flight.Query
{
    public class GetFlightPassengerQuery : QueryBase<GetFlightPassengerQuery, FlightPassengerTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            if (condition.QueryType == FlightReservationIntegrated.QueryType.PrimKeys)
                queryBuilder.Append(
                    @"SELECT DISTINCT PassengerId ");
            else if (condition.QueryType == FlightReservationIntegrated.QueryType.Overview)
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
