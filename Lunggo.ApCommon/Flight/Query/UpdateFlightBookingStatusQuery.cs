using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class UpdateFlightBookingStatusQuery : NoReturnQueryBase<UpdateFlightBookingStatusQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateSetClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE FlightItinerary ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET BookingStatus = CASE ");
            clauseBuilder.Append(
                @"WHEN @BookingStatus = 'TKTG' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatus = 'BOOK') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatus ");
            clauseBuilder.Append(
                        @"ELSE BookingStatus ");
            clauseBuilder.Append(
                    @"END");
            clauseBuilder.Append(
                @"WHEN @BookingStatus = 'TKTD' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN ((BookingStatus = 'BOOK') OR (BookingStatus = 'TKTG')) ");
            clauseBuilder.Append(
                        @"THEN @BookingStatus ");
            clauseBuilder.Append(
                        @"ELSE BookingStatus ");
            clauseBuilder.Append(
                    @"END");
            clauseBuilder.Append(
                @"WHEN @BookingStatus = 'CANC' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatus = 'BOOK') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatus ");
            clauseBuilder.Append(
                        @"ELSE BookingStatus ");
            clauseBuilder.Append(
                    @"END");
            clauseBuilder.Append(
                @"ELSE BookingStatus = BookingStatus ");
            clauseBuilder.Append(@"END ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE BookingId = @BookingId");
            return clauseBuilder.ToString();
        }
    }
}
