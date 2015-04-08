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
    public class GetFlightReservationQuery : QueryBase<GetFlightReservationQuery, FlightReservationsTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                condition == FlightReservationIntegrated.QueryType.Overview
                    ? @"SELECT RsvNo, RsvTime, ContactName, RsvStatusCd "
                    : @"SELECT * ");
            queryBuilder.Append(
                @"FROM FlightReservations ");
            queryBuilder.Append(
                @"WHERE RsvNo = @RsvNo");
            return queryBuilder.ToString();
        }
    }
}
