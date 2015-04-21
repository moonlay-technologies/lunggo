using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightPrimKeys : QueryBase<GetFlightPrimKeys, FlightPrimKeys>
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
            clauseBuilder.Append(@"SELECT r.RsvNo, i.ItineraryId, t.TripId, sg.SegmentId, st.StopId, p.PassengerId");
            clauseBuilder.Append(@"FROM FlightItinerary AS i");
            clauseBuilder.Append(@"INNER JOIN FlightReservation AS r ON i.RsvNo = r.RsvNo");
            clauseBuilder.Append(@"INNER JOIN FlightTrip AS t ON t.ItineraryId = i.ItineraryId");
            clauseBuilder.Append(@"INNER JOIN FlightSegment AS sg ON sg.TripId = t.TripId");
            clauseBuilder.Append(@"INNER JOIN FlightStop AS st ON st.SegmentId = sg.SegmentId");
            clauseBuilder.Append(@"INNER JOIN FlightPassenger AS p ON p.RsvNo = r.RsvNo");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE BookingId = @BookingId");
            return clauseBuilder.ToString();
        }
    }
}
