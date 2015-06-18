using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightTripInfoQuery : QueryBase<GetFlightTripInfoQuery, FlightTripInfo>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT t.OriginAirport, t.DestinationAirport, t.DepartureDate ");
            clauseBuilder.Append("FROM FlightTrip AS t ");
            clauseBuilder.Append("INNER JOIN FlightItinerary AS i ON t.ItineraryId = i.ItineraryId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE i.RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
