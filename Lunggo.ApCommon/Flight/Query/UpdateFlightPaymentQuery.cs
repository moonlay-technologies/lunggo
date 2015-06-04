using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class UpdateFlightPaymentQuery : NoReturnQueryBase<UpdateFlightPaymentQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            if (condition != null)
            {
                queryBuilder.Append(CreateUpdateClause());
                queryBuilder.Append(CreateSetClause(condition));
                queryBuilder.Append(CreateWhereClause());
            }
            return queryBuilder.ToString();
        }

        private static string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE FlightReservation ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET ");
            if (condition.PaymentId != null)
                clauseBuilder.Append(@"PaymentId = @PaymentId, ");
            if (condition.PaymentMediumCd != null)
                clauseBuilder.Append(@"PaymentMediumCd = @PaymentMediumCd, ");
            if (condition.PaymentMethodCd != null)
                clauseBuilder.Append(@"PaymentMethodCd = @PaymentMethodCd, ");
            if (condition.PaymentTime != null)
                clauseBuilder.Append(@"PaymentTime = @PaymentTime, ");
            if (condition.PaymentTargetAccount != null)
                clauseBuilder.Append(@"PaymentTargetAccount = @PaymentTargetAccount, ");
            if (condition.PaymentStatusCd != null)
            {
                clauseBuilder.Append(@"PaymentStatusCd = CASE ");
                clauseBuilder.Append(
                    @"WHEN @PaymentStatusCd = 'SET' THEN ");
                clauseBuilder.Append(
                    @"CASE WHEN ((PaymentStatusCd = 'CAN') OR (PaymentStatusCd = 'PEN') OR (PaymentStatusCd = 'CAP')) ");
                clauseBuilder.Append(
                    @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                    @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                    @"END ");
                clauseBuilder.Append(
                    @"WHEN @PaymentStatusCd = 'PEN' THEN ");
                clauseBuilder.Append(
                    @"CASE WHEN (PaymentStatusCd = 'CAN') ");
                clauseBuilder.Append(
                    @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                    @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                    @"END ");
                clauseBuilder.Append(
                    @"WHEN @PaymentStatusCd = 'CAP' THEN ");
                clauseBuilder.Append(
                    @"CASE WHEN ((PaymentStatusCd = 'CAN') OR (PaymentStatusCd = 'PEN')) ");
                clauseBuilder.Append(
                    @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                    @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                    @"END ");
                clauseBuilder.Append(
                    @"ELSE PaymentStatusCd = @PaymentStatusCd ");
                clauseBuilder.Append(@"END, ");
            }
            clauseBuilder.Remove(clauseBuilder.Length - 2, 2);
            clauseBuilder.Append(" ");
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
