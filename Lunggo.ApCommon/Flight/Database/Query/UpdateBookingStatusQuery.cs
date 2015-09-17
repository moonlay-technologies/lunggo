using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class UpdateBookingStatusQuery : NoReturnQueryBase<UpdateBookingStatusQuery>
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
            clauseBuilder.Append(@"SET ");
            clauseBuilder.Append(@"BookingStatusCd = CASE ");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'TKTG' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatusCd = 'BOOK') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END ");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'FAIL' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatusCd = 'BOOK') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END ");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'TKTD' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN ((BookingStatusCd = 'BOOK') OR (BookingStatusCd = 'TKTG')) ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END ");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'CANC' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN ((BookingStatusCd = 'BOOK') OR (BookingStatusCd = 'TKTD') OR (BookingStatusCd = 'TKTG') OR (BookingStatusCd = 'CHGD')) ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END ");
            clauseBuilder.Append(
                @"WHEN @BookingStatusCd = 'CHGD' THEN ");
            clauseBuilder.Append(
                    @"CASE WHEN (BookingStatusCd = 'TKTD') ");
            clauseBuilder.Append(
                        @"THEN @BookingStatusCd ");
            clauseBuilder.Append(
                        @"ELSE BookingStatusCd ");
            clauseBuilder.Append(
                    @"END ");
            clauseBuilder.Append(
                @"ELSE BookingStatusCd ");
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
