using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class InsertFlightTripQuery : QueryBase<InsertFlightTripQuery, OriginDestinationInfo>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateInsertValueClause());
            return queryBuilder.ToString();
        }

        private static string CreateInsertValueClause()
        {
            var insertClause = new StringBuilder();
            var valueClause = new StringBuilder();

            insertClause.Append(@"INSERT INTO FlightReservation (");
            valueClause.Append(@"VALUES (");

            insertClause.Append(@"ItineraryId, ");
            valueClause.Append(@"@ItineraryId, ");

            insertClause.Append(@"OriginAirportCd, ");
            valueClause.Append(@"@Info.OriginAirport, ");

            insertClause.Append(@"DestinationAirportCd, ");
            valueClause.Append(@"@Info.DestinationAirport, ");

            insertClause.Append(@"DepartureTime, ");
            valueClause.Append(@"@Info.DepartureDate, ");

            insertClause.Append(@"InsertBy, ");
            valueClause.Append(@"' ', ");

            insertClause.Append(@"InsertDate, ");
            valueClause.Append(@"07/07/2015, ");

            insertClause.Append(@"InsertPgId, ");
            valueClause.Append(@"' ', ");

            insertClause.Remove(insertClause.Length - 2, 2);
            valueClause.Remove(insertClause.Length - 2, 2);

            insertClause.Append(@") ");
            valueClause.Append(@")");

            return insertClause.Append(valueClause).ToString();
        }
    }
}
