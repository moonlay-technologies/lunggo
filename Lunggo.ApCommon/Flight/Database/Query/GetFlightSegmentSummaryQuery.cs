using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetFlightSegmentSummaryQuery : QueryBase<GetFlightSegmentSummaryQuery, FlightSegmentTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT AirlineCd, FlightNumber, OperatingAirlineCd, DepartureAirportCd, ArrivalAirportCd, DepartureTime, ArrivalTime, DepartureTerminal, ArrivalTerminal ");
            clauseBuilder.Append(@"FROM FlightSegment ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE TripId = @TripId");
            return clauseBuilder.ToString();
        }
    }
}
