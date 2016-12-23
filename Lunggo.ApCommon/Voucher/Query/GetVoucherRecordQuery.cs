using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Voucher.Query
{
    internal class GetVoucherRecordQuery : DbQueryBase<GetVoucherRecordQuery, VoucherTableRecord>
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
            clauseBuilder.Append("SELECT * ");
            clauseBuilder.Append("FROM Voucher ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE VoucherId = @VoucherCode");
            return clauseBuilder.ToString();
        }
    }
}
