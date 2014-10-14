using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{

    public class GetRoleByNameQuery : QueryBase<GetRoleByNameQuery, GetRoleByAnyQueryRecord>
    {
        private GetRoleByNameQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Roles WHERE Upper(Name) = Upper(@Name)");
            return queryBuilder.ToString();
        }
    }
				
}
