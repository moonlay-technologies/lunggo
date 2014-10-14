using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{

    public class GetRoleByIdQuery : QueryBase<GetRoleByIdQuery, GetRoleByAnyQueryRecord>
    {
        private GetRoleByIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Roles WHERE Id = @Id");
            return queryBuilder.ToString();
        }
    }
				
}
