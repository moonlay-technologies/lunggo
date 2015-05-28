using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    public class UpdateFlightPaymentQuery : NoReturnQueryBase<UpdateFlightPaymentQuery>
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
            clauseBuilder.Append(@"UPDATE FlightReservation ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET PaymentStatusCd = CASE ");
            clauseBuilder.Append(
                @"WHEN @BookingStatus = 'CAP' THEN ");
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
            clauseBuilder.Append("WHERE RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
