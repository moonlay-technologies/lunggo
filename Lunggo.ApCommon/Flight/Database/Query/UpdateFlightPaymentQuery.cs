using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    public class UpdateFlightPaymentQuery : NoReturnQueryBase<UpdateFlightPaymentQuery>
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
            if (condition.PaymentTimeLimit != null)
                clauseBuilder.Append(@"PaymentTimeLimit = @PaymentTimeLimit, ");
            if (condition.PaymentTargetAccount != null)
                clauseBuilder.Append(@"PaymentTargetAccount = @PaymentTargetAccount, ");
            if (condition.PaymentUrl != null)
                clauseBuilder.Append(@"PaymentUrl = @PaymentUrl, ");
            if (condition.PaymentReceiptUrl != null)
                clauseBuilder.Append(@"PaymentReceiptUrl = @PaymentReceiptUrl, ");
            if (condition.PaidAmount != null)
                clauseBuilder.Append(@"PaidAmount = @PaidAmount, ");
            if (condition.PaymentStatusCd != null)
            {
                clauseBuilder.Append(
                    @"PaymentStatusCd = CASE ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'CAN' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN ((PaymentStatusCd = 'PEN') OR (PaymentStatusCd = 'SET') OR (PaymentStatusCd = 'DEN') OR (PaymentStatusCd IS NULL)) ");
                clauseBuilder.Append(
                                @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                                @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                            @"END ");                
                clauseBuilder.Append(
                        @"WHEN ((@PaymentStatusCd = 'PEN') OR (PaymentStatusCd IS NULL)) THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                        @"WHEN ((@PaymentStatusCd = 'DEN') OR (PaymentStatusCd IS NULL)) THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'SET' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN ((PaymentStatusCd = 'PEN') OR (PaymentStatusCd IS NULL)) ");
                clauseBuilder.Append(
                                @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                                @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'EXP' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN ((PaymentStatusCd = 'PEN') OR (PaymentStatusCd IS NULL)) ");
                clauseBuilder.Append(
                                @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                                @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                    @"END, ");
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
