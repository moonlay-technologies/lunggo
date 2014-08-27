using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{

    public class GetUserByEmailQuery : QueryBase<GetUserByEmailQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByEmailQuery()
        {

        }

        protected override string GetQuery()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select * from Users where Email = @Email");
            return queryBuilder.ToString();
        }
    }
				
}
