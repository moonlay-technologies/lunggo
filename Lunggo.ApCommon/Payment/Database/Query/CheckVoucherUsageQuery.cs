using System.Text;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class CheckVoucherUsageQuery : DbQueryBase<CheckVoucherUsageQuery, int>
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
            clauseBuilder.Append("SELECT COUNT(p.RsvNo) ");
            clauseBuilder.Append("FROM [Reservation] AS r ");
            clauseBuilder.Append("INNER JOIN [Payment] AS p ON p.RsvNo = r.RsvNo ");
            clauseBuilder.Append("LEFT OUTER JOIN [User] AS u ON u.Id = r.UserId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE p.[DiscountCode] = @VoucherCode ");
            clauseBuilder.Append("AND u.[Email] = @Email ");
            clauseBuilder.Append("AND p.[StatusCd] <> '" + PaymentStatusCd.Mnemonic(PaymentStatus.Expired) + "'");
            return clauseBuilder.ToString();
        }
    }
}
