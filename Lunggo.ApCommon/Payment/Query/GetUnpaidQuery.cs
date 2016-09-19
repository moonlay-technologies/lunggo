using System.Text;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Query
{
    internal class GetUnpaidQuery : QueryBase<GetUnpaidQuery, PaymentTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT RsvNo, TimeLimit, FinalPriceIdr, InsertDate ");
            clauseBuilder.Append(@"FROM Payment ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE StatusCd = 'PEN' AND MethodCd = 'TRF'");
            return clauseBuilder.ToString();
        }
    }
}
