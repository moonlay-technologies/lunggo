using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Query
{
    public class UpdatePaymentQuery : NoReturnDbQueryBase<UpdatePaymentQuery>
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
            clauseBuilder.Append(@"UPDATE Payment ");
            return clauseBuilder.ToString();
        }

        private static string CreateSetClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET ");
            if (condition.MediumCd != null && condition.MediumCd != "")
                clauseBuilder.Append(@"MediumCd = @MediumCd, ");
            if (condition.MethodCd != null && condition.MethodCd != "")
                clauseBuilder.Append(@"MethodCd = @MethodCd, ");
            if (condition.SubMethod != null && condition.SubMethod != "")
                clauseBuilder.Append(@"SubMethod = @SubMethod, ");
            if (condition.Time != null)
                clauseBuilder.Append(@"Time = @Time, ");
            if (condition.TransferAccount != null)
                clauseBuilder.Append(@"TransferAccount = @TransferAccount, ");
            if (condition.TimeLimit != null)
                clauseBuilder.Append(@"TimeLimit = @TimeLimit, ");
            if (condition.RedirectionUrl != null)
                clauseBuilder.Append(@"RedirectionUrl = @RedirectionUrl, ");
            if (condition.PaidAmountIdr != null && condition.PaidAmountIdr != 0M)
                clauseBuilder.Append(@"PaidAmountIdr = @PaidAmountIdr, ");
            if (condition.LocalPaidAmount != null && condition.LocalPaidAmount != 0M)
                clauseBuilder.Append(@"LocalPaidAmount = @LocalPaidAmount, ");
            if (condition.ExternalId != null)
                clauseBuilder.Append(@"ExternalId = @ExternalId, ");
            if (condition.OriginalPriceIdr != null)
                clauseBuilder.Append(@"OriginalPriceIdr = @OriginalPriceIdr, ");
            if (condition.DiscountCode != null)
                clauseBuilder.Append(@"DiscountCode = @DiscountCode, ");
            if (condition.DiscountNominal != null)
                clauseBuilder.Append(@"DiscountNominal = @DiscountNominal, ");
            if (condition.Surcharge != null)
                clauseBuilder.Append(@"Surcharge = @Surcharge, ");
            if (condition.UniqueCode != null)
                clauseBuilder.Append(@"UniqueCode = @UniqueCode, ");
            if (condition.FinalPriceIdr != null)
                clauseBuilder.Append(@"FinalPriceIdr = @FinalPriceIdr, ");
            if (condition.LocalCurrencyCd != null)
                clauseBuilder.Append(@"LocalCurrencyCd = @LocalCurrencyCd, ");
            if (condition.LocalRate != null)
                clauseBuilder.Append(@"LocalRate = @LocalRate, ");
            if (condition.LocalFinalPrice != null)
                clauseBuilder.Append(@"LocalFinalPrice = @LocalFinalPrice, ");
            if (condition.InvoiceNo != null)
                clauseBuilder.Append(@"InvoiceNo = @InvoiceNo, ");
            if (condition.StatusCd != null)
            {
                clauseBuilder.Append(
                    @"StatusCd = CASE ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'CAN' THEN @StatusCd ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'PEN' THEN StatusCd ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'DEN' THEN @StatusCd ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'FAI' THEN @StatusCd ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'SET' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN ((StatusCd = 'PEN') OR (StatusCd = 'VER') OR (StatusCd = 'CHA')) ");
                clauseBuilder.Append(
                                @"THEN @StatusCd ");
                clauseBuilder.Append(
                                @"ELSE StatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'EXP' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN ((StatusCd = 'PEN') OR (StatusCd = 'VER') OR (StatusCd = 'CHA')) ");
                clauseBuilder.Append(
                                @"THEN @StatusCd ");
                clauseBuilder.Append(
                                @"ELSE StatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'VER' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN StatusCd = 'PEN' ");
                clauseBuilder.Append(
                                @"THEN @StatusCd ");
                clauseBuilder.Append(
                                @"ELSE StatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"WHEN @StatusCd = 'CHA' THEN ");
                clauseBuilder.Append(
                            @"CASE WHEN StatusCd = 'VER' ");
                clauseBuilder.Append(
                                @"THEN @StatusCd ");
                clauseBuilder.Append(
                                @"ELSE StatusCd ");
                clauseBuilder.Append(
                            @"END ");
                clauseBuilder.Append(
                        @"ELSE StatusCd ");
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
