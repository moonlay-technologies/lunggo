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
            clauseBuilder.Append(@"SET BookingStatusCd = CASE ");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'TKTG' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatusCd = 'BOOK') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'TKTD' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN ((BookingStatusCd = 'BOOK') OR (BookingStatusCd = 'TKTG')) ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'CANC' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatusCd = 'BOOK') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END");
            clauseBuilder.Append(
                @"ELSE BookingStatusCd = @BookingStatusCd ");
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
