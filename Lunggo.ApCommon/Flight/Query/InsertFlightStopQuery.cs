using System;
using System.Text;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class InsertFlightStopQuery : QueryBase<InsertFlightStopQuery, FlightStopQueryRecord>
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

            insertClause.Append(@"SegmentId, ");
            valueClause.Append(@"@SegmentId, ");

            insertClause.Append(@"AirportCd, ");
            valueClause.Append(@"@Stop.Airport, ");

            insertClause.Append(@"ArrivalTime, ");
            valueClause.Append(@"@Stop.Arrival, ");

            insertClause.Append(@"DepartureTime, ");
            valueClause.Append(@"@Stop.Departure, ");

            insertClause.Append(@"Duration, ");
            valueClause.Append(@"@Stop.Duration, ");

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
