﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    public class GetRsvNosByBookerId : DbQueryBase<GetRsvNosByBookerId, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            queryBuilder.Append(CreateConditionClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT r.RsvNo ");
            clauseBuilder.Append("FROM Reservation AS r ");
            clauseBuilder.Append("INNER JOIN Payment AS p ON r.RsvNo = p.RsvNo ");
            clauseBuilder.Append("INNER JOIN Contact AS c ON r.RsvNo = c.RsvNo ");
            clauseBuilder.Append("INNER JOIN FlightItinerary AS i ON r.RsvNo = i.RsvNo ");
            clauseBuilder.Append("INNER JOIN FlightTrip AS t ON i.Id = ");
            clauseBuilder.Append("(SELECT TOP 1 t.ItineraryId FROM FlightTrip WHERE t.ItineraryId = i.Id) ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE (r.UserId = @UserId AND r.RsvType = 'AGENT')");
            return clauseBuilder.ToString();
        }

        private static string CreateConditionClause(dynamic condition)
        {
            List<string> filters = null;
            int? page = null;
            int itemsPerPage = 10;
            if (condition != null)
            {
                //page = condition.Page;
                filters = condition.Filters;
                //itemsPerPage = condition.ItemsPerPage;
            }
            var clauseBuilder = new StringBuilder();
            if (filters != null)
            {
                if (filters.Contains("active"))
                    clauseBuilder.Append(" AND (t.DepartureDate >= DATEADD(day,-1,GETUTCDATE())) ");
                if (filters.Contains("inactive"))
                    clauseBuilder.Append(" AND (t.DepartureDate < DATEADD(day,-1,GETUTCDATE())) ");
                if (filters.Contains("issued"))
                    clauseBuilder.Append(" AND i.BookingStatusCd = 'TKTD'");
                if (filters.Contains("pending"))
                    clauseBuilder.Append(" AND r.RsvStatusCd = 'PROC'");
                if (filters.Contains("approved"))
                    clauseBuilder.Append(" AND r.RsvStatusCd = 'APRV'");
                if (filters.Contains("rejected"))
                    clauseBuilder.Append(" AND r.RsvStatusCd = 'REJE'");
            }
            if (page != null)
            {
                clauseBuilder.Append(" ORDER BY r.RsvTime OFFSET " + page * itemsPerPage + " ROWS FETCH NEXT " + itemsPerPage + " ROWS ONLY");
            }
            return clauseBuilder.ToString();
        }
    }
}