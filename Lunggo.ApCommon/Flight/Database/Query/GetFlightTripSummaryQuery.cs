﻿using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetFlightTripSummaryQuery : QueryBase<GetFlightTripSummaryQuery, FlightTripTableRecord>
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
            clauseBuilder.Append(@"SELECT t.TripId, t.OriginAirportCd, t.DestinationAirportCd ");
            clauseBuilder.Append(@"FROM FlightTrip AS t ");
            clauseBuilder.Append(@"INNER JOIN FlightItinerary AS i ON t.ItineraryId = i.ItineraryId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE i.RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}