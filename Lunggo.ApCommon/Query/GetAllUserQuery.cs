using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{

    public class GetAllUserQuery : QueryBase<GetAllUserQuery,GetUserByAnyQueryRecord>
    {
        private GetAllUserQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select * from Users");
            return queryBuilder.ToString();
        }
    }
				
}
