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
    public class GetFlightTripQuery : QueryBase<GetFlightTripQuery, FlightTripTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                condition == FlightReservationIntegrated.QueryType.Overview
                    ? @"SELECT TripId, BookingNumber "
                    : @"SELECT * ");
            queryBuilder.Append(
                @"FROM FlightTrip ");
            queryBuilder.Append(
                @"WHERE RsvNo = @RsvNo ");
            queryBuilder.Append(
                @"ORDER BY BookingNumber");
            return queryBuilder.ToString();
        }
    }
}
