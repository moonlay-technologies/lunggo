using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    class GetRsvNosByContactEmailQuery : QueryBase<GetRsvNosByContactEmailQuery, string>
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
            clauseBuilder.Append("SELECT RsvNo ");
            clauseBuilder.Append("FROM Contact ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Email = @Email");
            return clauseBuilder.ToString();
        }
    }
}
