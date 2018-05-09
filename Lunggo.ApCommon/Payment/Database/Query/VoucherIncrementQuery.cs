using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class VoucherIncrementQuery : ExecuteNonQueryBase<VoucherIncrementQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateSetClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE CampaignVoucher ");
            return clauseBuilder.ToString();
        }

        private string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET [RemainingCount] = ([RemainingCount]+1) ");
            return clauseBuilder.ToString();
        }

        private string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE [VoucherCode] = @VoucherCode ");
            return clauseBuilder.ToString();
        }
    }
}
