using System.Text;
using Lunggo.ApCommon.Actifity.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
namespace Lunggo.ApCommon.Actifity.Database.Query
{
    internal class GetCityActivitiesQuery : QueryBase<GetCityActivitiesQuery, ActivityType>
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
            clauseBuilder.Append("SELECT DISTINCT Type ");
            clauseBuilder.Append("FROM Activities ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Location like '%' + @city + '%'");
            return clauseBuilder.ToString();
        }
    }
}