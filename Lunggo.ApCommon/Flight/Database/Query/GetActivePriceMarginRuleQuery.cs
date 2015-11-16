using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetActivePriceMarginRuleQuery : QueryBase<GetActivePriceMarginRuleQuery, FlightPriceMarginRuleTableRecord>
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
            clauseBuilder.Append("FROM FlightPriceMarginRule ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE IsActive = 1");
            return clauseBuilder.ToString();
        }
    }
}
