using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    public class UpdatePaymentQuery : NoReturnQueryBase<UpdatePaymentQuery>
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
            if (condition.PaymentMediumCd != null && condition.PaymentMediumCd != "")
                clauseBuilder.Append(@"PaymentMediumCd = @PaymentMediumCd, ");
            if (condition.PaymentMethodCd != null && condition.PaymentMethodCd != "")
                clauseBuilder.Append(@"PaymentMethodCd = @PaymentMethodCd, ");
            if (condition.PaymentTime != null)
                clauseBuilder.Append(@"PaymentTime = @PaymentTime, ");
            if (condition.PaymentTimeLimit != null)
                clauseBuilder.Append(@"PaymentTimeLimit = @PaymentTimeLimit, ");
            if (condition.PaymentTargetAccount != null)
                clauseBuilder.Append(@"PaymentTargetAccount = @PaymentTargetAccount, ");
            if (condition.PaymentUrl != null)
                clauseBuilder.Append(@"PaymentUrl = @PaymentUrl, ");
            if (condition.PaidAmount != null && condition.PaidAmount != 0M)
                clauseBuilder.Append(@"PaidAmount = @PaidAmount, ");
            if (condition.TransferCode != null)
                clauseBuilder.Append(@"TransferCode = @TransferCode, ");
            if (condition.FinalPrice != null)
                clauseBuilder.Append(@"FinalPrice = @FinalPrice, ");
            if (condition.CurrencyCd != null)
                clauseBuilder.Append(@"CurrencyCd = @CurrencyCd, ");
            if (condition.VoucherCode != null)
                clauseBuilder.Append(@"VoucherCode = @VoucherCode, ");
            if (condition.DiscountId != null)
                clauseBuilder.Append(@"DiscountId = @DiscountId, ");
            if (condition.DiscountName != null)
                clauseBuilder.Append(@"DiscountName = @DiscountName, ");
            if (condition.DiscountPercentage != null)
                clauseBuilder.Append(@"DiscountPercentage = @DiscountPercentage, ");
            if (condition.DiscountConstant != null)
                clauseBuilder.Append(@"DiscountConstant = @DiscountConstant, ");
            if (condition.DiscountNominal != null)
                clauseBuilder.Append(@"DiscountNominal = @DiscountNominal, ");
            if (condition.PaymentStatusCd != null)
            {
                clauseBuilder.Append(
                    @"PaymentStatusCd = CASE ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'CAN' THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'PEN' THEN PaymentStatusCd ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'DEN' THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'SET' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN ((PaymentStatusCd = 'PEN') OR (PaymentStatusCd = 'VER') OR (PaymentStatusCd = 'CHA')) ");
                clauseBuilder.Append(
                                @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                                @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'EXP' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN PaymentStatusCd = 'PEN' ");
                clauseBuilder.Append(
                                @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                                @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'VER' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN PaymentStatusCd = 'PEN' ");
                clauseBuilder.Append(
                                @"THEN @PaymentStatusCd ");
                clauseBuilder.Append(
                                @"ELSE PaymentStatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @PaymentStatusCd = 'CHA' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN PaymentStatusCd = 'VER' ");
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
