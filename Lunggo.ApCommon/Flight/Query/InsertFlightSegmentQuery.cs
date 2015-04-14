using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class InsertFlightSegmentQuery : QueryBase<InsertFlightSegmentQuery, FlightFareTrip>
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

            insertClause.Append(@"TripId, ");
            valueClause.Append(@"@TripId, ");

            insertClause.Append(@"OperatingAirlineCd, ");
            valueClause.Append(@"@Segment.OperatingAirlineCode, ");

            insertClause.Append(@"AirlineCd, ");
            valueClause.Append(@"@Segment.AirlineCode, ");

            insertClause.Append(@"FlightNumber, ");
            valueClause.Append(@"@Segment.FlightNumber, ");

            insertClause.Append(@"AircraftCd, ");
            valueClause.Append(@"@Segment.AircraftCode, ");

            insertClause.Append(@"DepartureAirportCd, ");
            valueClause.Append(@"@Segment.DepartureAirport, ");

            insertClause.Append(@"ArrivalAirportCd, ");
            valueClause.Append(@"@Segment.ArrivalAirport, ");

            insertClause.Append(@"DepartureTime, ");
            valueClause.Append(@"@Segment.DepartureTime, ");

            insertClause.Append(@"ArrivalTime, ");
            valueClause.Append(@"@Segment.ArrivalTime, ");

            insertClause.Append(@"Duration, ");
            valueClause.Append(@"@Segment.Duration, ");

            insertClause.Append(@"StopQuantity, ");
            valueClause.Append(@"@Segment.StopQuantity, ");

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
