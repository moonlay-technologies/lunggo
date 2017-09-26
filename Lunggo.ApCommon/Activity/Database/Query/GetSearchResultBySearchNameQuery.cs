using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Activity.Query
{
    internal class GetSearchResultBySearchNameQuery : DbQueryBase<GetSearchResultBySearchNameQuery, ActivityTableRecord>
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
            clauseBuilder.Append("SELECT * FROM Activity ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Name LIKE '%' + @Name + '%'");
            return clauseBuilder.ToString();
        }
        
    }
}
